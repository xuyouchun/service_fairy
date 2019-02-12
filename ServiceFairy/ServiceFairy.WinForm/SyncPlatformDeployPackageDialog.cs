using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using System.IO;
using Common.Contracts.UIObject;
using Common.Utility;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Package.UIObject;
using ServiceFairy.Entities;

namespace ServiceFairy.WinForm
{
    public partial class SyncPlatformDeployPackageDialog : XDialog
    {
        public SyncPlatformDeployPackageDialog()
        {
            InitializeComponent();
        }

        private void SyncPlatformDeployPackageDialog_Load(object sender, EventArgs e)
        {
            _formLoaded = true;

            _LoadContents();
        }

        private bool _formLoaded;

        /// <summary>
        /// 程序集的路径
        /// </summary>
        public string AssemblyPath
        {
            get { return _txtDir.Text; }
            set { _txtDir.Text = value; }
        }

        /// <summary>
        /// 忽略的文件
        /// </summary>
        public string[] IgnoreFiles { get; set; }

        private void _txtDir_TextChanged(object sender, EventArgs e)
        {
            if (_formLoaded)
                _LoadContents();
        }

        private void _LoadContents()
        {
            string path = AssemblyPath;
            if (!Directory.Exists(path))
            {
                _ctlList.Items.Clear();
            }
            else
            {
                try
                {
                    _ctlList.BeginUpdate();
                    _ctlList.Items.Clear();

                    foreach (ItemBase item in _LoadContents(path).WhereNotNull())
                    {
                        _ctlList.Items.Add(item);
                    }

                    _ctlSelectAll.UpdateState();
                }
                finally
                {
                    _ctlList.EndUpdate();
                }
            }
        }

        private static readonly Dictionary<string, ResourceUIObjectImageLoader> _imageLoaders
            = new Dictionary<string, ResourceUIObjectImageLoader>();

        private static ResourceUIObjectImageLoader _GetImageLoader(string name)
        {
            return _imageLoaders.GetOrSet(name,
                (key) => ResourceUIObjectImageLoader.Load(typeof(SyncPlatformDeployPackageDialog), name));
        }

        #region Class ItemBase ...

        abstract class ItemBase
        {
            public ItemBase(string name, string desc, string imgResName, int fileCount)
            {
                Name = name;
                Desc = desc;
                ImageResName = imgResName;
                FileCount = fileCount;
            }

            public string Name { get; private set; }

            public string Desc { get; private set; }

            public string ImageResName { get; private set; }

            public int FileCount { get; private set; }

            public abstract IEnumerable<CompressFileItemBase> GetCompressFileItems(string basePath);

            public override string ToString()
            {
                string s;
                if (string.IsNullOrWhiteSpace(Desc))
                    s = Name;
                else
                    s = string.Format("{0} ({1})", Name, Desc);

                s += string.Format("  {0}个文件", FileCount);
                return s;
            }
        }

        #endregion

        #region Class Item ...

        class Item : ItemBase
        {
            public Item(string name, string desc, string[] fileNames, string imgResName)
                : base(name, desc, imgResName, fileNames.Length)
            {
                FileNames = fileNames;
            }

            public string[] FileNames { get; private set; }

            public override IEnumerable<CompressFileItemBase> GetCompressFileItems(string basePath)
            {
                foreach (string file in FileNames)
                {
                    yield return new CompressFileItem(file, file.Substring(basePath.Length).TrimStart('\\'));
                }
            }
        }

        #endregion

        #region Class ServiceItem ...

        class ServiceItem : Item
        {
            public ServiceItem(string name, string desc, string[] fileNames, string imgResName, ServiceDesc serviceDesc)
                : base(name, desc, fileNames, imgResName)
            {
                ServiceDesc = serviceDesc;
            }

            public ServiceDesc ServiceDesc { get; private set; }

            public override IEnumerable<CompressFileItemBase> GetCompressFileItems(string basePath)
            {
                return base.GetCompressFileItems(basePath).Union(new[] { _GetSettingsFile(basePath) });
            }

            private CompressFileItemBase _GetSettingsFile(string basePath)
            {
                long version = DateTime.UtcNow.Ticks;
                ServiceDeployPackageSettings settings = new ServiceDeployPackageSettings() { LastUpdate = DateTime.UtcNow };
                return new BufferCompressFileItem(ServiceDeployPackageSettings.DefaultFileName, settings.ToXml());
            }
        }

        #endregion

        private IEnumerable<ItemBase> _LoadContents(string path)
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            string[] filter = IgnoreFiles;
            if (!filter.IsNullOrEmpty())
            {
                files = files.WhereToArray(f => !filter.Any(flt => string.Equals(flt.TrimStart('\\'),
                    f.Substring(path.Length).TrimStart('\\'), StringComparison.OrdinalIgnoreCase)));
            }

            // 平台
            yield return new Item("平台", "",
                files.WhereToArray(f => !_StartWith(f, path, new[] { "Resource", "Service" })), "Platform");

            // 资源
            yield return new Item("资源", "",
                files.WhereToArray(f => _StartWith(f, path, new[] { "Resource" })), "Resource");

            // 服务
            var serviceList = from file in files.Where(f => _StartWith(f, path, new[] { "Service" }))
                              let sd = _GetServiceDesc(path, file)
                              where sd != null
                              group file by sd into g
                              select new ServiceItem(g.Key.ToString(), "", g.ToArray(), "Service", g.Key);

            foreach (ItemBase item in serviceList)
            {
                yield return item;
            }
        }

        private ServiceDesc _GetServiceDesc(string path, string file)
        {
            string s = file.Substring(Path.Combine(path, "Service").Length).TrimStart('\\');
            int k = s.IndexOf('\\');
            if (k < 0)
                return null;

            int k2 = s.IndexOf('\\', k + 1);
            SVersion ver;
            if (k2 > 0 && SVersion.TryParse(s.Substring(k + 1, k2 - k - 1), out ver))
                return new ServiceDesc(s.Substring(0, k), ver);

            string filename = s.Substring(k + 1);
            if (filename.IndexOf('\\') < 0 && filename.EndsWith(".config", StringComparison.OrdinalIgnoreCase))
            {
                string sver = Path.GetFileNameWithoutExtension(filename);
                if (SVersion.TryParse(sver, out ver))
                    return new ServiceDesc(s.Substring(0, k), ver);
            }

            return null;
        }

        private static bool _StartWith(string s, string path, string[] dirNames)
        {
            foreach (string dirName in dirNames)
            {
                if (s.StartsWith(Path.Combine(path, dirName) + "\\", StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void _ctlList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ListBox lb = (ListBox)sender;
            ItemBase item = lb.Items[e.Index] as ItemBase;

            if ((e.State & DrawItemState.Selected) != 0)
            {
                e.DrawBackground();
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            }

            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            int imgWidth = lb.ItemHeight;
            IUIObjectImageLoader imgLoader = _GetImageLoader(item.ImageResName);
            if (imgLoader != null)
                e.Graphics.DrawImage(imgLoader.GetImage(imgWidth), e.Bounds.Location);

            Color color = (e.State & DrawItemState.Selected) != 0 ? Color.White : e.ForeColor;
            Rectangle rect = new Rectangle(e.Bounds.Left + imgWidth + 2, e.Bounds.Top, e.Bounds.Width - imgWidth - 2, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, item.ToString(), e.Font, rect, color, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (_ctlList.SelectedItems.Count == 0)
                {
                    this.ShowError("未选择任何内容");
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 创建安装包
        /// </summary>
        /// <returns></returns>
        public byte[] BuildDeployPackage()
        {
            return WinFormUtility.InvokeWithProgressWindow(this,
                () => StreamUtility.CompressDirectory(_GetProcessFilterItems()), "正在生成安装包");
        }

        private IEnumerable<CompressFileItemBase> _GetProcessFilterItems()
        {
            return _ctlList.SelectedItems.Cast<ItemBase>().SelectMany(item => item.GetCompressFileItems(AssemblyPath));
        }
    }
}
