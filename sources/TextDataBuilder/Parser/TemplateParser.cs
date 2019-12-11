using System;
using System.Collections.Generic;
using TextDataBuilder.Core;
using TextDataBuilder.Prototype;

namespace TextDataBuilder.Parser
{
    public class TemplateParser
    {
        private const string TagStartToken = "@{";
        private const string TagEndToken = "}";

        private readonly Dictionary<string, Func<Tag, IPrototype>> factory;
        private readonly Dictionary<string, Alias> alias = new Dictionary<string, Alias>();
        private readonly IDice dice;

        public TemplateParser(IDice dice)
        {
            this.dice = dice;
            this.factory = new Dictionary<string, Func<Tag, IPrototype>>
            {
                { "Text", CreateRawText },
                { "RandomInteger", CreateRandomInteger }
            };
        }

        public Template Parse(Browser browser)
        {
            try
            {
                var prototypes = new List<IPrototype>();
                while(browser.CursorIsIn)
                {
                    while(browser.CursorIsIn && !browser.StartWith(TagStartToken))
                        browser.Move();
                    prototypes.Add(new StaticText(browser.Read()));
                    if(browser.CursorIsIn)
                    {
                        prototypes.Add(ParseTag(browser));
                    }
                }
                return new Template(prototypes);
            }
            catch(InvalidOperationException ex)
            {
                throw new ParsingException(browser.CurrentLine(), ex);
            }
        }

        private IPrototype ParseTag(Browser browser)
        {
            var tagParser = new TagParser();
            var tag = tagParser.Parse(browser);
            browser.Move(TagEndToken.Length);
            browser.JumpReaderCursorToCursor();
            if(tag.Name == "CSV")
                return ParseTagCsv(browser, tag);
            else
                return ParseTagData(tag);
        }

        private IPrototype ParseTagCsv(Browser browser, Tag beginTag)
        {
            var tagEndCsv = Environment.NewLine + "@{/CSV}";
            browser.Move(Environment.NewLine.Length);
            browser.JumpReaderCursorToCursor();
            while(browser.CursorIsIn && !browser.StartWith(tagEndCsv))
                browser.Move();
            var format = browser.Read();
            browser.Move(tagEndCsv.Length);
            browser.JumpReaderCursorToCursor();
            return new Csv(beginTag.Parameters["Path"], format);
        }

        private IPrototype ParseTagData(Tag tag)
        {
            if(factory.ContainsKey(tag.Name))
            {
                var text = factory[tag.Name](tag);
                if(tag.Alias != string.Empty)
                {
                    var a = new Alias(text);
                    alias.Add(tag.Alias, a);
                    return a;
                }
                return text;
            }
            else if(alias.ContainsKey(tag.Name))
            {
                return alias[tag.Name];
            }
            else
            {
                return new StaticText(tag.Name);
            }
        }

        private IPrototype CreateRandomInteger(Tag tag)
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

        private IPrototype CreateRawText(Tag tag)
        {
            if(tag.Parameters.TryGetValue("Raw", out string? raw))
            {
                return new StaticText(raw);
            }
            else
            {
                throw new InvalidOperationException("Miss 'Raw' parameter.");
            }
        }
    }
}