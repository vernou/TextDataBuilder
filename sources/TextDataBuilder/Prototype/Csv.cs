using System.Text;
using Microsoft.VisualBasic.FileIO ;

namespace TextDataBuilder.Prototype
{
    public class Csv : IPrototype
    {
        private readonly string path;
        private readonly string format;
        private readonly string join;

        public Csv(string path, string format, string join)
        {
            this.path = path;
            this.format = format;
            this.join = join;
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
                        build.AppendLine(join);
                }
            }
            return build.ToString();
        }
    }
}