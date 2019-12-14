using System;

namespace TextDataBuilder.Parser
{
    [System.Serializable]
    public sealed class ParsingException : Exception
    {
        public ParsingException(int line, Exception inner) :
            base(inner.Message, inner) 
        {
            Line = line;
        }

        public int Line { get; }
    }
}