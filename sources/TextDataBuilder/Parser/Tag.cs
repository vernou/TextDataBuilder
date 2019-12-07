using System.Collections.Generic;

namespace TextDataBuilder.Parser
{
    public class Tag
    {
        public Tag(string name, IReadOnlyDictionary<string, string> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }
        public IReadOnlyDictionary<string, string> Parameters { get; }
        public string Alias => Parameters.ContainsKey("As") ? Parameters["As"] : string.Empty;
    }
}