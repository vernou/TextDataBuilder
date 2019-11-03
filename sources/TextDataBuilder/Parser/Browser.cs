using System;
using System.Linq;

namespace TextDataBuilder.Parser
{
    public class Browser
    {
        private readonly string text;
        private readonly string newLine;

        public Browser(string text) :
            this(text, Environment.NewLine)
        { }

        public Browser(string text, string newLine)
        {
            this.text = text;
            this.newLine = newLine;
        }

        public int Cursor { get; private set; }
        public int ReaderCursor { get; private set; }
        public char Current => text[Cursor];
        public bool CurrentIsWhite => char.IsWhiteSpace(Current);
        public bool CursorIsIn => Cursor < text.Length;

        public void Move(int moveOf = 1)
        {
            Cursor += moveOf;
        }

        public void SkipWhiteChar()
        {
            while (Cursor < text.Length && CurrentIsWhite)
                Cursor++;
        }

        public bool SkeepNewLine()
        {
            if (OccurrenceAtIndexOf(newLine, Cursor))
            {
                Move(newLine.Length);
                return true;
            }
            return false;
        }

        public string Read()
        {
            if (ReaderCursor == Cursor || ReaderCursor >= text.Length)
                return string.Empty;
            var endReaderCursor = Cursor > text.Length ? text.Length : Cursor;
            return text.Substring(ReaderCursor, endReaderCursor - ReaderCursor);
        }

        public void JumpReaderCursorToCursor()
        {
            ReaderCursor = Cursor;
        }

        public bool StartWith(string str) =>
            OccurrenceAtIndexOf(str, Cursor);

        private bool OccurrenceAtIndexOf(string occurrence, int position) =>
            text.IndexOf(occurrence, position) == position;

        public int CurrentLine()
        {
            var count = 1;
            var position = 0;
            while ((position = text.IndexOf(newLine, position)) != -1)
            {
                if (position >= Cursor)
                    break;
                count++;
                position += newLine.Length;
            }
            return count;
        }
    }
}