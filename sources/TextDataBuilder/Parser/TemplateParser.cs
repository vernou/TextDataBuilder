using System;
using System.Collections.Generic;
using TextDataBuilder.Core;
using TextDataBuilder.Prototype;

namespace TextDataBuilder.Parser
{
    public class TemplateParser
    {
        private readonly TagParser _tagParser = new TagParser();
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
                return ParseWithEnd(browser, string.Empty);
            }
            catch(InvalidOperationException ex)
            {
                throw new ParsingException(browser.CurrentLine(), ex);
            }
        }

        public Template ParseWithEnd(Browser browser, string contentTagName)
        {
            var prototypes = new List<IPrototype>();
            while (browser.CursorIsIn)
            {
                while (browser.CursorIsIn && !browser.StartWith(TagParser.StartToken))
                    browser.Move();
                prototypes.Add(new StaticText(browser.Read()));
                if (browser.CursorIsIn)
                {
                    var tag = _tagParser.Parse(browser);
                    if (tag.Name == "/" + contentTagName)
                    {
                        break;
                    }
                    prototypes.Add(ParseTag(tag, browser));
                }
            }
            return new Template(prototypes);
        }

        private IPrototype ParseTag(Tag tag, Browser browser)
        {
            if (tag.Name == "CSV")
                return ParseTagCsv(browser, tag);
            else
                return ParseTagData(tag);
        }

        private IPrototype ParseTagCsv(Browser browser, Tag beginTag)
        {
            browser.Move(Environment.NewLine.Length);
            browser.JumpReaderCursorToCursor();
            var content = ParseWithEnd(browser, beginTag.Name);
            browser.JumpReaderCursorToCursor();
            var join = beginTag.Parameters.ContainsKey("Join") ? beginTag.Parameters["Join"] : string.Empty;
            var format = content.Build();
            if (format.EndsWith(Environment.NewLine))
                format = format.Remove(format.Length - Environment.NewLine.Length);
            return new Csv(beginTag.Parameters["Path"], format, join);
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