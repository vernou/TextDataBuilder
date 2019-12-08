using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextDataBuilder.Parser
{
    public class TagParser
    {
        internal const string TagStartToken = "@{";
        internal const string TagEndToken = "}";

        public Tag Parse(Browser browser)
        {
            browser.Move(TagStartToken.Length);
            browser.JumpReaderCursorToCursor();
            var name = ParseTagName(browser);
            var parameters = ParseParameters(browser);
            return new Tag(name, parameters);
        }

        private static string ParseTagName(Browser browser)
        {
            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();
            while (browser.CursorIsIn && !browser.StartWith(TagEndToken) && !char.IsWhiteSpace(browser.Current))
            {
                browser.Move();
            }

            var name = browser.Read();
            if (name == string.Empty)
                throw new InvalidOperationException($"The prototype name '{name}' is invalid. The name can't be empty");
            if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
                throw new InvalidOperationException($"The prototype name '{name}' is invalid. Valid characters are [a-zA-Z0-9].");
            return name;
        }

        private static IReadOnlyDictionary<string, string> ParseParameters(Browser browser)
        {
            var parameters = new Dictionary<string, string>();

            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();

            while (browser.CursorIsIn && !browser.StartWith(TagEndToken))
            {
                var parameter = ParseParameter(browser);
                parameters.Add(parameter.Key, parameter.Value);
                browser.SkipWhiteChar();
                if (browser.StartWith(TagEndToken))
                    return parameters;
                if (!browser.CursorIsIn)
                    throw new InvalidOperationException("Miss '}' to close the tag.");
                browser.JumpReaderCursorToCursor();
            }
            return parameters;
        }

        private static KeyValuePair<string, string> ParseParameter(Browser browser)
        {
            var name = ParseParameterName(browser);
            browser.SkipWhiteChar();
            if (!browser.CursorIsIn || browser.Current != '=')
                throw new InvalidOperationException("Expected '=' between the parameter's name and the parameter's value.");
            browser.Move();
            var value = ParseParameterValue(browser);
            return new KeyValuePair<string, string>(name, value);
        }

        private static string ParseParameterName(Browser browser)
        {
            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();
            while (browser.CursorIsIn && !char.IsWhiteSpace(browser.Current) && browser.Current != '=')
                browser.Move();
            return browser.Read();
        }

        private static string ParseParameterValue(Browser browser)
        {
            browser.SkipWhiteChar();
            browser.JumpReaderCursorToCursor();
            if (!browser.CursorIsIn)
                throw new InvalidOperationException($"Miss a value in parameter.");
            if (browser.Current == '"')
            {
                browser.Move();
                browser.JumpReaderCursorToCursor();
                while (browser.CursorIsIn && (browser.Current != '"' || browser.StartWith("\"\"")))
                {
                    if (browser.StartWith("\"\""))
                        browser.Move(2);
                    browser.Move();
                }
                var value = browser.Read();
                browser.Move();
                return value.Replace("\"\"", "\"");
            }
            else
            {
                while (browser.CursorIsIn && !browser.StartWith(TagEndToken) && !char.IsWhiteSpace(browser.Current))
                    browser.Move();
                return browser.Read();
            }
        }
    }
}