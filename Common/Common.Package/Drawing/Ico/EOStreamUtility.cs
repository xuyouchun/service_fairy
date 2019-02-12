using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace Common.Package.Drawing.Ico
{
    public class EOStreamUtility
    {
        public static unsafe void LoadStreamDataToAllocatedBlock(Stream s, void* mem)
        {
            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, (int)s.Length);
            int length = (int)s.Length;
            for (int i = 0; i < length; i++)
            {
                *((sbyte*)((sbyte*)mem + i)) = (sbyte)buffer[i];
            }
        }

        public static uint Read_uint(Stream inS)
        {
            int num = inS.ReadByte();
            int num2 = inS.ReadByte();
            int num3 = inS.ReadByte();
            int num4 = inS.ReadByte();
            if (num == -1)
            {
                num = num2 = num3 = num4 = 0;
            }
            else if (num2 == -1)
            {
                num2 = num3 = num4 = 0;
            }
            else if (num3 == -1)
            {
                num3 = num4 = 0;
            }
            else if (num4 == -1)
            {
                num4 = 0;
            }
            return (uint)(((num | (num2 << 8)) | (num3 << 0x10)) | (num4 << 0x18));
        }

        public static ushort Read_ushort(Stream inS)
        {
            int num = inS.ReadByte();
            int num2 = inS.ReadByte();
            return (ushort)(num | (num2 << 8));
        }

        public static int ReadInt(Stream inS)
        {
            byte[] buffer = new byte[4];
            inS.Read(buffer, 0, 4);
            return (((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
        }

        public static unsafe void ReadRaw(Stream s, void* mem, int ReadSize)
        {
            byte[] buffer = new byte[ReadSize];
            s.Read(buffer, 0, ReadSize);
            Marshal.Copy(buffer, 0, new IntPtr(mem), ReadSize);
        }

        public static uint[] ReadUInts(Stream s, int Count)
        {
            byte[] buffer = new byte[Count * 4];
            uint[] numArray = new uint[Count];
            s.Read(buffer, 0, Count * 4);
            for (int i = 0; i < Count; i++)
            {
                numArray[i] = (uint)(((buffer[i * 4] | buffer[(i * 4) + 1]) | buffer[(i * 4) + 2]) | buffer[(i * 4) + 3]);
            }
            return numArray;
        }

        public static void Write_uint(Stream outS, uint dword)
        {
            byte[] buffer = new byte[] { (byte)dword, (byte)(dword >> 8), (byte)(dword >> 0x10), (byte)(dword >> 0x18) };
            outS.Write(buffer, 0, 4);
        }

        public static void Write_ushort(Stream outS, ushort word)
        {
            outS.WriteByte((byte)(word & 0xff));
            outS.WriteByte((byte)(word >> 8));
        }

        public static void WriteBGRAColor(Stream outS, Color BGRA)
        {
            outS.WriteByte(BGRA.B);
            outS.WriteByte(BGRA.G);
            outS.WriteByte(BGRA.R);
            outS.WriteByte(BGRA.A);
        }

        public static unsafe bool WriteRaw(Stream outS, void* Data, int Size)
        {
            try
            {
                byte* numPtr = (byte*)Data;
                for (int i = 0; i < Size; i++)
                {
                    outS.WriteByte(numPtr[i]);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static unsafe bool WriteRaw(Stream outS, void* Data, uint Size)
        {
            try
            {
                byte* numPtr = (byte*)Data;
                for (uint i = 0; i < Size; i += 1)
                {
                    outS.WriteByte(numPtr[(int)((byte*)i)]);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool WriteString(Stream outS, string s, bool includeNullTerminator)
        {
            try
            {
                int length = s.Length;
                char[] chArray = s.ToCharArray();
                for (int i = 0; i < length; i++)
                {
                    outS.WriteByte((byte)chArray[i]);
                }
                if (includeNullTerminator)
                {
                    outS.WriteByte(0);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

}