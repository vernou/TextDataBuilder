using TextDataBuilder.UnitTests.Core;
using TextDataBuilder.Parser;
using Xunit;
using System;
using System.IO;

namespace TextDataBuilder.UnitTests.Parser
{
    public class TemplateParserTest
    {
        [Fact]
        public void OutputRawText()
        {
            Assert.Equal(
                "Raw text",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(new Browser("Raw text")).Build()
            );
        }

        [Fact]
        public void OutputRawTextWithBreakLine()
        {
            Assert.Equal(
                "First Line" + Environment.NewLine + "Second Line",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser("First Line" + Environment.NewLine + "Second Line")
                ).Build()
            );
        }

        [Fact]
        public void OutputTagName()
        {
            Assert.Equal(
                "FirstTag",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser("@{FirstTag}")
                ).Build()
            );
        }

        [Fact]
        public void OutputRandomInteger()
        {
            Assert.Equal(
                "42",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser("@{RandomInteger}")
                ).Build()
            );
        }

        [Fact]
        public void OutputRandomIntegerWithParameters()
        {
            Assert.Equal(
                "A random integer : 42",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser("A random integer : @{RandomInteger Min=42, Max=42}")
                ).Build()
            );
        }

        [Fact]
        public void PrintTwoTagsOneLine()
        {
            Assert.Equal(
                "FirstTag and SecondTag",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser("@{FirstTag} and @{SecondTag}")
                ).Build()
            );
        }

        [Fact]
        public void PrintCsvTag()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(
                path,
                "Value1,Value2,Value3" + Environment.NewLine + 
                "Value4,Value5,Value6"
            );

            Assert.Equal(
                "Column 1 : Value1, Column 2 : Value2, Column 3 : Value3" + Environment.NewLine +
                "Column 1 : Value4, Column 2 : Value5, Column 3 : Value6",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser(
                        "@{CSV Path=\"" + path + "\"}" + Environment.NewLine +
                        "Column 1 : {0}, Column 2 : {1}, Column 3 : {2}" + Environment.NewLine +
                        "@{EndCSV}"
                    )
                ).Build()
            );
        }

        [Fact]
        public void PrintSqlInsertFromCsv()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path,
                "Value1,Value2,Value3" + Environment.NewLine + 
                "Value4,Value5,Value6"
            );

            Assert.Equal(
                "INSERT INTO MyTable (Col1, Col2, Col3)" + Environment.NewLine +
                "VALUES" + Environment.NewLine +
                "(Value1, Value2, Value3)," + Environment.NewLine +
                "(Value4, Value5, Value6),",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser(
                        "INSERT INTO MyTable (Col1, Col2, Col3)" + Environment.NewLine +
                        "VALUES" + Environment.NewLine +
                        "@{CSV Path=\"" + path + "\"}" + Environment.NewLine +
                        "({0}, {1}, {2})," + Environment.NewLine +
                        "@{EndCSV}"
                    )
                ).Build()
            );
        }

        [Fact]
        public void ReprintFromTagAlias()
        {
            Assert.Equal(
                "Tow identical random integers : 42 and 42",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser(
                        "Tow identical random integers : " +
                        "@{RandomInteger As=Rand, Min=42, Max=42} and @{Rand}"
                    )
                ).Build()
            );
        }

        [Fact]
        public void PrintRawText()
        {
            Assert.Equal(
                "Raw text : Poppey",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser(
                        @"Raw text : @{Text Raw=""Poppey""}"
                    )
                ).Build()
            );
        }

        [Fact]
        public void PrintRawTextWithAlias()
        {
            Assert.Equal(
                "Raw text : Poppey Poppey",
                new TemplateParser(
                    new RiggedDice(42)
                ).Parse(
                    new Browser(
                        @"Raw text : @{Text As=Raw, Raw=""Poppey""} @{Raw}"
                    )
                ).Build()
            );
        }
    }
}