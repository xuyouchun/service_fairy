using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MobileAreaTestData
{
    static class GenerateQuhaoUtility
    {
        private const string _path = @"D:\Work\Data\PhoneNumberAreaData\Quhao\中国邮编区号.txt";

        private static readonly Dictionary<string, ushort> _textDict = new Dictionary<string, ushort>();
        private static ushort _textPos = 0;
        private static readonly Encoding _encoding = Encoding.GetEncoding("gb2312");

        private static ushort _ReadPos(string text)
        {
            ushort pos;
            if (_textDict.TryGetValue(text, out pos))
                return pos;

            checked
            {
                pos = _textPos;
                _textPos += (ushort)_encoding.GetByteCount(text);
                _textDict.Add(text, pos);

                return pos;
            }
        }

        class Item
        {
            public ushort ProvincePos, CityPos, Code;
        }

        public static void Run()
        {
            foreach (Item item in _ReadItems(_path).OrderBy(v => v.Code))
            {
                _Add(item);
            }
        }

        private static void _Add(Item item)
        {
            MemoryStream ms = new MemoryStream();
            //ms.Write(
        }

        private static IEnumerable<Item> _ReadItems(string path)
        {
            string lastCode = "";
            foreach (string line in File.ReadAllLines(_path))
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                    throw new FormatException("");

                string province = parts[1], city = parts[2], code = parts[4];
                if (lastCode != code)
                {
                    yield return new Item { ProvincePos = _ReadPos(province), CityPos = _ReadPos(city), Code = ushort.Parse(code) };
                    lastCode = code;
                }
            }
        }
    }
}
