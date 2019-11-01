using System;
using System.Text.RegularExpressions;

namespace TextDataBuilder.Parser
{
    public class Tag
    {
        private readonly string text;

        public Tag(string text)
        {
            this.text = text;
        }

        public string Name
        {
            get
            {
                var name = text.Trim();
                if(Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
                    return text.Trim();
                throw new InvalidOperationException($"The prototype name '{name}' is invalid. Valid characters are [a-zA-Z0-9]");
            }
        }
    }
}