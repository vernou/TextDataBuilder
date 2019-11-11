namespace TextDataBuilder.Prototype
{
    public class StaticText : IPrototype
    {
        private readonly string text;

        public StaticText(string text)
        {
            this.text = text;
        }

        public string Build() => text;
    }
}