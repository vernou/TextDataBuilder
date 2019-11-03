using System.Text;
using TextDataBuilder.Text;
using TextDataBuilder.UnitTests.Core;
using Xunit;

namespace TextDataBuilder.UnitTests.Text
{
    public class RandomIntegerTest
    {
        [Fact]
        public void TestName()
        {
            var dice = new RiggedDice(42);
            var text = new RandomInteger(dice, 0, 12);
            var result = new StringBuilder();
            text.Print(result);
            Assert.Equal("42", result.ToString());
        }
    }
}