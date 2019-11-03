using System.Text;
using TextDataBuilder.Core;

namespace TextDataBuilder.Text
{
    public class RandomInteger : IText
    {
        private readonly IDice dice;
        private readonly int min;
        private readonly int max;

        public RandomInteger(IDice dice, int min, int max)
        {
            this.dice = dice;
            this.min = min;
            this.max = max;
        }

        public void Print(StringBuilder output)
        {
            output.Append(dice.Roll(min, max));
        }
    }
}