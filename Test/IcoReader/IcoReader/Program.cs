using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Common.Utility;

namespace IcoReader
{
    unsafe class Program
    {
        static void Main(string[] args)
        {
            string icoFile = @"D:\Users\xuyc\Pictures\图标\OK\16smo9\Blue_Chip.ico";

            FileStream fs = new FileStream(icoFile, FileMode.Open);
            ICONDIR iconDir;
            fs.ReadToBuffer((byte*)&iconDir, sizeof(ICONDIR));

            ICONDIRENTRY[] iconDirEntity = new ICONDIRENTRY[iconDir.idCount];
            fixed (ICONDIRENTRY* pIconDirEntity = iconDirEntity)
            {
                for (int k = 0; k < iconDir.idCount; k++)
                {
                    byte* p = (byte*)(pIconDirEntity + k);
                    fs.ReadToBuffer((byte*)pIconDirEntity, sizeof(ICONDIRENTRY));
                    _ReadImage(fs, *(ICONDIRENTRY*)p);
                    p += sizeof(ICONDIRENTRY);
                }
            }

            return;
        }

        private static Image _ReadImage(Stream stream, ICONDIRENTRY iconDirEntity)
        {
            byte[] bytes = new byte[iconDirEntity.dwBytesInRes];
            stream.Seek(iconDirEntity.dwImageOffset, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            IntPtr xorBits, andBits;
            _TwoBitsFromDIB(bytes, out xorBits, out andBits, iconDirEntity);

            return null;
        }

        private static void _TwoBitsFromDIB(byte[] bytes, out IntPtr xorBits, out IntPtr andBits, ICONDIRENTRY iconDirEntity)
        {
            int height = iconDirEntity.bHeight >> 1;
            int sizeImage = _BytesPerScanline(iconDirEntity.bWidth, iconDirEntity.wBitCount, 32) * iconDirEntity.bHeight;
        }

        private static long _BytesPerScanline(byte width, ushort bitCount, int align)
        {
            throw new NotImplementedException();
        }

        struct ICONDIR
        {
            public ushort idReserved;
            public ushort idType;
            public ushort idCount;
        }

        struct ICONDIRENTRY
        {
            public byte bWidth;
            public byte bHeight;
            public byte bColorCount;
            public byte bReserved;
            public ushort wPlanes;
            public ushort wBitCount;
            public uint dwBytesInRes;
            public uint dwImageOffset;
        }
    }
}
