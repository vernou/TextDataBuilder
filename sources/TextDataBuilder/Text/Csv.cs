using System.Text;
using Microsoft.VisualBasic.FileIO ;

namespace TextDataBuilder.Text
{
    public class Csv : IText
    {
        private readonly string path;
        private readonly string format;

        public Csv(string path, string format)
        {
            this.path = path;
            this.format = format;
        }

        public void Print(StringBuilder output)
        {
            using(var parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });

                while (!parser.EndOfData)
                {
                    output.Append(string.Format(format, parser.ReadFields()));
                    if(!parser.EndOfData)
                        output.AppendLine();
                }
            }
        }

        public void Reprint(StringBuilder output)
        {
            throw new System.NotImplementedException();
        }
    }
}