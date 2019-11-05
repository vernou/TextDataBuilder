using System;
using System.Linq;
using TextDataBuilder.Parser;
using Xunit;

namespace TextDataBuilder.UnitTests.Parser
{
    public class TagTest
    {
        [Fact]
        public void RetreiveNameOfTag()
        {
            Assert.Equal("Simple", new Tag("Simple").Name);
        }

        [Fact]
        public void RetreiveNameOfTagWithSpace()
        {
            Assert.Equal("Simple", new Tag("  Simple  ").Name);
        }

        [Fact]
        public void TagNameWithInvalidCharacterThrowException()
        {
            Assert.Throws<InvalidOperationException>(()=> new Tag("N@me").Name);
        }

        [Fact]
        public void EmptyTagNameThrowException()
        {
            Assert.Throws<InvalidOperationException>(()=> new Tag("").Name);
        }

        [Fact]
        public void TagWithOneParameter()
        {
            var tag = new Tag("TagName Parameter=Value");
            Assert.True(tag.Parameters.ContainsKey("Parameter"));
            Assert.Equal("Value", tag.Parameters["Parameter"]);
        }

        [Fact]
        public void TagWithTwoParameter()
        {
            var tag = new Tag("TagName Parameter1=Value1,Parameter2=Value2");
            var parameter = tag.Parameters.First();
            Assert.True(tag.Parameters.ContainsKey("Parameter1"));
            Assert.Equal("Value1", tag.Parameters["Parameter1"]);
            Assert.True(tag.Parameters.ContainsKey("Parameter2"));
            Assert.Equal("Value2", tag.Parameters["Parameter2"]);
        }

        [Fact]
        public void TagWithTwoParameterWithSpace()
        {
            var tag = new Tag("  TagName  Parameter1=Value1  ,  Parameter2=Value2  ");
            var parameter = tag.Parameters.First();
            Assert.True(tag.Parameters.ContainsKey("Parameter1"));
            Assert.Equal("Value1", tag.Parameters["Parameter1"]);
            Assert.True(tag.Parameters.ContainsKey("Parameter2"));
            Assert.Equal("Value2", tag.Parameters["Parameter2"]);
        }

        [Fact]
        public void TagWithOneParameterWithSpaceInValue()
        {
            var tag = new Tag(@"TagName Parameter="" V a l u e """);
            Assert.True(tag.Parameters.ContainsKey("Parameter"));
            Assert.Equal(" V a l u e ", tag.Parameters["Parameter"]);
        }

        [Fact]
        public void TagWithoutAlias()
        {
            Assert.Equal(string.Empty, new Tag("TagName").Alias);
        }

        [Fact]
        public void TagWithtAlias()
        {
            Assert.Equal("TagAlias", new Tag("TagName As=TagAlias").Alias);
        }
    }
}