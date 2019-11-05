using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TextDataBuilder.Parser
{
    public class Tag
    {
        private readonly Lazy<Fields> fields;

        public Tag(string text)
        {
            fields = new Lazy<Fields>(() => Parse(text));
        }

        public string Name => fields.Value.Name;
        public string Alias => fields.Value.Parameters.ContainsKey("As") ? fields.Value.Parameters["As"] : string.Empty;
        public IReadOnlyDictionary<string, string> Parameters => fields.Value.Parameters;

        private static Fields Parse(string text)
        {
            var browser = new Browser(text);
            return new Fields(ParseTagName(browser), ParseParameters(browser));
        }

        private static string ParseTagName(Browser browser)
        {
            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();
            while(browser.CursorIsIn && !char.IsWhiteSpace(browser.Current))
            {
                browser.Move();
            }

            var name = browser.Read();
            if(name == string.Empty)
                throw new InvalidOperationException($"The prototype name '{name}' is invalid. The name can't be empty");
            if(!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
                throw new InvalidOperationException($"The prototype name '{name}' is invalid. Valid characters are [a-zA-Z0-9].");
            return name;
        }

        private static IReadOnlyDictionary<string, string> ParseParameters(Browser browser)
        {
            var parameters = new Dictionary<string, string>();

            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();

            while(browser.CursorIsIn)
            {
                var parameter = ParseParameter(browser);
                parameters.Add(parameter.Key, parameter.Value);
                browser.SkipWhiteChar();
                if(browser.CursorIsIn && browser.Current != ',')
                    throw new InvalidOperationException("Expected ',' between parameters.");
                browser.Move();
                browser.SkipWhiteChar();
                browser.JumpReaderCursorToCursor();
            }
            return parameters;
        }

        private static KeyValuePair<string, string> ParseParameter(Browser browser)
        {
                var name = ParseParameterName(browser);
                browser.SkipWhiteChar();
                if(!browser.CursorIsIn || browser.Current != '=')
                    throw new InvalidOperationException("Expected '=' between the parameter's name and the parameter's value.");
                browser.Move();
                var value = ParseParameterValue(browser);
                return new KeyValuePair<string, string>(name, value);
        }

        private static string ParseParameterName(Browser browser)
        {
            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();
            while(browser.CursorIsIn && !char.IsWhiteSpace(browser.Current) && browser.Current != '=')
                    browser.Move();
            return browser.Read();
        }

        private static string ParseParameterValue(Browser browser)
        {
            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();
            if(!browser.CursorIsIn)
                throw new InvalidOperationException($"Miss a value in parameter.");
            if(browser.Current == '"')
            {
                browser.Move();
                browser.JumpReaderCursorToCursor();
                while(browser.CursorIsIn && browser.Current != '"')
                    browser.Move();
                var value = browser.Read();
                browser.Move();
                return value;
            }
            else
            {
                while(browser.CursorIsIn && !char.IsWhiteSpace(browser.Current) && browser.Current != ',')
                    browser.Move();
                return browser.Read();
            }
        }

        private class Fields
        {
            public Fields(string name, IReadOnlyDictionary<string, string> parameters)
            {
                Name = name;
                Parameters = parameters;
            }

            public string Name { get; }
            public IReadOnlyDictionary<string, string> Parameters { get ; }
        }
    }
}