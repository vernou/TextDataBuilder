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

        public int Value { get; private set; }

        public void Print(StringBuilder output)
        {
            Value = dice.Roll(min, max);
            output.Append(Value);
        }

        public void Reprint(StringBuilder output)
        {
            output.Append(Value);
        }
    }
}