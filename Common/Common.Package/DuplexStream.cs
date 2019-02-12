using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Common.Package
{
    /// <summary>
    /// 双向流，同时支持读取和写入操作
    /// </summary>
    public class DuplexStream : Stream
    {
        public DuplexStream(bool waitForRead = false)
        {
            _waitForRead = waitForRead;
            _waitForReadHandles = new WaitHandle[] { _waitForReadEvent, _waitForExitEvent };
        }

        private static readonly ObjectPool<byte[]> _pool = new ObjectPool<byte[]>(() => new byte[128], int.MaxValue);
        private readonly Queue<Item> _queue = new Queue<Item>();
        private readonly ManualResetEvent _waitForExitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _waitForReadEvent = new ManualResetEvent(false);
        private readonly object _readLock = new object(), _writeLock = new object();
        private Item _current;
        private readonly WaitHandle[] _waitForReadHandles;
        private bool _waitForRead;

        class Item
        {
            public byte[] Buffer;
            public int WritePos;
            public int ReadPos;
        }

        private byte[] _Accquire()
        {
            return _pool.Accquire(waitForExit: _waitForExitEvent);
        }

        private void _Release(byte[] buffer)
        {
            _pool.Release(buffer);
        }

        /// <summary>
        /// 是否支持读操作
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// 是否支持定位操作
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// 是否支持写操作
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// 刷新缓冲区
        /// </summary>
        public override void Flush()
        {
            if (_current == null)
                return;

            lock (_writeLock)
            {
                lock (_queue)
                {
                    if (_current != null)
                    {
                        _queue.Enqueue(_current);
                        _current = null;
                        _waitForReadEvent.Set();
                    }
                }
            }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        private bool _WaitForRead()
        {
            return WaitHandle.WaitAny(_waitForReadHandles) == 0;
        }

        private Item _GetReadCurrent()
        {
            lock (_queue)
            {
                while (_queue.Count > 0)
                {
                    Item item = _queue.Peek();
                    if (item.ReadPos < item.WritePos)
                        return item;

                    _Release(_queue.Dequeue().Buffer);
                }

                _waitForReadEvent.Reset();
                return null;
            }
        }

        /// <summary>
        /// 写操作
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            Contract.Requires(buffer != null && offset >= 0 && offset + count < buffer.Length);

            if (count <= 0)
                return 0;

            int readCount = 0;
            Item item;

            lock (_readLock)
            {
                while (readCount < count)
                {
                    if ((item = _GetReadCurrent()) == null)
                    {
                        if (_waitForRead && readCount == 0)
                        {
                            if (!_WaitForRead())
                                return readCount;
                        }
                        else
                        {
                            return readCount;
                        }
                    }
                    else
                    {
                        int c = Math.Min(count - readCount, item.WritePos - item.ReadPos);
                        Buffer.BlockCopy(item.Buffer, item.ReadPos, buffer, offset + readCount, c);
                        item.ReadPos += c;
                        readCount += c;
                    }
                }
            }

            return readCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        private Item _GetWriteCurrent()
        {
            if (_current == null)
            {
                byte[] buffer = _Accquire();
                if (buffer == null)
                    return null;

                _current = new Item() { Buffer = buffer };
            }

            return _current;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Contract.Requires(buffer != null && offset >= 0 && count >= 0 && offset + count < buffer.Length);
            if (count == 0)
                return;

            lock (_writeLock)
            {
                Item item;
                int writeCount = 0;
                while (writeCount < count)
                {
                    if ((item = _GetWriteCurrent()) != null)
                    {
                        int c = Math.Min(item.Buffer.Length - item.WritePos, count - offset);

                        Buffer.BlockCopy(buffer, offset, item.Buffer, item.WritePos, c);
                        offset += c;
                        item.WritePos += c;
                        writeCount += c;

                        if (item.WritePos >= item.Buffer.Length)
                            Flush();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        public override void Close()
        {
            GC.SuppressFinalize(this);
            base.Close();

            _waitForExitEvent.Set();
            lock (_queue)
            {
                foreach (Item item in _queue)
                {
                    _Release(item.Buffer);
                }
            }
        }

        ~DuplexStream()
        {
            Close();
        }
    }
}
