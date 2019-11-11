using System.Text;
using TextDataBuilder.Prototype;
using TextDataBuilder.UnitTests.Core;
using Xunit;

namespace TextDataBuilder.UnitTests.Prototype
{
    public class RandomIntegerTest
    {
        [Fact]
        public void TestName()
        {
            Assert.Equal(
                "42", 
                new RandomInteger(new RiggedDice(42), 0, 12).Build()
            );
        }
    }
}