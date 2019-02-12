using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Collection;
using Common;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.File.UnionFile;
using System.IO;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 文件系统管理器
    /// </summary>
    [AppComponent("文件系统管理器")]
    class FileSystemManagerAppComponent : TimerAppComponentBase
    {
        public FileSystemManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(30))
        {
            _service = service;
            _fileTokenManager = new FileTokenManager();
            _fileCluster = new UnionFileCluster(Settings.DataBasePath);
        }

        private readonly Service _service;
        private readonly FileTokenManager _fileTokenManager;
        private readonly IUnionFileCluster _fileCluster;

        public IUnionFileCluster FileCluster
        {
            get { return _fileCluster; }
        }

        /// <summary>
        /// 开始上传文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string BeginUpload(string path)
        {
            Contract.Requires(path != null);

            string token = Guid.NewGuid().ToString();
            FilePathParser parser = new FilePathParser(path);
            WriteFileToken ft = new WriteFileToken() {
                Token = token, UnionFile = _fileCluster.Root.SearchFile(parser.Path),
                Service = _service, Path = path,
            };

            _fileTokenManager.Add(ft);

            return token;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="buffer"></param>
        public void Upload(string token, byte[] buffer)
        {
            Contract.Requires(token != null && buffer != null);

            WriteFileToken ft = _fileTokenManager.Get(token) as WriteFileToken;
            if (ft == null)
                throw Utility.CreateBusinessException(FileStatusCode.WriteNotSupported);

            lock(ft)
            {
                ft.Stream.Write(buffer);
            }
        }

        /// <summary>
        /// 获取事务实体
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public FileToken GetToken(string token)
        {
            Contract.Requires(token != null);

            return _fileTokenManager.Get(token);
        }

        /// <summary>
        /// 结束上传或下载文件
        /// </summary>
        /// <param name="token"></param>
        public bool End(string token)
        {
            Contract.Requires(token != null);

            FileToken ft = _fileTokenManager.Remove(token, false);
            if (ft == null)
                return false;
            
            lock (ft)
            {
                IFileTokenTag tag = ft.Tag;
                if (tag != null)
                    tag.End();

                ft.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 结束上传或下载文件
        /// </summary>
        /// <param name="tokens"></param>
        public void End(string[] tokens)
        {
            Contract.Requires(tokens != null);

            foreach (string token in tokens)
            {
                End(token);
            }
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="tokens"></param>
        public void Cancel(string[] tokens)
        {
            Contract.Requires(tokens != null);

            foreach (string token in tokens)
            {
                Cancel(token);
            }
        }

        /// <summary>
        /// 取消上传或下载
        /// </summary>
        /// <param name="token"></param>
        public bool Cancel(string token)
        {
            Contract.Requires(token != null);

            FileToken ft = _fileTokenManager.Remove(token, false);
            if (ft == null)
                return false;

            lock (ft)
            {
                IFileTokenTag tag = ft.Tag;
                if (tag != null)
                    tag.Cancel();

                ft.Stream.Cancel();
            }

            return true;
        }

        /// <summary>
        /// 一次上传全部的文件内容，适用于文件尺寸不是太大的场合
        /// </summary>
        /// <param name="path"></param>
        /// <param name="buffer"></param>
        public void UploadAll(string path, byte[] buffer)
        {
            Contract.Requires(path != null && buffer != null);

            FilePathParser parser = new FilePathParser(path);
            using (UnionFileStream stream = _fileCluster.Root.SearchFile(parser.Path).Open(UnionFileOpenModel.Write))
            {
                stream.Write(buffer);
            }
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public string BeginDownload(string path, out UnionFileInfo fileInfo)
        {
            Contract.Requires(path != null);

            string token = Guid.NewGuid().ToString();
            IUnionFile unionFile = _GetUnionFileInfo(path, out fileInfo);

            ReadFileToken ft = new ReadFileToken() { Token = token, UnionFile = unionFile, Service = _service, Path = path };
            _fileTokenManager.Add(ft);

            return token;
        }

        private IUnionFile _GetUnionFileInfo(string path, out UnionFileInfo fileInfo)
        {
            FilePathParser parser = new FilePathParser(path);
            IUnionFile unionFile = _fileCluster.Root.SearchFile(parser.Path);
            if (!unionFile.Exists() || (fileInfo = unionFile.GetFileInfo()) == null)
                throw Utility.CreateBusinessException(FileStatusCode.FileNotExists);

            return unionFile;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="token">事务标识</param>
        /// <param name="maxSize">指定最多下载的字节流量</param>
        /// <param name="atEnd">是否已经到达结尾</param>
        /// <returns></returns>
        public byte[] Download(string token, int maxSize, out bool atEnd)
        {
            Contract.Requires(token != null);

            ReadFileToken ft = _fileTokenManager.Get(token, true) as ReadFileToken;
            if (ft == null)
                throw Utility.CreateBusinessException(FileStatusCode.ReadNotSupported);

            lock (ft)
            {
                byte[] buffer = ft.Stream.ToBytes(maxSize, out atEnd);
                if (atEnd)
                    _fileTokenManager.Remove(token, false);

                return buffer;
            }
        }

        /// <summary>
        /// 下载全部的文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public byte[] DownloadAll(string path, out UnionFileInfo fileInfo)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            IUnionFile unionFile = _GetUnionFileInfo(path, out fileInfo);

            using (UnionFileStream stream = unionFile.Open())
            {
                return stream.ToBytes();
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="paths"></param>
        public void Delete(string[] paths)
        {
            Contract.Requires(paths != null);

            foreach (string path in paths)
            {
                Delete(path);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public bool Delete(string path)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            IUnionFile unionFile = _fileCluster.Root.SearchFile(parser.Path);
            return unionFile.Delete();
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _fileTokenManager.Dispose();
        }

        /// <summary>
        /// 获取指定目录的信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public UnionDirectoryInfo GetDirectoryInfo(string path)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            return _fileCluster.Root.SearchDirectory(parser.Path).GetDirectoryInfo();
        }

        /// <summary>
        /// 获取指定目录的子目录信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public UnionDirectoryInfo[] GetSubDirectoryInfos(string path, string pattern = null)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            return _fileCluster.Root.SearchDirectory(parser.Path).GetDirectories(pattern).ToArrayNotNull(p => p.GetDirectoryInfo());
        }

        /// <summary>
        /// 获取指定目录的文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public UnionFileInfo[] GetFileInfos(string path, string pattern = null)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            return _fileCluster.Root.SearchDirectory(parser.Path).GetFiles(pattern).ToArrayNotNull(p => p.GetFileInfo());
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public UnionFileInfo GetFileInfo(string path)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            return _fileCluster.Root.SearchFile(parser.Path).GetFileInfo();
        }
    }
}
