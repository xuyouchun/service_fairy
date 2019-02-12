using System;
using System.IO;

namespace Common.Package.Drawing.Ico
{
    public sealed class EOOffsetStream : Stream
    {
        private Stream m_base;
        private long m_pos;

        public EOOffsetStream(Stream underlyingStream)
        {
            this.m_base = underlyingStream;
            this.m_pos = underlyingStream.Position;
        }

        public override void Flush()
        {
            this.m_base.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.m_base.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.m_base.Seek(offset + this.m_pos, origin);
        }

        public override void SetLength(long value)
        {
            this.m_base.SetLength(value + this.m_pos);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.m_base.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get
            {
                return this.m_base.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.m_base.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.m_base.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return (this.m_base.Length - this.m_pos);
            }
        }

        public override long Position
        {
            get
            {
                return (this.m_base.Position - this.m_pos);
            }
            set
            {
                this.m_base.Position = this.m_pos + value;
            }
        }
    }
}