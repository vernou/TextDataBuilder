using System;
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
            Assert.Equal("Simple", new Tag("Simple").Name);
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
    }
}