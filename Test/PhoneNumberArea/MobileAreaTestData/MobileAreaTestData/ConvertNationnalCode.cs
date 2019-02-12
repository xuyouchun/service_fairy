using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MobileAreaTestData
{
    static class ConvertNationnalCode
    {
        private const string _path = @"D:\Work\Data\PhoneNumberAreaData\2011-12-16\";
        public static void Run()
        {
            Item[] items = _GetAll().OrderBy(v => v.Code).ToArray();
            _Save(items);
        }

        private static IEnumerable<Item> _GetAll()
        {
            foreach (string line in File.ReadAllLines(Path.Combine( _path, "国际区号.txt")))
            {
                string[] parts = line.Split('\t');
                if (parts.Length != 3)
                    continue;

                string name = parts[0];
                ushort code;
                if (!ushort.TryParse(parts[2], out code))
                    continue;

                yield return new Item { Name = name, Code = code };
            }
        }

        class Item
        {
            public string Name;
            public ushort Code;

            public override string ToString()
            {
                return Name + " " + Code;
            }
        }

        private static void _Save(Item[] items)
        {
            StringBuilder object_c = new StringBuilder();
            object_c.Append("static NationalCode nation_codes = {");
            for (int k = 0; k < items.Length; k++)
            {
                if (k % 4 == 0)
                    object_c.AppendLine();

                Item item = items[k];
                object_c.AppendFormat("\t{{ {0}, \"{1}\" }}", item.Code, item.Name);
                if (k != items.Length - 1)
                    object_c.Append(",");
            }

            object_c.Append("\r\n};");
            File.AppendAllText(Path.Combine(_path, "nationalcode.c"), object_c.ToString(), Encoding.UTF8);
        }
    }
}
