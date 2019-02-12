using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.File.UnionFile;
using System.IO;

namespace CommonTest
{
    static class UnionFileTest
    {
        public static void Test()
        {
            UnionFileCluster cluster = new UnionFileCluster(@"d:\temp\UnionFile");

            // 在根目录中创建N个文件
            /*for (int k = 0; k < 3; k++)
            {
                string filename = "first_file_" + k.ToString().PadLeft(4, '0') + ".txt";
                //_WriteFile(cluster.Root, filename);
                IUnionFile file = cluster.Root.GetFile(filename);
                file.Delete();
            }

            // 在根目录中创建目录
            IUnionFileDirectory dir = cluster.Root.SearchDirectory("first_directory/subdir");

            // 在新目录中创建文件
            _WriteFile(dir, "first_file.txt");*/

            int count = cluster.Root.GetDirectoryCount("*dir*");
            Console.WriteLine(count);

            IUnionDirectory[] dirs = cluster.Root.GetDirectories("*dir*");
            Console.WriteLine(dirs.Length);
        }

        private static void _WriteFile(IUnionDirectory dir, string filename)
        {
            IUnionFile unionFile = dir.GetFile(filename);
            using (UnionFileStream stream = unionFile.Open(UnionFileOpenModel.Write))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine("我爱北京天安门");
                writer.WriteLine("天安门上太阳升");
                writer.WriteLine("伟大领袖毛主席");
                writer.WriteLine("指导我们向前进");
            }
        }
    }
}
