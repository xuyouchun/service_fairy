using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PhoneNumberParser
{
    [Serializable]
    struct PhoneArea
    {
        public ushort Area;
        public byte Province;
        public byte City;

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Area, Province, City);
        }
    }

    class ConvertToBin
    {
        static void Main(string[] args)
        {
            sbyte s = -1;
            int a = s & 0xFF;

            //_Convert(@"D:\Work\mobile\130");
            //return;

            foreach (string filePath in Directory.GetFiles(@"D:\Work\Data\PhoneNumberAreaData"))
            {
                int num;
                if (!Path.HasExtension(filePath) && int.TryParse(Path.GetFileName(filePath), out num))
                    _Convert(filePath);
            }

            return;
        }

        private static void _Convert(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            List<PhoneArea> areas = new List<PhoneArea>();
            foreach (PhoneArea[] group in _ReadAllAreaGroups(lines))
            {
                areas.Add(group[0]);
            }

            File.WriteAllText(filePath + ".txt", string.Join("\r\n", areas), Encoding.ASCII);

            // write
            using (FileStream fs = new FileStream(filePath + ".bin", FileMode.Create, FileAccess.Write))
            {
                foreach (PhoneArea area in areas)
                {
                    fs.WriteByte((byte)(area.Area));
                    fs.WriteByte((byte)(area.Area >> 8));
                    fs.WriteByte(area.Province);
                    fs.WriteByte(area.City);
                }
            }

            // read
            List<PhoneArea> areas2 = new List<PhoneArea>();
            using (FileStream fs = new FileStream(filePath + ".bin", FileMode.Open, FileAccess.Read))
            {
                const int size = 4;
                byte[] buffer = new byte[size];
                int len;
                while ((len = fs.Read(buffer, 0, size)) == size)
                {
                    areas2.Add(new PhoneArea() { Area = (ushort)((buffer[0] << 8) | buffer[1]), Province = buffer[2], City = buffer[3] });
                }
            }
        }

        private static IEnumerable<PhoneArea[]> _ReadAllAreaGroups(string[] lines)
        {
            Array.Sort(lines);

            byte lastProvince = 0xFF, lastCity = 0xFF;
            List<PhoneArea> groups = new List<PhoneArea>();
            foreach (string line in lines)
            {
                string[] parts = line.Trim().Split(',');
                if (parts.Length < 3)
                    continue;

                ushort area = ushort.Parse(parts[0]);
                PhoneArea phoneArea = new PhoneArea() { Area = area, Province = byte.Parse(parts[1]), City = byte.Parse(parts[2]) };
                if (lastProvince == 0xFF || (lastProvince == phoneArea.Province && lastCity == phoneArea.City))
                {
                    groups.Add(phoneArea);
                }
                else
                {
                    yield return groups.ToArray();
                    groups.Clear();
                    groups.Add(phoneArea);
                }

                lastProvince = phoneArea.Province;
                lastCity = phoneArea.City;
            }

        }
    }
}
