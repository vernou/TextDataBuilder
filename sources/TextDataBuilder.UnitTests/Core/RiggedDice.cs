using TextDataBuilder.Core;

namespace TextDataBuilder.UnitTests.Core
{
    public class RiggedDice : IDice
    {
        private readonly int desired;

        public RiggedDice(int desired)
        {
            this.desired = desired;
        }

        public int Roll(int min, int max)
        {
            return desired;
        }
    }
}