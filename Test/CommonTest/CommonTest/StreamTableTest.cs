using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using Common.Algorithms;
using Common.Contracts.Entities;
using Common.Package.Storage;

namespace CommonTest
{
    class StreamTableTest
    {
        public static void Test()
        {
            using (FileStream fs = new FileStream(@"d:\aaa.st", FileMode.Create))
            {
                _WriteTable(fs);
            }

            using (FileStream fs = new FileStream(@"d:\aaa.st", FileMode.Open))
            {
                _ReadTable(fs);
            }
        }

        private static void _ReadTable(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                _ReadTable(fs);
            }
        }

        private static void _ReadTable(Stream stream)
        {
            StreamTableReader reader = new StreamTableReader(stream);
            int tableCount = reader.TableCount;

            Console.WriteLine(reader.HeaderInfo);
            foreach (StreamTable st in reader.GetAllTables())
            {
                foreach (StreamTableRow row in st)
                {
                    Console.WriteLine(row);
                }
            }
        }

        private static void _WriteTable(Stream stream)
        {
            StreamTableWriter writer = new StreamTableWriter();
            WritableStreamTable st = writer.CreateTable("mytable", new StreamTableColumn[] {
                new StreamTableColumn("id", StreamTableColumnType.String, "my id"),
                new StreamTableColumn("var_array", StreamTableColumnType.Variant,  StreamTableColumnStorageModel.DynamicArray),
                new StreamTableColumn("info_array", StreamTableColumnType.String, StreamTableColumnStorageModel.DynamicArray),
            }, "hello mytable");

            for (int k = 0; k < 100; k++)
            {
                StreamTableRow row = st.AppendRow();
                row["id"] = k;
                row["var_array"] = new object[] { 1, "aaa", DateTime.Now, (decimal)100, Guid.NewGuid() };
                row["info_array"] = new string[] { "1", "2", "3", "4", "5" };
            }

            WritableStreamTable st2 = writer.CreateTable("mytable_2", new StreamTableColumn[] {
                new StreamTableColumn("id", StreamTableColumnType.String, "my id"),
                new StreamTableColumn("name", StreamTableColumnType.Int),
            });

            for (int k = 0; k < 20; k++)
            {
                StreamTableRow row = st2.AppendRow();
                row["id"] = k;
                row["name"] = k.ToString();
            }

            writer.WriteToStream(stream);
        }
    }
}
