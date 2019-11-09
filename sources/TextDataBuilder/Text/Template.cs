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
            var browser = new Browser(reader.ReadToEnd());
            while(browser.CursorIsIn)
            {
                while(browser.CursorIsIn && !browser.StartWith(TagStartToken))
                    browser.Move();
                output.Append(browser.Read());
                if(browser.CursorIsIn)
                {
                    PrintTag(output, browser);
                }
            }
        }

        public void Reprint(StringBuilder output)
        {
            throw new NotImplementedException();
        }

        private void PrintTag(StringBuilder output, Browser browser)
        {
            browser.Move(TagStartToken.Length);
            browser.JumpReaderCursorToCursor();
            while(browser.CursorIsIn && !browser.StartWith(TagEndToken))
                browser.Move();
            if(!browser.CursorIsIn)
                throw new InvalidOperationException($"Miss '{TagEndToken}'");
            var tag = new Tag(browser.Read());
            browser.Move(TagEndToken.Length);
            browser.JumpReaderCursorToCursor();
            if(tag.Name == "CSV")
                PrintTagCsv(output, tag, browser);
            else
                PrintTag2(output, tag);
        }

        private void PrintTagCsv(StringBuilder output, Tag tag, Browser browser)
        {
            var tagEndCsv = Environment.NewLine + "@{EndCSV}";
            browser.Move(Environment.NewLine.Length);
            browser.JumpReaderCursorToCursor();
            while(browser.CursorIsIn && !browser.StartWith(tagEndCsv))
                browser.Move();
            var format = browser.Read();
            browser.Move(tagEndCsv.Length);
            browser.JumpReaderCursorToCursor();
            var csv = new Csv(tag.Parameters["Path"], format);
            csv.Print(output);
        }

        private void PrintTag2(StringBuilder output, Tag tag)
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
    }
}