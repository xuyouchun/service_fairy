using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MobileAreaTestData
{
    public static class TestUtility
    {
        [DllImport("PhoneNumberArea_Dll.dll", CharSet = CharSet.Ansi, EntryPoint = "get_area_by_phone_number_2")]
        public static extern int get_area_by_phone_number_2(string phone_number, byte[] buffer);

        private static bool GetAreaByPhoneNumber(string phone_number, out string operation, out string province, out string city)
        {
            operation = province = city = string.Empty;
            byte[] buffer = new byte[1024];
            int len = get_area_by_phone_number_2(phone_number, buffer);
            if (len > 0)
            {
                string[] s = Encoding.Default.GetString(buffer, 0, len).Split('|');
                operation = s[0].Trim();
                province = s[1].Trim();
                city = s[2].Trim();
                _AssignEach(ref province, ref city);
                return true;
            }

            return false;
        }

        private const string _path = @"D:\Work\Data\PhoneNumberAreaData\2011-12-16";

        public static void Run()
        {
            string errorFile = Path.Combine(_path, "testdata_error.txt");

            foreach (string filename in new[] { "PhoneNumberArea.txt", "PhoneNumberArea_2.txt" })
            {
                foreach (string item in File.ReadAllLines(Path.Combine(_path, filename), Encoding.UTF8))
                {
                    string[] items = item.Split(' ');
                    if (items.Length != 4)
                        continue;

                    string phone_number = items[0].Trim(), operation = items[1].Trim(), province = items[2].Trim(), city = items[3].Trim();
                    _AssignEach(ref province, ref city);

                    Console.Write("{0} {1} {2} {3}: ", phone_number, operation, province, city);
                    string operation2, province2, city2;
                    bool result = GetAreaByPhoneNumber(phone_number, out operation2, out province2, out city2);

                    if (result && operation == operation2 && province == province2 && city == city2)
                    {
                        Console.Write("OK!");
                    }
                    else
                    {
                        Console.Write("{0} {1} {2} Error!", operation2, province2, city2);

                        string error = string.Format("{0}: {1} {2} {3} <-> {4} {5} {6}\r\n", phone_number, operation, province, city, operation2, province2, city2);
                        File.AppendAllText(errorFile, error, Encoding.UTF8);
                    }

                    Console.WriteLine();
                }
            }

            File.AppendAllText(errorFile, "OK!", Encoding.UTF8);
        }

        private static void _AssignEach(ref string s1, ref string s2)
        {
            if (string.IsNullOrWhiteSpace(s1))
                s1 = s2;
            else if (string.IsNullOrWhiteSpace(s2))
                s2 = s1;
        }
    }
}
