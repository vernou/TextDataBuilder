using TextDataBuilder.Core;

namespace TextDataBuilder.Prototype
{
    public class RandomInteger : IPrototype
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

        public string Build() => dice.Roll(min, max).ToString();
    }
}