using System;
using System.IO;
using TextDataBuilder.Parser;

namespace TextDataBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("TextDataBuilder <template path file>");
                return;
            }
            var path = string.Empty;
            if(args.Length > 0)
            {
                path = args[0];
            }
            try
            {
                var template = new TemplateParser(new Core.Dice()).Parse(new Browser(File.ReadAllText(path)));
                Console.WriteLine(template.Build());
            }
            catch(ParsingException ex)
            {
                Console.Error.WriteLine($"l.{ex.Line} : {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}