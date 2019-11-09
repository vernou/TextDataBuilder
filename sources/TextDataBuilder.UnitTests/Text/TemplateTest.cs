using System;
using Xunit;
using TextDataBuilder.Text;
using System.IO;
using System.Text;
using TextDataBuilder.UnitTests.Core;

namespace TextDataBuilder.UnitTests.Text
{
    public class TemplateTest
    {
        [Fact]
        public void OutputRawText()
        {
            var result = new StringBuilder();
            new Template(new StringReader("Raw text")).Print(result);
            Assert.Equal("Raw text", result.ToString());
        }

        [Fact]
        public void OutputRawTextWithBreakLine()
        {
            var result = new StringBuilder();
            new Template(new StringReader("First Line" + Environment.NewLine + "Second Line")).Print(result);
            Assert.Equal("First Line" + Environment.NewLine + "Second Line", result.ToString());
        }

        [Fact]
        public void OutputTagName()
        {
            var result = new StringBuilder();
            new Template(new StringReader("@{FirstTag}")).Print(result);
            Assert.Equal("FirstTag", result.ToString());
        }

        [Fact]
        public void OutputRandomInteger()
        {
            var dice = new RiggedDice(42);
            var result = new StringBuilder();
            new Template(new StringReader("@{RandomInteger}"), dice).Print(result);
            Assert.Equal("42", result.ToString());
        }

        [Fact]
        public void OutputRandomIntegerWithParameters()
        {
            var dice = new RiggedDice(42);
            var result = new StringBuilder();
            new Template(new StringReader("A random integer : @{RandomInteger Min=42, Max=42}"), dice).Print(result);
            Assert.Equal("A random integer : 42", result.ToString());
        }

        [Fact]
        public void ReprintFromTagAlias()
        {
            var dice = new RiggedDice(42);
            var result = new StringBuilder();
            new Template(new StringReader("Tow identical random integers : @{RandomInteger As=Rand, Min=42, Max=42} and @{Rand}"), dice).Print(result);
            Assert.Equal("Tow identical random integers : 42 and 42", result.ToString());
        }

        [Fact]
        public void PrintTwoTagsOneLine()
        {
            var result = new StringBuilder();
            new Template(new StringReader("@{FirstTag} and @{SecondTag}")).Print(result);
            Assert.Equal("FirstTag and SecondTag", result.ToString());
        }

        [Fact]
        public void PrintCsvTag()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, "Value1,Value2,Value3" + Environment.NewLine + "Value4,Value5,Value6");

            var result = new StringBuilder();
            new Template(
                new StringReader(
                    "@{CSV Path=\"" + path + "\"}" + Environment.NewLine +
                    "Column 1 : {0}, Column 2 : {1}, Column 3 : {2}" + Environment.NewLine +
                    "@{EndCSV}"
                )
            ).Print(result);

            Assert.Equal(
                "Column 1 : Value1, Column 2 : Value2, Column 3 : Value3" + Environment.NewLine +
                "Column 1 : Value4, Column 2 : Value5, Column 3 : Value6",
                result.ToString()
            );
        }

        [Fact]
        public void PrintSqlInsertFromCsv()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, "Value1,Value2,Value3" + Environment.NewLine + "Value4,Value5,Value6");

            var result = new StringBuilder();
            new Template(
                new StringReader(
                    "INSERT INTO MyTable (Col1, Col2, Col3)" + Environment.NewLine +
                    "VALUES" + Environment.NewLine +
                    "@{CSV Path=\"" + path + "\"}" + Environment.NewLine +
                    "({0}, {1}, {2})," + Environment.NewLine +
                    "@{EndCSV}"
                )
            ).Print(result);

            Assert.Equal(
                "INSERT INTO MyTable (Col1, Col2, Col3)" + Environment.NewLine +
                "VALUES" + Environment.NewLine +
                "(Value1, Value2, Value3)," + Environment.NewLine +
                "(Value4, Value5, Value6),",
                result.ToString()
            );
        }
    }
}
