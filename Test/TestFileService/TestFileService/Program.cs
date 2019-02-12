using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.File;
using Common.Contracts.Service;
using Common.File.UnionFile;
using System.IO;
using Common.Package.Storage;
using System.Diagnostics;

namespace TestFileService
{
    class Program
    {
        static void Main(string[] args)
        {
            string navigation = "127.0.0.1:8090";
            using (SystemInvoker invoker = new SystemInvoker(navigation))
            {
                var f = invoker.File;

                string path = "stream_table/abcdefg.st";
                FileClient fc = new FileClient(invoker);

                /*
                using (FileClientUploadStreamTableHandle h = fc.UploadStreamTable(path, "my_stream_table", "ok?"))
                {
                    h.CreateTable("my_table", new StreamTableColumn[] {
                        new StreamTableColumn("title", StreamTableColumnType.String, "my_title"),
                        new StreamTableColumn("desc", StreamTableColumnType.String, "desc"),
                    }, "OK!");

                    for (int k = 0; k < 10; k++)
                    {
                        h.AppendRow(new[] { "title_" + k, "desc_" + k });
                    }

                    h.Upload();
                }*/

                using (FileClientDownloadStreamTableHandle h = fc.DownloadStreamTable(path))
                {
                    var basicInfo = h.BasicInfo;

                    Stopwatch sw = Stopwatch.StartNew();

                    for (int k = 0; k < 10000; k++)
                    {
                        string tableName = basicInfo.TableInfos[0].Name;
                        object[][] datas = h.GetRowDatas(tableName);
                    }

                    sw.Stop();
                    Console.WriteLine(sw.ElapsedMilliseconds);
                }

                return;
            }
        }
    }
}
