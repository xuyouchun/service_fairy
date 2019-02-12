using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Common.Package.Drawing.Ico
{
    public class EOIcoCurLoader
    {
        public string ErrorMsg;
        private Point HotSpot;
        private long m_initialStreamPos;
        private Stream m_stream;

        public EOIcoCurLoader(Stream icoCurStream)
        {
            if (!icoCurStream.CanRead)
            {
                throw new ArgumentException("Cannot initialize EOIcoCurLoader with a stream that doesn't support reading");
            }
            this.m_stream = icoCurStream;
            this.m_initialStreamPos = this.m_stream.Position;
            this.ErrorMsg = "An unspecified error has occured";
        }

        public int CountImages()
        {
            long position = this.m_stream.Position;
            this.m_stream.Position = this.m_initialStreamPos;
            byte[] buffer = new byte[6];
            try
            {
                this.m_stream.Read(buffer, 0, 6);
            }
            catch (Exception exception)
            {
                this.ErrorMsg = "Could not get 6 bytes from the beginning of the stream. The following exception was generated:\r\n" + exception.ToString();
                return -1;
            }
            if ((((buffer[0] != 0) || (buffer[1] != 0)) || ((buffer[2] != 1) && (buffer[2] != 2))) || (buffer[3] != 0))
            {
                return -2;
            }
            int num2 = buffer[4] + (buffer[5] * 0x100);
            this.m_stream.Seek(position, SeekOrigin.Begin);
            return num2;
        }

        public unsafe Bitmap GetImage(int imageIndex)
        {
            int num;
            Bitmap srcBM = null;
            byte[] buffer2;
            this.m_stream.Position = this.m_initialStreamPos;
            IcoHeader header = new IcoHeader();
            header.ReadFromStream(this.m_stream);
            if (imageIndex >= header.Count)
            {
                this.ErrorMsg = "Invalid image index of " + Convert.ToString(imageIndex) + " was passed to GetImage";
                return null;
            }
            DirectoryEntry[] entryArray = new DirectoryEntry[header.Count];
            for (num = 0; num < header.Count; num++)
            {
                entryArray[num].ReadFromStream(this.m_stream);
            }
            this.HotSpot = new Point(entryArray[imageIndex].Planes_XHotspot, entryArray[imageIndex].BitCount_YHotspot);
            if ((this.m_initialStreamPos + entryArray[imageIndex].dwImageOffset) > this.m_stream.Length)
            {
                throw new InvalidDataException("Directory entry is invalid. Image offset is outside of the bounds of the input stream.");
            }
            this.m_stream.Position = this.m_initialStreamPos + entryArray[imageIndex].dwImageOffset;
            uint num2 = EOStreamUtility.Read_uint(this.m_stream);
            if (0x474e5089 == num2)
            {
                this.m_stream.Seek(-4, SeekOrigin.Current);
                EOOffsetStream stream = new EOOffsetStream(this.m_stream);
                try
                {
                    srcBM = new Bitmap(stream);
                    srcBM.RotateFlip(RotateFlipType.Rotate180FlipX);
                }
                catch (ArgumentException)
                {
                    return null;
                }
                return srcBM;
            }

            this.m_stream.Seek(-4, SeekOrigin.Current);
            uint num3 = 0;
            uint num4 = 0;
            uint num5 = 0;
            this.GetImageDimensions(imageIndex, ref num3, ref num4, ref num5);
            new BITMAPINFOHEADER().ReadFromStream(this.m_stream);
            //uint* palette0 = (uint*) stackalloc byte[(((IntPtr) 0x100) * 4)];
            byte* palette0 = stackalloc byte[0x100 * 4];
            uint* palette = (uint*)palette0;
            if (num5 <= 8)
            {
                int num6 = ((int)1) << (int)num5;
                for (num = 0; num < num6; num++)
                {
                    palette[num] = EOStreamUtility.Read_uint(this.m_stream);
                }
            }
            uint num7 = this.SizeComp(num3, num4, num5);
            byte[] buffer = new byte[num7];
            this.m_stream.Read(buffer, 0, (int)num7);

            if (((buffer2 = buffer) == null) || (buffer2.Length == 0))
            {
                srcBM = EvanBitmap.FromRawBitsbpp(null, palette, (int)num3, (int)num4, (int)num5, true);
            }
            else
            {
                fixed (void* voidRef = buffer2)
                {
                    srcBM = EvanBitmap.FromRawBitsbpp(voidRef, palette, (int)num3, (int)num4, (int)num5, true);
                }
            }

            buffer = null;
            if ((srcBM != null) && (num5 != 0x20))
            {
                void* mem = Marshal.AllocHGlobal((int)this.SizeComp(num3, num4, 1)).ToPointer();
                EOStreamUtility.ReadRaw(this.m_stream, mem, (int)this.SizeComp(num3, num4, 1));
                Bitmap bWMask = EvanBitmap.FromRawBitsBinary(mem, (int)num3, (int)num4, true);
                Marshal.FreeHGlobal(new IntPtr(mem));
                EvanBitmap.MaskToAlpha(srcBM, bWMask);
            }

            srcBM.RotateFlip(RotateFlipType.Rotate180FlipX);
            return srcBM;
        }

        public bool GetImageDimensions(int imageIndex, ref uint out_Width, ref uint out_Height, ref uint out_bpp)
        {
            long position = this.m_stream.Position;
            this.m_stream.Position = this.m_initialStreamPos;
            IcoHeader header = new IcoHeader();
            header.ReadFromStream(this.m_stream);
            if (imageIndex >= header.Count)
            {
                this.ErrorMsg = "Invalid image index passed to GetImageDimensions.\r\nImage index: " + Convert.ToString(imageIndex) + "\r\nAvailable image count: " + Convert.ToString(header.Count);
                return false;
            }
            DirectoryEntry[] entryArray = new DirectoryEntry[header.Count];
            for (int i = 0; i < header.Count; i++)
            {
                entryArray[i].ReadFromStream(this.m_stream);
            }
            long offset = this.m_initialStreamPos + entryArray[imageIndex].dwImageOffset;
            try
            {
                this.m_stream.Seek(offset, SeekOrigin.Begin);
            }
            catch (Exception)
            {
                this.ErrorMsg = "Could not seek to appropriate position in icon stream data.\r\nThe file data may be truncated, inaccessible or invalid.\r\nAttempted seek position: " + Convert.ToString(offset);
                this.m_stream.Seek(position, SeekOrigin.Begin);
                return false;
            }
            uint num4 = EOStreamUtility.Read_uint(this.m_stream);
            if (0x474e5089 == num4)
            {
                this.m_stream.Seek(-4, SeekOrigin.Current);
                EOOffsetStream stream = new EOOffsetStream(this.m_stream);
                try
                {
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        out_Width = (uint)bitmap.Width;
                        out_Height = (uint)bitmap.Height;
                        out_bpp = this.PixelFormatTobpp(bitmap.PixelFormat);
                    }
                }
                catch (ArgumentException)
                {
                    return false;
                }
                return true;
            }
            this.m_stream.Seek(-4, SeekOrigin.Current);
            BITMAPINFOHEADER bitmapinfoheader = new BITMAPINFOHEADER();
            bitmapinfoheader.ReadFromStream(this.m_stream);
            out_bpp = bitmapinfoheader.BitCount;
            out_Width = entryArray[imageIndex].bWidth;
            out_Height = entryArray[imageIndex].bHeight;
            uint num5 = this.SizeComp(out_Width, out_Height, out_bpp) + this.SizeComp(out_Width, out_Height, 1);
            if ((out_Width == 0) && (out_Height == 0))
            {
                out_Width = (uint)bitmapinfoheader.Width;
                out_Height = (uint)(bitmapinfoheader.Height / 2);
            }
            else if (num5 != entryArray[imageIndex].dwBytesInRes)
            {
                if (out_Width == 0xff)
                {
                    out_Width = 0x100;
                }
                if (out_Height == 0xff)
                {
                    out_Height = 0x100;
                }
            }
            if (out_Width == 0)
            {
                out_Width = 0x100;
            }
            if (out_Height == 0)
            {
                out_Height = 0x100;
            }
            this.m_stream.Seek(position, SeekOrigin.Begin);
            return true;
        }

        private PixelFormat PixelFormatFrombpp(uint bpp)
        {
            switch (bpp)
            {
                case 1:
                    return PixelFormat.Format1bppIndexed;

                case 4:
                    return PixelFormat.Format4bppIndexed;

                case 8:
                    return PixelFormat.Format8bppIndexed;

                case 15:
                    return PixelFormat.Format16bppRgb555;

                case 0x10:
                    return PixelFormat.Format16bppRgb565;

                case 0x18:
                    return PixelFormat.Format24bppRgb;

                case 0x20:
                    return PixelFormat.Format32bppArgb;
            }
            return PixelFormat.Undefined;
        }

        private uint PixelFormatTobpp(PixelFormat pf)
        {
            switch (pf)
            {
                case PixelFormat.Format16bppRgb555:
                    return 15;

                case PixelFormat.Format16bppRgb565:
                    return 0x10;

                case PixelFormat.Format24bppRgb:
                    return 0x18;

                case PixelFormat.Format1bppIndexed:
                    return 1;

                case PixelFormat.Format4bppIndexed:
                    return 4;

                case PixelFormat.Format8bppIndexed:
                    return 8;

                case PixelFormat.Format32bppArgb:
                    return 0x20;
            }
            return 0x20;
        }

        private int SizeComp(int w, int h, int bpp)
        {
            int num = (w * bpp) / 8;
            if ((num % 4) != 0)
            {
                num += 4 - (num % 4);
            }
            return (h * num);
        }

        private uint SizeComp(uint w, uint h, uint bpp)
        {
            uint num = (w * bpp) / 8;
            if ((num % 4) != 0)
            {
                num += 4 - (num % 4);
            }
            return (h * num);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public uint StructSize;
            public int Width;
            public int Height;
            public ushort Planes;
            public ushort BitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
            public unsafe BITMAPINFOHEADER(int width, int height, int bpp)
            {
                this.StructSize = (uint)sizeof(EOIcoCurLoader.BITMAPINFOHEADER);
                this.Width = width;
                this.Height = height;
                this.Planes = 1;
                this.BitCount = (ushort)bpp;
                this.biCompression = 0;
                this.biSizeImage = (uint)(((width * height) * bpp) / 8);
                this.biXPelsPerMeter = 0;
                this.biYPelsPerMeter = 0;
                this.biClrUsed = 0;
                this.biClrImportant = 0;
            }

            public unsafe static int GetStructSize()
            {
                return sizeof(EOIcoCurLoader.BITMAPINFOHEADER);
            }

            public void ReadFromStream(Stream inS)
            {
                this.StructSize = EOStreamUtility.Read_uint(inS);
                this.Width = EOStreamUtility.ReadInt(inS);
                this.Height = EOStreamUtility.ReadInt(inS);
                this.Planes = EOStreamUtility.Read_ushort(inS);
                this.BitCount = EOStreamUtility.Read_ushort(inS);
                this.biCompression = EOStreamUtility.Read_uint(inS);
                this.biSizeImage = EOStreamUtility.Read_uint(inS);
                this.biXPelsPerMeter = EOStreamUtility.ReadInt(inS);
                this.biYPelsPerMeter = EOStreamUtility.ReadInt(inS);
                this.biClrUsed = EOStreamUtility.Read_uint(inS);
                this.biClrImportant = EOStreamUtility.Read_uint(inS);
            }

            public void WriteToStream(Stream outS)
            {
                EOStreamUtility.Write_uint(outS, this.StructSize);
                EOStreamUtility.Write_uint(outS, (uint)this.Width);
                EOStreamUtility.Write_uint(outS, (uint)this.Height);
                EOStreamUtility.Write_ushort(outS, this.Planes);
                EOStreamUtility.Write_ushort(outS, this.BitCount);
                EOStreamUtility.Write_uint(outS, this.biCompression);
                EOStreamUtility.Write_uint(outS, this.biSizeImage);
                EOStreamUtility.Write_uint(outS, (uint)this.biXPelsPerMeter);
                EOStreamUtility.Write_uint(outS, (uint)this.biYPelsPerMeter);
                EOStreamUtility.Write_uint(outS, this.biClrUsed);
                EOStreamUtility.Write_uint(outS, this.biClrImportant);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DirectoryEntry
        {
            public byte bWidth;
            public byte bHeight;
            public byte bColorCount;
            public byte bReserved;
            public ushort Planes_XHotspot;
            public ushort BitCount_YHotspot;
            public uint dwBytesInRes;
            public uint dwImageOffset;
            public unsafe static int GetStructSize()
            {
                return sizeof(EOIcoCurLoader.DirectoryEntry);
            }

            public void ReadFromStream(Stream InStream)
            {
                this.bWidth = (byte)InStream.ReadByte();
                this.bHeight = (byte)InStream.ReadByte();
                this.bColorCount = (byte)InStream.ReadByte();
                this.bReserved = (byte)InStream.ReadByte();
                this.Planes_XHotspot = EOStreamUtility.Read_ushort(InStream);
                this.BitCount_YHotspot = EOStreamUtility.Read_ushort(InStream);
                this.dwBytesInRes = EOStreamUtility.Read_uint(InStream);
                this.dwImageOffset = EOStreamUtility.Read_uint(InStream);
            }

            public void WriteToStream(Stream outS)
            {
                outS.WriteByte(this.bWidth);
                outS.WriteByte(this.bHeight);
                outS.WriteByte(this.bColorCount);
                outS.WriteByte(this.bReserved);
                EOStreamUtility.Write_ushort(outS, this.Planes_XHotspot);
                EOStreamUtility.Write_ushort(outS, this.BitCount_YHotspot);
                EOStreamUtility.Write_uint(outS, this.dwBytesInRes);
                EOStreamUtility.Write_uint(outS, this.dwImageOffset);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IcoHeader
        {
            public ushort Reserved;
            public ushort Type;
            public ushort Count;
            public IcoHeader(EOIcoCurLoader.Type type)
            {
                this.Reserved = 0;
                this.Type = (ushort)type;
                this.Count = 0;
            }

            public bool IsValid()
            {
                if ((this.Reserved != 0) || ((this.Type != 1) && (this.Type != 2)))
                {
                    return false;
                }
                return true;
            }

            public unsafe static int GetStructSize()
            {
                return sizeof(EOIcoCurLoader.IcoHeader);
            }

            public void ReadFromStream(Stream InStream)
            {
                this.Reserved = EOStreamUtility.Read_ushort(InStream);
                this.Type = EOStreamUtility.Read_ushort(InStream);
                this.Count = EOStreamUtility.Read_ushort(InStream);
            }

            public void WriteToStream(Stream outS)
            {
                EOStreamUtility.Write_ushort(outS, this.Reserved);
                EOStreamUtility.Write_ushort(outS, this.Type);
                EOStreamUtility.Write_ushort(outS, this.Count);
            }
        }

        public enum Type
        {
            Type_Cursor = 2,
            Type_Icon = 1
        }
    }

}