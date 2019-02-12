using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace TestBinaryFormatter
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch sw = Stopwatch.StartNew();
            int length = 0;
            int count = 100 * 10000;
            for (int k = 0; k < count; k++)
            {
                Item item = new Item() {
                    ID = k,
                    Name = "Name_" + k,
                    Title = "Title_" + k,
                    Address = "Address_" + k,
                    PhoneNumber = "PhoneNumber_" + k,
                    Sex = true,
                    Married = true,
                    Age = k
                };

                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, item);

                byte[] buffer = ms.ToArray();
                length = buffer.Length;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + ", " + (double)sw.ElapsedMilliseconds / count + ", " + count / sw.Elapsed.TotalSeconds);
            Console.WriteLine(length);
        }
    }


    [Serializable]
    public class Item
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public bool Sex { get; set; }

        public bool Married { get; set; }

        public int Age { get; set; }
    }

}
