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
    }
}
