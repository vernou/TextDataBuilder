using System;
using System.IO;
using System.Text;
using TextDataBuilder.Text;
using Xunit;

namespace TextDataBuilder.UnitTests.Text
{
    public class CsvTest
    {
        [Fact]
        public void PrintFormatedCsv()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, "Value1,Value2,Value3" + Environment.NewLine + "Value4,Value5,Value6");
            var csv = new Csv(path, "Column 1 : {0}, Column 2 : {1}, Column 3 : {2}");

            var output = new StringBuilder();
            csv.Print(output);

            Assert.Equal(
                "Column 1 : Value1, Column 2 : Value2, Column 3 : Value3" + Environment.NewLine +
                "Column 1 : Value4, Column 2 : Value5, Column 3 : Value6",
                output.ToString()
            );
        }

        [Fact]
        public void PrintEmptyCsvFile()
        {
            var path = Path.GetTempFileName();
            var csv = new Csv(path, "Column 1 : {0}, Column 2 : {1}, Column 3 : {2}");

            var output = new StringBuilder();
            csv.Print(output);

            Assert.Equal(string.Empty, output.ToString());
        }

        [Fact]
        public void ReprintIsNotImplemented()
        {
            var csv = new Csv(string.Empty, string.Empty);
            Assert.Throws<NotImplementedException>(() => csv.Reprint(new StringBuilder()));
        }
    }
}