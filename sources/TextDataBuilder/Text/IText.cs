using System.Text;

namespace TextDataBuilder.Text
{
    public interface IText
    {
        void Print(StringBuilder output);
        void Reprint(StringBuilder output);
    }
}