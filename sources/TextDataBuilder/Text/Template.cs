using System.IO;
using System.Text;

namespace TextDataBuilder.Text
{
    public class Template : IText
    {
        private const string TagStartToken = "@{";
        private const string TagEndToken = "}";
        private readonly TextReader reader;

        public Template(TextReader reader)
        {
            this.reader = reader;
        }

        public void Print(StringBuilder output)
        {
            var line = reader.ReadLine();
            while(line != null)
            {
                var indexOfTagStart = line.IndexOf(TagStartToken);
                if(indexOfTagStart >= 0)
                {
                    output.Append(line.Substring(0, indexOfTagStart));
                    var indexOfTagBodyStart = indexOfTagStart + TagStartToken.Length;
                    var indexOfTagEnd = line.IndexOf("}", indexOfTagBodyStart);
                    if(indexOfTagEnd >= 0)
                    {
                        var indexOfTagBodyEnd = indexOfTagEnd - 1;
                        var tag = new Parser.Tag(Substring(line, indexOfTagBodyStart, indexOfTagBodyEnd));
                        output.Append(tag.Name);
                        output.Append(Substring(line, indexOfTagEnd + 1));
                    }
                }
                else
                {
                    output.Append(line);
                }
                line = reader.ReadLine();
                if(line != null)
                    output.AppendLine();
            }
        }

        private static string Substring(string str, int start)
        {
            return Substring(str, start, str.Length - 1);
        }

        private static string Substring(string str, int start, int end)
        {
            if(start > end)
                return string.Empty;
            return str.Substring(start, end - start + 1);
        }
    }
}