using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace IcoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string icoFile = @"D:\Work\Dev\Resource\Icon\0002\kpackage.ico";
            using (FileStream fs = new FileStream(icoFile, FileMode.Open))
            {
                EOIcoCurLoader loader = new EOIcoCurLoader(fs);

                int count = loader.CountImages();
                string path = @"D:\Temp\Ico\" + Path.GetFileName(icoFile);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                for (int k = 0; k < count; k++)
                {
                    Bitmap bmp = loader.GetImage(k);
                    string file = Path.Combine(path, bmp.Width + "X" + bmp.Height) + ".png";
                    bmp.Save(file, ImageFormat.Png);
                }
            }
        }
    }
}
