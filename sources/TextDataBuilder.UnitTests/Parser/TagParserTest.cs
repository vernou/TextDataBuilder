using System;
using System.Linq;
using TextDataBuilder.Parser;
using Xunit;

namespace TextDataBuilder.UnitTests.Parser
{
    public class TagParserTest
    {
        private static Tag ParseTag(string text)
        {
            return new TagParser().Parse(new Browser(text));
        }

        [Fact]
        public void RetreiveNameOfTag()
        {
            Assert.Equal("Simple", ParseTag("@{Simple}").Name);
        }

        [Fact]
        public void RetreiveNameOfTagWithSpace()
        {
            Assert.Equal("Simple", ParseTag("@{  Simple  }").Name);
        }

        [Fact]
        public void TagNameWithInvalidCharacterThrowException()
        {
            Assert.Throws<InvalidOperationException>(()=> ParseTag("@{N@me}").Name);
        }

        [Fact]
        public void EmptyTagNameThrowException()
        {
            Assert.Throws<InvalidOperationException>(()=> ParseTag("@{}").Name);
        }

        [Fact]
        public void TagWithOneParameter()
        {
            var tag = ParseTag("@{TagName Parameter=Value}");
            Assert.True(tag.Parameters.ContainsKey("Parameter"));
            Assert.Equal("Value", tag.Parameters["Parameter"]);
        }

        [Fact]
        public void TagWithTwoParameter()
        {
            var tag = ParseTag("TagName Parameter1=Value1,Parameter2=Value2");
            var parameter = tag.Parameters.First();
            Assert.True(tag.Parameters.ContainsKey("Parameter1"));
            Assert.Equal("Value1", tag.Parameters["Parameter1"]);
            Assert.True(tag.Parameters.ContainsKey("Parameter2"));
            Assert.Equal("Value2", tag.Parameters["Parameter2"]);
        }

        [Fact]
        public void TagWithTwoParameterWithSpace()
        {
            var tag = ParseTag("  TagName  Parameter1=Value1  ,  Parameter2=Value2  ");
            var parameter = tag.Parameters.First();
            Assert.True(tag.Parameters.ContainsKey("Parameter1"));
            Assert.Equal("Value1", tag.Parameters["Parameter1"]);
            Assert.True(tag.Parameters.ContainsKey("Parameter2"));
            Assert.Equal("Value2", tag.Parameters["Parameter2"]);
        }

        [Fact]
        public void TagWithOneParameterWithSpaceInValue()
        {
            var tag = ParseTag(@"TagName Parameter="" V a l u e """);
            Assert.True(tag.Parameters.ContainsKey("Parameter"));
            Assert.Equal(" V a l u e ", tag.Parameters["Parameter"]);
        }

        [Fact]
        public void TagWithoutAlias()
        {
            Assert.Equal(string.Empty, ParseTag("@{TagName}").Alias);
        }

        [Fact]
        public void TagWithtAlias()
        {
            Assert.Equal("TagAlias", ParseTag("@{TagName As=TagAlias}").Alias);
        }
    }
}