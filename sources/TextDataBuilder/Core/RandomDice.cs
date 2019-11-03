using System;

namespace TextDataBuilder.Core
{
    public class Dice : IDice
    {
        private readonly Random random = new Random();
        public int Roll(int min, int max) => random.Next(min, max);
    }
}