using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using Common.Utility;

namespace FreeTest
{
    unsafe class ReadIco
    {
        public static void Execute()
        {
            string icoPath = @"D:\Work\Dev\Resource\Icon\0010\public.ico";
            byte[] buffer = File.ReadAllBytes(icoPath);

            fixed (byte* pBuffer = buffer)
            {
                byte* p = pBuffer;
                IcoHeader* pHeader = (IcoHeader*)p;

                IcoType type = pHeader->Type;
                int count = pHeader->Count;


                List<IcoInfo> icoInfos = new List<IcoInfo>();
                p += sizeof(IcoHeader);
                for (int k = 0; k < count; k++)
                {
                    IcoInfo* pInfo = (IcoInfo*)p;
                    icoInfos.Add(*pInfo);

                    p += sizeof(IcoInfo);
                }

                foreach (IcoInfo info in icoInfos)
                {
                    BmpHeader* pBmpHeader = (BmpHeader*)(pBuffer + info.Offset);
                    BmpFileHeader bfh = new BmpFileHeader() {
                        bfType = 0x4D42,
                        bfSize = 14 + info.Length,
                        bfOffBits = 14 + sizeof(BmpHeader) + (pBmpHeader->bit_count == 8 ? 1024 : 0)
                    };

                    byte[] bmpBytes = new byte[14 + info.Length];
                    fixed (byte* pBmpBytes = bmpBytes)
                    {
                        BufferUtility.Copy(pBmpBytes, (byte*)&bfh, 14);
                        BufferUtility.Copy(pBmpBytes + 14, pBuffer + info.Offset, info.Length);
                    }

                    System.Drawing.Image bmp = Bitmap.FromStream(new MemoryStream(bmpBytes));
                }
            }

            return;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct IcoHeader
        {
            byte b0, b1;
            ushort ico_type;
            byte ico_count;
            byte b5;

            /// <summary>
            /// 图标类型
            /// </summary>
            public IcoType Type
            {
                get { return (IcoType)ico_type; }
            }

            /// <summary>
            /// 图标个数
            /// </summary>
            public int Count
            {
                get { return ico_count; }
            }
        }

        enum IcoType : ushort
        {
            Ico = 0x0100,

            Cursor = 0x0200,
        }


        [StructLayout(LayoutKind.Sequential)]
        struct IcoInfo
        {
            byte width, height, color_type;
            byte b3, b4, b5, b6, b7;
            int length, offset;

            /// <summary>
            /// 宽度
            /// </summary>
            public int Width { get { return width; } }

            /// <summary>
            /// 高度
            /// </summary>
            public int Height { get { return height; } }

            /// <summary>
            /// 颜色类型
            /// </summary>
            public IcoColorType ColorType { get { return (IcoColorType)color_type; } }

            /// <summary>
            /// 数据块长度
            /// </summary>
            public int Length { get { return length; } }

            /// <summary>
            /// 数据块偏移
            /// </summary>
            public int Offset { get { return offset; } }
        }

        enum IcoColorType : byte
        {
            /// <summary>
            /// 单色
            /// </summary>
            Single = 2,

            /// <summary>
            /// >=256色
            /// </summary>
            Milutile = 0,
        }

        [StructLayout(LayoutKind.Sequential, Size = 14)]
        struct BmpFileHeader
        {
            public short bfType;
            public int bfSize;
            public short r0, r1;
            public int bfOffBits;
        }

        /// <summary>
        /// BMP信息头结构长度
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Size = 40)]
        struct BmpHeader
        {
            public int header_length, width, height;
            public short bit_count, bits_of_pixel;
            public int compress_type, data_length;

            int r1, r2, r3, r4;
        }

        [StructLayout(LayoutKind.Sequential, Size = 64)]
        struct BmpColorBoard
        {
            byte xor_blue, xor_green, xor_red, r0;
        }

        [StructLayout(LayoutKind.Sequential, Size = 64)]
        struct XorBmpData
        {

        }
    }
}
