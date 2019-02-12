using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using EOFC;

internal class EOIcoCurWriter
{
    public string ErrorMsg;
    private DirectoryEntry[] m_Entries;
    private int m_NumWritten;
    private Stream m_outS;
    private long m_StreamStart;
    private IcoCurType m_type;

    public EOIcoCurWriter(Stream outputStream, int imageCount, IcoCurType type)
    {
        if (!outputStream.CanSeek)
        {
            throw new ArgumentException("Icon/cursor output stream does not support seeking. A stream that supports seeking is required to write icon and cursor data.");
        }
        this.m_StreamStart = outputStream.Position;
        this.m_outS = outputStream;
        this.m_type = type;
        new IcoHeader(type) { Count = (ushort) imageCount }.WriteToStream(this.m_outS);
        this.m_NumWritten = 0;
        this.m_Entries = new DirectoryEntry[imageCount];
    }

    private unsafe void MakeMask(Bitmap AlphaImg, ref byte[] maskdata, int MaskRowSize)
    {
        int num = MaskRowSize * 8;
        int width = AlphaImg.Width;
        int height = AlphaImg.Height;
        Rectangle rect = new Rectangle(0, 0, width, height);
        BitmapData bitmapdata = AlphaImg.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        for (int i = 0; i < height; i++)
        {
            byte* numPtr = (byte*) (((byte*)bitmapdata.Scan0.ToPointer() + (bitmapdata.Stride * i)) + 3);
            int bitIndex = num * ((height - 1) - i);
            for (int j = 0; j < width; j++)
            {
                if (numPtr[0] > 0x7f)
                {
                    BooleanBitArray.SetMSbFirst(maskdata, bitIndex, false);
                }
                else
                {
                    BooleanBitArray.SetMSbFirst(maskdata, bitIndex, true);
                }
                bitIndex++;
                numPtr += 4;
            }
        }
        AlphaImg.UnlockBits(bitmapdata);
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

    public unsafe bool WriteBitmap(Bitmap img, Bitmap AlphaImgMask, Point hotSpot)
    {
        int w = 0;
        int h = 0;
        int bpp = 0;
        int size = 0;
        long offset = 0;
        long num6 = IcoHeader.GetStructSize() + (DirectoryEntry.GetStructSize() * this.m_Entries.Length);
        if (((img == null) || (img.Width <= 0)) || (img.Height <= 0))
        {
            this.ErrorMsg = "Invalid image passed to \"WriteBitmap\".";
            return false;
        }
        MemoryStream stream = null;
        if ((img.Width >= 0x100) || (img.Height >= 0x100))
        {
            stream = new MemoryStream();
            img.Save(stream, ImageFormat.Png);
        }
        w = img.Width;
        h = img.Height;
        bpp = EvanBitmap.GetBitsPerPixel(img);
        size = this.SizeComp(w, h, bpp);
        int maskRowSize = w / 8;
        int count = (w * h) / 8;
        if ((maskRowSize % 4) != 0)
        {
            int num9 = 4 - (maskRowSize % 4);
            maskRowSize += num9;
            count += num9 * h;
        }
        if (this.m_NumWritten != 0)
        {
            num6 = this.m_Entries[this.m_NumWritten - 1].dwImageOffset + this.m_Entries[this.m_NumWritten - 1].dwBytesInRes;
        }
        this.m_Entries[this.m_NumWritten].bWidth = (w >= 0x100) ? ((byte) 0) : ((byte) w);
        this.m_Entries[this.m_NumWritten].bHeight = (h >= 0x100) ? ((byte) 0) : ((byte) h);
        this.m_Entries[this.m_NumWritten].bColorCount = 0;
        this.m_Entries[this.m_NumWritten].bReserved = 0;
        this.m_Entries[this.m_NumWritten].Planes_XHotspot = 1;
        this.m_Entries[this.m_NumWritten].BitCount_YHotspot = (ushort) bpp;
        if (IcoCurType.Cursor == this.m_type)
        {
            this.m_Entries[this.m_NumWritten].Planes_XHotspot = (ushort) hotSpot.X;
            this.m_Entries[this.m_NumWritten].BitCount_YHotspot = (ushort) hotSpot.Y;
        }
        this.m_Entries[this.m_NumWritten].dwBytesInRes = (uint) (((BITMAPINFOHEADER.GetStructSize() + ((bpp == 8) ? 0x400 : 0)) + size) + count);
        if (null != stream)
        {
            this.m_Entries[this.m_NumWritten].dwBytesInRes = (uint) stream.Length;
        }
        this.m_Entries[this.m_NumWritten].dwImageOffset = (uint) num6;
        offset = (this.m_StreamStart + IcoHeader.GetStructSize()) + (DirectoryEntry.GetStructSize() * this.m_NumWritten);
        this.m_outS.Seek(offset, SeekOrigin.Begin);
        this.m_Entries[this.m_NumWritten].WriteToStream(this.m_outS);
        this.m_outS.Seek(this.m_StreamStart + num6, SeekOrigin.Begin);
        if (null != stream)
        {
            this.m_outS.Write(stream.ToArray(), 0, (int) stream.Length);
        }
        else
        {
            new BITMAPINFOHEADER(w, h * 2, bpp).WriteToStream(this.m_outS);
            if (8 == bpp)
            {
                ColorPalette palette = img.Palette;
                int length = palette.Entries.Length;
                for (int i = 0; i < length; i++)
                {
                    EOStreamUtility.WriteBGRAColor(this.m_outS, palette.Entries[i]);
                }
            }
            img.RotateFlip(RotateFlipType.Rotate180FlipX);
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = img.LockBits(rect, ImageLockMode.ReadOnly, img.PixelFormat);
            EOStreamUtility.WriteRaw(this.m_outS, bitmapdata.Scan0.ToPointer(), size);
            img.UnlockBits(bitmapdata);
            img.RotateFlip(RotateFlipType.Rotate180FlipX);
            Bitmap alphaImg = (AlphaImgMask == null) ? img : AlphaImgMask;
            byte[] maskdata = new byte[count];
            this.MakeMask(alphaImg, ref maskdata, maskRowSize);
            this.m_outS.Write(maskdata, 0, count);
        }
        this.m_NumWritten++;
        return true;
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
            this.StructSize = (uint) sizeof(EOIcoCurWriter.BITMAPINFOHEADER);
            this.Width = width;
            this.Height = height;
            this.Planes = 1;
            this.BitCount = (ushort) bpp;
            this.biCompression = 0;
            this.biSizeImage = (uint) (((width * height) * bpp) / 8);
            this.biXPelsPerMeter = 0;
            this.biYPelsPerMeter = 0;
            this.biClrUsed = 0;
            this.biClrImportant = 0;
        }

        public unsafe static int GetStructSize()
        {
            return sizeof(EOIcoCurWriter.BITMAPINFOHEADER);
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

        public void WriteToStream(Stream m_outS)
        {
            EOStreamUtility.Write_uint(m_outS, this.StructSize);
            EOStreamUtility.Write_uint(m_outS, (uint) this.Width);
            EOStreamUtility.Write_uint(m_outS, (uint) this.Height);
            EOStreamUtility.Write_ushort(m_outS, this.Planes);
            EOStreamUtility.Write_ushort(m_outS, this.BitCount);
            EOStreamUtility.Write_uint(m_outS, this.biCompression);
            EOStreamUtility.Write_uint(m_outS, this.biSizeImage);
            EOStreamUtility.Write_uint(m_outS, (uint) this.biXPelsPerMeter);
            EOStreamUtility.Write_uint(m_outS, (uint) this.biYPelsPerMeter);
            EOStreamUtility.Write_uint(m_outS, this.biClrUsed);
            EOStreamUtility.Write_uint(m_outS, this.biClrImportant);
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
            return sizeof(EOIcoCurWriter.DirectoryEntry);
        }

        public void ReadFromStream(Stream InStream)
        {
            this.bWidth = (byte) InStream.ReadByte();
            this.bHeight = (byte) InStream.ReadByte();
            this.bColorCount = (byte) InStream.ReadByte();
            this.bReserved = (byte) InStream.ReadByte();
            this.Planes_XHotspot = EOStreamUtility.Read_ushort(InStream);
            this.BitCount_YHotspot = EOStreamUtility.Read_ushort(InStream);
            this.dwBytesInRes = EOStreamUtility.Read_uint(InStream);
            this.dwImageOffset = EOStreamUtility.Read_uint(InStream);
        }

        public void WriteToStream(Stream m_outS)
        {
            m_outS.WriteByte(this.bWidth);
            m_outS.WriteByte(this.bHeight);
            m_outS.WriteByte(this.bColorCount);
            m_outS.WriteByte(this.bReserved);
            EOStreamUtility.Write_ushort(m_outS, this.Planes_XHotspot);
            EOStreamUtility.Write_ushort(m_outS, this.BitCount_YHotspot);
            EOStreamUtility.Write_uint(m_outS, this.dwBytesInRes);
            EOStreamUtility.Write_uint(m_outS, this.dwImageOffset);
        }
    }

    public enum IcoCurType
    {
        Cursor = 2,
        Icon = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IcoHeader
    {
        public ushort Reserved;
        public ushort Type;
        public ushort Count;
        public IcoHeader(EOIcoCurWriter.IcoCurType type)
        {
            this.Reserved = 0;
            this.Type = (ushort) type;
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
            return sizeof(EOIcoCurWriter.IcoHeader);
        }

        public void ReadFromStream(Stream InStream)
        {
            this.Reserved = EOStreamUtility.Read_ushort(InStream);
            this.Type = EOStreamUtility.Read_ushort(InStream);
            this.Count = EOStreamUtility.Read_ushort(InStream);
        }

        public void WriteToStream(Stream m_outS)
        {
            EOStreamUtility.Write_ushort(m_outS, this.Reserved);
            EOStreamUtility.Write_ushort(m_outS, this.Type);
            EOStreamUtility.Write_ushort(m_outS, this.Count);
        }
    }
}

