using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextDataBuilder.Core;
using TextDataBuilder.Parser;

namespace TextDataBuilder.Text
{
    public class Template : IText
    {
        private const string TagStartToken = "@{";
        private const string TagEndToken = "}";
        private readonly TextReader reader;
        private readonly IDice dice;

        public Template(TextReader reader, IDice dice)
        {
            this.reader = reader;
            this.dice = dice;
        }

        public Template(TextReader reader)
            : this(reader, new Dice())
        { }

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
                        var tag = new Tag(Substring(line, indexOfTagBodyStart, indexOfTagBodyEnd));
                        PrintTag(output, tag);
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

        private void PrintTag(StringBuilder output, Tag tag)
        {
            if(tag.Name == nameof(RandomInteger))
            {
                var min = 0;
                var max = int.MaxValue;
                string? value = string.Empty;
                if(tag.Parameters.TryGetValue("Min", out value))
                {
                    if(!int.TryParse(value, out min))
                        throw new InvalidOperationException("The 'Min' parameter's value is invalid.");
                }
                if(tag.Parameters.TryGetValue("Max", out value))
                {
                    if(!int.TryParse(value, out max))
                        throw new InvalidOperationException("The 'Max' parameter's value is invalid.");
                }
                var text = new RandomInteger(dice, min, max);
                text.Print(output);
            }
            else
            {
                output.Append(tag.Name);
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