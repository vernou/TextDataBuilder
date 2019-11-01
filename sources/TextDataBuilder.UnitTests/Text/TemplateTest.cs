using System;
using Xunit;
using TextDataBuilder.Text;
using System.IO;
using System.Text;

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
    }
}
