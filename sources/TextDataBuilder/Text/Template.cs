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
        private readonly Dictionary<string, IText> tags = new Dictionary<string, IText>();
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
            var broswer = new Browser(reader.ReadToEnd());
            while(broswer.CursorIsIn)
            {
                while(broswer.CursorIsIn && !broswer.StartWith(TagStartToken))
                    broswer.Move();
                output.Append(broswer.Read());
                if(broswer.CursorIsIn)
                {
                    broswer.Move(TagStartToken.Length);
                    broswer.JumpReaderCursorToCursor();
                    while(broswer.CursorIsIn && !broswer.StartWith(TagEndToken))
                        broswer.Move();
                    if(!broswer.CursorIsIn)
                        throw new InvalidOperationException($"Miss '{TagEndToken}'");
                    var tag = new Tag(broswer.Read());
                    PrintTag(output, tag);
                    broswer.Move(TagEndToken.Length);
                    broswer.JumpReaderCursorToCursor();
                }
            }
        }

        public void Reprint(StringBuilder output)
        {
            throw new NotImplementedException();
        }

        private void PrintTag(StringBuilder output, Tag tag)
        {
            if(tag.Name == nameof(RandomInteger))
            {
                PrintRandomInteger(output, tag);
            }
            else if(tags.ContainsKey(tag.Name))
            {
                tags[tag.Name].Reprint(output);
            }
            else
            {
                output.Append(tag.Name);
            }
        }

        private void PrintRandomInteger(StringBuilder output, Tag tag)
        {
            var text = CreateRandomInteger(tag);
            text.Print(output);
            if(tag.Alias != string.Empty)
            tags.Add(tag.Alias, text);
        }

        private IText CreateRandomInteger(Tag tag)
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
            return new RandomInteger(dice, min, max);
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