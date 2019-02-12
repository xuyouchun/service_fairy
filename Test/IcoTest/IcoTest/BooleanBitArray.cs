namespace EOFC
{
    using System;
    using System.Reflection;

    internal class BooleanBitArray
    {
        private int m_bitSize;
        private byte[] m_data;

        public BooleanBitArray(int sizeInBits)
        {
            this.m_bitSize = sizeInBits;
            int num = (this.m_bitSize / 8) + 1;
            if (0 == (this.m_bitSize % 8))
            {
                num = this.m_bitSize / 8;
            }
            this.m_data = new byte[num];
            if (null == this.m_data)
            {
                throw new OutOfMemoryException("Could not allocate " + Convert.ToString(num) + " bytes of memory for boolean bit array");
            }
            Array.Clear(this.m_data, 0, this.m_data.Length);
        }

        public bool Get(int bitIndex)
        {
            byte num = (byte) (((int) 1) << (bitIndex % 8));
            return ((this.m_data[bitIndex / 8] & num) != 0);
        }

        public static bool Get(byte[] data, int bitIndex)
        {
            byte num = (byte) (((int) 1) << (bitIndex % 8));
            return ((data[bitIndex / 8] & num) != 0);
        }

        public static unsafe bool Get(byte* data, int BitIndex)
        {
            byte num = (byte) (((int) 1) << (BitIndex % 8));
            if ((data[BitIndex / 8] & num) == 0)
            {
                return false;
            }
            return true;
        }

        public static void Set(byte[] data, int BitIndex, bool value)
        {
            byte num;
            if (value)
            {
                num = (byte) (((int) 1) << (BitIndex % 8));
                data[BitIndex / 8] = (byte) (data[BitIndex / 8] | num);
            }
            else
            {
                num = (byte) ~(((int) 1) << (BitIndex % 8));
                data[BitIndex / 8] = (byte) (data[BitIndex / 8] & num);
            }
        }

        public static unsafe void Set(byte* data, int BitIndex, bool value)
        {
            byte num;
            if (value)
            {
                num = (byte) (((int) 1) << (BitIndex % 8));
                byte* numPtr1 = data + (BitIndex / 8);
                numPtr1[0] = (byte) (numPtr1[0] | num);
            }
            else
            {
                num = (byte) ~(((int) 1) << (BitIndex % 8));
                byte* numPtr2 = data + (BitIndex / 8);
                numPtr2[0] = (byte) (numPtr2[0] & num);
            }
        }

        public static void SetMSbFirst(byte[] data, int BitIndex, bool value)
        {
            byte num;
            if (value)
            {
                num = (byte) (((int) 1) << (7 - (BitIndex % 8)));
                data[BitIndex / 8] = (byte) (data[BitIndex / 8] | num);
            }
            else
            {
                num = (byte) ~(((int) 1) << (7 - (BitIndex % 8)));
                data[BitIndex / 8] = (byte) (data[BitIndex / 8] & num);
            }
        }

        public bool this[int index]
        {
            get
            {
                return this.Get(index);
            }
            set
            {
                Set(this.m_data, index, value);
            }
        }
    }
}

