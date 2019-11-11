using System.Text;
using Microsoft.VisualBasic.FileIO ;

namespace TextDataBuilder.Prototype
{
    public class Csv : IPrototype
    {
        private readonly string path;
        private readonly string format;

        public Csv(string path, string format)
        {
            this.path = path;
            this.format = format;
        }

        public string Build()
        {
            var build = new StringBuilder();
            using(var parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });

                while (!parser.EndOfData)
                {
                    build.Append(string.Format(format, parser.ReadFields()));
                    if(!parser.EndOfData)
                        build.AppendLine();
                }
            }
            return build.ToString();
        }
    }
}