using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.File.UnionFile;
using Common.Framework.TrayPlatform;

namespace ServiceFairy.Service.File.Components
{
    abstract class FileToken : IDisposable
    {
        public FileToken()
        {
            CreationTime = LastAccessTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public Service Service { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 事务标识
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public IUnionFile UnionFile { get; set; }

        public IFileTokenTag Tag { get; set; }

        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <returns></returns>
        protected abstract UnionFileStream OnCreateStream();

        private volatile UnionFileStream _stream;

        /// <summary>
        /// 文件流
        /// </summary>
        public UnionFileStream Stream
        {
            get
            {
                if (_stream != null)
                    return _stream;

                lock (this)
                {
                    return _stream ?? (_stream = OnCreateStream());
                }
            }
        }

        /// <summary>
        /// Stream是否已经创建
        /// </summary>
        /// <returns></returns>
        public bool IsStreamCreated()
        {
            return _stream != null;
        }

        public void Dispose()
        {
            IFileTokenTag tag = Tag;
            if (tag != null)
                tag.Dispose();

            if (_stream != null)
                _stream.Dispose();
        }
    }

    public interface IFileTokenTag : IDisposable
    {
        /// <summary>
        /// 结束
        /// </summary>
        void End();

        /// <summary>
        /// 取消
        /// </summary>
        void Cancel();
    }
}
