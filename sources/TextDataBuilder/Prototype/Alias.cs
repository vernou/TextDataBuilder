using System;

namespace TextDataBuilder.Prototype
{
    public class Alias : IPrototype
    {
        private readonly Lazy<string> value;

        public Alias(IPrototype prototype)
        {
            value = new Lazy<string>(prototype.Build);
        }

        public string Build()
        {
            return value.Value.ToString();
        }
    }
}