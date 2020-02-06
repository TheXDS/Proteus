using ExcelDataReader;
using System;
using System.IO;

namespace Catalog2cs
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stream = File.Open(args[0], FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());
                }
            }
        }
    }
}
