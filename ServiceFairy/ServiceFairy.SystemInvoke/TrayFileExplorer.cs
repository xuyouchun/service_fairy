using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using System.IO;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 文件浏览器
    /// </summary>
    public class TrayFileExplorer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invoker">交互终端</param>
        /// <param name="root">根路径</param>
        /// <param name="sid">安全码</param>
        public TrayFileExplorer(CoreInvoker invoker, string root = "/", Sid sid = default(Sid))
        {
            Contract.Requires(invoker != null);
            Invoker = invoker;
            Sid = sid;

            _root = string.IsNullOrEmpty(root) ? "/" : root;
            _rootDirectory = new TrayFileExplorerDirectory(this, _root);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation">导航服务地址</param>
        /// <param name="root">根路径</param>
        /// <param name="sid">安全码</param>
        public TrayFileExplorer(CommunicationOption navigation, string root = "/", Sid sid = default(Sid))
            : this(CoreInvoker.FromNavigation(navigation), root, sid)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation">导航服务地址</param>
        /// <param name="root">根路径</param>
        /// <param name="sid">安全码</param>
        public TrayFileExplorer(string navigation, string root = "/", Sid sid = default(Sid))
            : this(CoreInvoker.FromNavigation(navigation), root, sid)
        {

        }

        /// <summary>
        /// 交互终端
        /// </summary>
        internal CoreInvoker Invoker { get; private set; }

        /// <summary>
        /// 安全码
        /// </summary>
        internal Sid Sid { get; private set; }

        private readonly string _root;

        /// <summary>
        /// 根路径
        /// </summary>
        public TrayFileExplorerDirectory Root
        {
            get
            {
                return _rootDirectory;
            }
        }

        private readonly TrayFileExplorerDirectory _rootDirectory;
    }

    public abstract class TrayFileExplorerEntity
    {
        internal protected TrayFileExplorerEntity(TrayFileExplorer explorer, string fullPath)
        {
            Explorer = explorer;
            FullPath = fullPath;
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string FullPath { get; private set; }

        private PathBuilder _pb;

        private PathBuilder _GetPb()
        {
            return _pb ?? (_pb = new PathBuilder(FullPath));
        }

        /// <summary>
        /// 终端ID
        /// </summary>
        public Guid ClientId
        {
            get { return _GetPb().ClientId; }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return _GetPb().Path; }
        }

        /// <summary>
        /// 文件浏览器
        /// </summary>
        public TrayFileExplorer Explorer { get; private set; }

        #region Class PathBuilder ...

        class PathBuilder
        {
            public PathBuilder(string fullPath)
            {
                if (fullPath == null || !fullPath.StartsWith("/"))
                    goto _error;

                int k = fullPath.IndexOf('/', 1);
                if (k >= 0)
                {
                    string sCid = fullPath.Substring(1, k - 1);
                    if (!Guid.TryParse(sCid, out _clientId))
                        goto _error;

                    Path = fullPath.Substring(k + 1);
                }

                else goto _error;
                return;

            _error:
                throw new FormatException(string.Format("路径“{0}”格式错误", fullPath));
            }

            private Guid _clientId;

            /// <summary>
            /// 终端ID
            /// </summary>
            public Guid ClientId { get { return _clientId; } }

            /// <summary>
            /// 路径
            /// </summary>
            public string Path { get; private set; }
        }

        #endregion

        public override string ToString()
        {
            return Path;
        }
    }

    /// <summary>
    /// 文件浏览器的目录
    /// </summary>
    public class TrayFileExplorerDirectory : TrayFileExplorerEntity
    {
        internal TrayFileExplorerDirectory(TrayFileExplorer explorer, string fullPath)
            : base(explorer, fullPath)
        {
            _info = new Lazy<FsDirectoryInfoItem>(() => explorer.Invoker.Tray.FsGetDirectoryInfo(Path, FsGetDirectoryInfosOption.All, false, ClientId, Explorer.Sid), true);
        }

        private Lazy<FsDirectoryInfoItem> _info;

        private FsDirectoryInfoItem _GetInfo()
        {
            return _info.Value;
        }

        /// <summary>
        /// 获取所有的子目录
        /// </summary>
        /// <returns></returns>
        public TrayFileExplorerDirectory[] GetDirectories()
        {
            return null;
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <returns></returns>
        public TrayFileExplorerFile GetFiles()
        {
            return null;
        }
    }

    /// <summary>
    /// 文件浏览器的文件
    /// </summary>
    public class TrayFileExplorerFile : TrayFileExplorerEntity
    {
        internal TrayFileExplorerFile(TrayFileExplorer explorer, string fullPath)
            : base(explorer, fullPath)
        {

        }

        /// <summary>
        /// 读取文件的字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ReadAllBytes()
        {
            var invoker = Explorer.Invoker;
            return invoker.Tray.FsDownloadFile(Path, ClientId, Explorer.Sid);
        }

        /// <summary>
        /// 读取所有文本
        /// </summary>
        /// <returns></returns>
        public string ReadAllText()
        {
            byte[] bytes = ReadAllBytes();
            using (StreamReader sr = new StreamReader(new MemoryStream(bytes), true))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 读取所有文本
        /// </summary>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string ReadAllText(Encoding encoding)
        {
            byte[] bytes = ReadAllBytes();
            using (StreamReader sr = new StreamReader(new MemoryStream(bytes), encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 读取所有行
        /// </summary>
        /// <returns></returns>
        public string[] ReadAllLines()
        {
            return _ToLines(ReadAllText());
        }

        /// <summary>
        /// 读取所有行
        /// </summary>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string[] ReadAllLines(Encoding encoding)
        {
            return _ToLines(ReadAllText(encoding));
        }

        private string[] _ToLines(string s)
        {
            List<string> list = new List<string>();
            StringReader sr = new StringReader(s);
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }

            return list.ToArray();
        }
    }
}
