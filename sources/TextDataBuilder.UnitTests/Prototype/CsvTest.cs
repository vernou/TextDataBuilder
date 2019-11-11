using System;
using System.IO;
using System.Text;
using TextDataBuilder.Prototype;
using Xunit;

namespace TextDataBuilder.UnitTests.Prototype
{
    public class CsvTest
    {
        [Fact]
        public void PrintFormatedCsv()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(
                path, 
                "Value1,Value2,Value3" + Environment.NewLine + 
                "Value4,Value5,Value6"
            );

            Assert.Equal(
                "Column 1 : Value1, Column 2 : Value2, Column 3 : Value3" + Environment.NewLine +
                "Column 1 : Value4, Column 2 : Value5, Column 3 : Value6",
                new Csv(path, "Column 1 : {0}, Column 2 : {1}, Column 3 : {2}").Build()
            );
        }

        [Fact]
        public void PrintEmptyCsvFile()
        {
            Assert.Equal(
                string.Empty, 
                new Csv(
                    Path.GetTempFileName(), 
                    "Column 1 : {0}, Column 2 : {1}, Column 3 : {2}"
                ).Build()
            );
        }
    }
}