using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace Common.File.UnionFile
{
    /// <summary>
    /// UnionFile的文件流
    /// </summary>
    public class UnionFileStream : Stream
    {
        public UnionFileStream(Stream baseStream)
        {
            Contract.Requires(baseStream != null);

            _baseStream = baseStream;
        }

        private readonly Stream _baseStream;

        public override bool CanRead
        {
            get { return _baseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _baseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _baseStream.CanWrite; }
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override long Length
        {
            get { return _baseStream.Length; }
        }

        public override long Position
        {
            get
            {
                return _baseStream.Position;
            }
            set
            {
                _baseStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _baseStream.Dispose();
            }
        }

        /// <summary>
        /// 是否取消
        /// </summary>
        protected bool Canceled { get; private set; }

        /// <summary>
        /// 空的UnionFileStream
        /// </summary>
        public new static readonly UnionFileStream Null = new UnionFileStream(Stream.Null);

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Cancel()
        {
            Canceled = true;
            Dispose();
        }
    }
}
