using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Common.Package.Drawing.Ico
{
    public static class EvanBitmap
    {
        public static unsafe void AddToAlpha(Bitmap bm, int A_Add)
        {
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* numPtr = (byte*)bitmapdata.Scan0.ToPointer();
            byte* numPtr2 = numPtr + ((bm.Width * bm.Height) * 4);
            while (numPtr < numPtr2)
            {
                int num = numPtr[3];
                num += A_Add;
                if (num < 0)
                {
                    num = 0;
                }
                else if (num > 0xff)
                {
                    num = 0xff;
                }
                numPtr[3] = (byte)num;
                numPtr += 4;
            }
            bm.UnlockBits(bitmapdata);
        }

        private static uint ComputeSize(uint w, uint h, uint bpp, bool PaddedTo32)
        {
            uint num = (w * bpp) / 8;
            if (PaddedTo32 && ((num % 4) != 0))
            {
                num += 4 - (num % 4);
            }
            return (h * num);
        }

        public static unsafe int CountTransparentColumnsFromLeft(Bitmap bm)
        {
            int num = 0;
            int width = bm.Width;
            int height = bm.Height;
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)((byte*)bitmapdata.Scan0.ToPointer() + ((width * height) * 4));
            num = 0;
            while (num < width)
            {
                for (uint* numPtr2 = (uint*)((byte*)bitmapdata.Scan0.ToPointer() + (num * 4)); numPtr2 < numPtr; numPtr2 += width)
                {
                    if ((numPtr2[0] & 0xff000000) != 0)
                    {
                        bm.UnlockBits(bitmapdata);
                        return num;
                    }
                }
                num++;
            }
            bm.UnlockBits(bitmapdata);
            return num;
        }

        public static unsafe int CountTransparentColumnsFromRight(Bitmap bm)
        {
            int num = 0;
            int width = bm.Width;
            int height = bm.Height;
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)((byte*)bitmapdata.Scan0.ToPointer() + ((width * height) * 4));
            for (num = width - 1; num >= 0; num--)
            {
                for (uint* numPtr2 = (uint*)((byte*)bitmapdata.Scan0.ToPointer() + (num * 4)); numPtr2 < numPtr; numPtr2 += width)
                {
                    if ((numPtr2[0] & 0xff000000) != 0)
                    {
                        bm.UnlockBits(bitmapdata);
                        return ((width - num) - 1);
                    }
                }
            }
            bm.UnlockBits(bitmapdata);
            return width;
        }

        public static unsafe int CountTransparentRowsFromBottom(Bitmap bm)
        {
            int num = 0;
            int width = bm.Width;
            int height = bm.Height;
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            for (num = height - 1; num >= 0; num--)
            {
                uint* numPtr = (uint*)((byte*)bitmapdata.Scan0.ToPointer() + ((num * width) * 4));
                uint* numPtr2 = numPtr + width;
                while (numPtr < numPtr2)
                {
                    if ((numPtr[0] & 0xff000000) != 0)
                    {
                        bm.UnlockBits(bitmapdata);
                        return ((height - num) - 1);
                    }
                    numPtr++;
                }
            }
            bm.UnlockBits(bitmapdata);
            return height;
        }

        public static unsafe void FillRawBits32(Bitmap bm, uint* bits, int w, int h)
        {
            int num = w * h;
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < num; i++)
            {
                bits[i] = numPtr[i];
            }
            bm.UnlockBits(bitmapdata);
        }

        public static PixelFormat FormatFrombpp(int bpp)
        {
            switch (bpp)
            {
                case 1:
                    return PixelFormat.Format1bppIndexed;

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

        public static unsafe Bitmap FromBitsNative(void* bits, int w, int h, int bpp)
        {
            uint num = ComputeSize((uint)w, (uint)h, (uint)bpp, true);
            PixelFormat format = FormatFrombpp(bpp);
            Bitmap bitmap = new Bitmap(w, h);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, format);
            byte* numPtr = (byte*)bitmapdata.Scan0.ToPointer();
            for (uint i = 0; i < num; i += 1)
            {
                numPtr[i] = (byte)((byte*)bits)[i];
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap FromRawBits24(void* bits, int w, int h, bool PaddedTo32Bit)
        {
            Bitmap bitmap = new Bitmap(w, h);
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int num = w * 3;
            if (PaddedTo32Bit && ((num % 4) != 0))
            {
                num += 4 - (num % 4);
            }
            byte* numPtr = (byte*)bits;
            byte* numPtr2 = (byte*)bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < h; i++)
            {
                int num3;
                uint* numPtr3 = (uint*)(numPtr2 + (i * bitmapdata.Stride));
                byte* numPtr4 = numPtr + (i * num);
                if (i == (h - 1))
                {
                    num3 = 0;
                    while (num3 < w)
                    {
                        ushort num4 = *((ushort*)(numPtr4 + (num3 * 3)));
                        byte num5 = numPtr4[(num3 * 3) + 2];
                        numPtr3[num3] = (uint)((num4 | (num5 << 0x10)) | -16777216);
                        num3++;
                    }
                }
                else
                {
                    for (num3 = 0; num3 < w; num3++)
                    {
                        numPtr3[num3] = *(((uint*)(numPtr4 + (num3 * 3)))) | 0xff000000;
                    }
                }
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap FromRawBits32(uint* bits, int w, int h)
        {
            int num = w * h;
            Bitmap bitmap = new Bitmap(w, h);
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < num; i++)
            {
                numPtr[i] = bits[i];
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap FromRawBits4(void* bits, uint* Palette, int w, int h, bool PaddedTo32Bit)
        {
            Bitmap bitmap = new Bitmap(w, h);
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int num = w / 2;
            if (PaddedTo32Bit && ((num % 4) != 0))
            {
                num += 4 - (num % 4);
            }
            byte* numPtr = (byte*)bits;
            byte* numPtr2 = (byte*)bitmapdata.Scan0.ToPointer();
            uint index = 0;
            for (int i = 0; i < h; i++)
            {
                uint* numPtr3 = (uint*)(numPtr2 + (i * bitmapdata.Stride));
                byte* numPtr4 = numPtr + (i * num);
                for (int j = 0; j < w; j++)
                {
                    bool flag = (j % 2) == 0;
                    index = (uint)(numPtr4[j / 2] & (flag ? 240 : 15));
                    if (flag)
                    {
                        index = index >> 4;
                    }
                    numPtr3[j] = Palette[index] | 0xff000000;
                }
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap FromRawBits8(void* bits, uint* Palette, int w, int h, bool PaddedTo32Bit)
        {
            Bitmap bitmap = new Bitmap(w, h);
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int num = w;
            if (PaddedTo32Bit && ((w % 4) != 0))
            {
                num += 4 - (w % 4);
            }
            byte* numPtr = (byte*)bits;
            byte* numPtr2 = (byte*)bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < h; i++)
            {
                uint* numPtr3 = (uint*)(numPtr2 + (i * bitmapdata.Stride));
                byte* numPtr4 = numPtr + (i * num);
                for (int j = 0; j < w; j++)
                {
                    numPtr3[j] = Palette[numPtr4[j]] | 0xff000000;
                }
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap FromRawBitsBinary(void* bits, int w, int h, bool PaddedTo32Bit)
        {
            Bitmap bitmap = new Bitmap(w, h);
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int num = w / 8;
            if (PaddedTo32Bit && ((num % 4) != 0))
            {
                num += 4 - (num % 4);
            }
            byte* numPtr = (byte*)bits;
            byte* numPtr2 = (byte*)bitmapdata.Scan0.ToPointer();
            uint num2 = 0;
            for (int i = 0; i < h; i++)
            {
                uint* numPtr3 = (uint*)(numPtr2 + (i * bitmapdata.Stride));
                byte* numPtr4 = numPtr + (i * num);
                for (int j = 0; j < w; j++)
                {
                    num2 = numPtr4[j / 8];
                    num2 = (num2 >> (7 - (j % 8))) & 1;
                    numPtr3[j] = (num2 == 0) ? 0xff000000 : uint.MaxValue;
                }
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap FromRawBitsbpp(void* bits, uint* Palette, int w, int h, int bpp, bool PaddedTo32Bit)
        {
            if (bpp == 0x20)
            {
                return FromRawBits32((uint*)bits, w, h);
            }
            if (bpp == 0x18)
            {
                return FromRawBits24(bits, w, h, PaddedTo32Bit);
            }
            if (bpp == 0x10)
            {
                return FromBitsNative(bits, w, h, bpp);
            }
            if (bpp == 8)
            {
                return FromRawBits8(bits, Palette, w, h, PaddedTo32Bit);
            }
            if (bpp == 4)
            {
                return FromRawBits4(bits, Palette, w, h, PaddedTo32Bit);
            }
            if (bpp == 1)
            {
                return FromRawBitsBinary(bits, w, h, PaddedTo32Bit);
            }
            return null;
        }

        public static int GetBitsPerPixel(Bitmap bm)
        {
            PixelFormat pixelFormat = bm.PixelFormat;
            if (pixelFormat <= PixelFormat.Format32bppRgb)
            {
                switch (pixelFormat)
                {
                    case PixelFormat.Format24bppRgb:
                        return 0x18;

                    case PixelFormat.Format32bppRgb:
                        goto Label_003C;
                }
                goto Label_004A;
            }
            if (pixelFormat == PixelFormat.Format8bppIndexed)
            {
                return 8;
            }
            if ((pixelFormat != PixelFormat.Format32bppPArgb) && (pixelFormat != PixelFormat.Format32bppArgb))
            {
                goto Label_004A;
            }
        Label_003C:
            return 0x20;
        Label_004A:
            return 0;
        }

        public static unsafe void MakeOpaque(Bitmap bm)
        {
            int width = bm.Width;
            int height = bm.Height;
            int num3 = width * height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < num3; i++)
            {
                uint* numPtr1 = numPtr + i;
                numPtr1[0] |= 0xff000000;
            }
            bm.UnlockBits(bitmapdata);
        }

        public static unsafe void MakeSolidColor(Bitmap bm, uint IntColor)
        {
            int width = bm.Width;
            int height = bm.Height;
            int num3 = width * height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < num3; i++)
            {
                numPtr[i] = IntColor;
            }
            bm.UnlockBits(bitmapdata);
        }

        public static unsafe bool MaskToAlpha(Bitmap srcBM, Bitmap BWMask)
        {
            if ((srcBM.Width != BWMask.Width) || (srcBM.Height != BWMask.Height))
            {
                return false;
            }
            int height = srcBM.Height;
            int width = srcBM.Width;
            Rectangle rect = new Rectangle(0, 0, width, height);
            uint* numPtr = null;
            uint* numPtr2 = null;
            BitmapData bitmapdata = null;
            BitmapData data2 = null;
            try
            {
                bitmapdata = BWMask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                numPtr = (uint*)bitmapdata.Scan0.ToPointer();
                data2 = srcBM.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                numPtr2 = (uint*)data2.Scan0.ToPointer();
            }
            catch (Exception)
            {
                return false;
            }
            int num3 = width * height;
            for (int i = 0; i < num3; i++)
            {
                if ((numPtr[i] & 0xffffff) == 0xffffff)
                {
                    uint* numPtr1 = numPtr2 + i;
                    numPtr1[0] &= 0xffffff;
                }
            }
            BWMask.UnlockBits(bitmapdata);
            srcBM.UnlockBits(data2);
            return true;
        }

        public static unsafe Bitmap ResizeCropPad(Bitmap bm, int NewWidth, int NewHeight)
        {
            if ((NewWidth == bm.Width) && (NewHeight == bm.Height))
            {
                return new Bitmap(bm);
            }
            int height = bm.Height;
            int width = bm.Width;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            uint* numPtr = (uint*)bitmapdata.Scan0.ToPointer();
            Bitmap bitmap = new Bitmap(NewWidth, NewHeight);
            Rectangle rectangle2 = new Rectangle(0, 0, NewWidth, NewHeight);
            BitmapData data2 = bitmap.LockBits(rectangle2, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            uint* numPtr2 = (uint*)data2.Scan0.ToPointer();
            int num3 = (NewWidth > bm.Width) ? bm.Width : NewWidth;
            for (int i = 0; i < NewHeight; i++)
            {
                int num5;
                uint* numPtr3 = numPtr2 + (i * NewWidth);
                uint* numPtr4 = numPtr + (i * width);
                if (i >= height)
                {
                    num5 = 0;
                    while (num5 < NewWidth)
                    {
                        numPtr3[num5] = 0;
                        num5++;
                    }
                }
                else
                {
                    for (num5 = 0; num5 < NewWidth; num5++)
                    {
                        if (num5 < num3)
                        {
                            numPtr3[num5] = numPtr4[num5];
                        }
                        else
                        {
                            numPtr3[num5] = 0;
                        }
                    }
                }
            }
            bitmap.UnlockBits(data2);
            bm.UnlockBits(bitmapdata);
            return bitmap;
        }
    }

}