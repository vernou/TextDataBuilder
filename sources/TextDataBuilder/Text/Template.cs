using System.IO;
using System.Text;

namespace TextDataBuilder.Text
{
    public class Template : IText
    {
        private readonly TextReader reader;

        public Template(TextReader reader)
        {
            this.reader = reader;
        }

        public void Print(StringBuilder output)
        {
            output.Append(reader.ReadToEnd());
        }
    }
}