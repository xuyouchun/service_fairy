using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Utility;
using ServiceFairy.Entities.Master;
using System.IO;
using Common.Contracts;
using ServiceFairy.Entities;

namespace ServiceFairy.WinForm
{
    public partial class UploadPackageDialog : XDialog
    {
        public UploadPackageDialog()
        {
            InitializeComponent();

            _dlg = new PathSelectDialog(this);
        }

        private readonly string _initDirectory;

        /// <summary>
        /// 设置初始值
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="title"></param>
        /// <param name="basePath"></param>
        public void SetInitValue(IValidator validator, string title, string basePath = null)
        {
            _validator = validator;

            if (basePath != null && Directory.Exists(basePath))
                _dlg = new PathSelectDialog(this, basePath);

            _ctlTitle.Text = title ?? string.Empty;
        }

        #region Class FileItem ...

        public class FileItem
        {
            internal FileItem(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; private set; }

            public override int GetHashCode()
            {
                return FileName.ToLower().GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if(obj==null || obj.GetType() != typeof(FileItem))
                    return false;

                return string.Equals(FileName, ((FileItem)obj).FileName, StringComparison.OrdinalIgnoreCase);
            }

            public override string ToString()
            {
                return FileName;
            }
        }

        #endregion

        #region Class DirectoryItem ...

        public class DirectoryItem
        {
            internal DirectoryItem(string directoryName)
            {
                DirectoryName = directoryName;
            }

            public string DirectoryName { get; private set; }

            public override int GetHashCode()
            {
                return DirectoryName.ToLower().GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(FileItem))
                    return false;

                return string.Equals(DirectoryName, ((DirectoryItem)obj).DirectoryName, StringComparison.OrdinalIgnoreCase);
            }

            public override string ToString()
            {
                return DirectoryName;
            }
        }

        #endregion

        private PathSelectDialog _dlg;
        private IValidator _validator;

        /// <summary>
        /// 设置验证器
        /// </summary>
        /// <param name="validator"></param>
        public void SetValidator(IValidator validator)
        {
            _validator = validator;
        }

        private void _ctlAddFiles_Click(object sender, EventArgs e)
        {
            string[] files = _dlg.SelectFiles("添加文件", "所有文件(*.*)|*.*", 1);

            if (!files.IsNullOrEmpty())
            {
                FileItem[] fileItems = files.ToArray(f => new FileItem(f));
                object[] fs = fileItems.Where(f => !_fileList.Items.Contains(f)).ToArray();
                _fileList.Items.AddRange(fs);
                _fileList.SelectedItems.Clear();
                fs.ForEach(f => _fileList.SelectedItems.Add(f));
            }
        }

        private void _ctlAddDirectories_Click(object sender, EventArgs e)
        {
            string dir = _dlg.SelectFolder("添加目录");
            if (dir != null)
            {
                DirectoryItem item = new DirectoryItem(dir);
                if (!_fileList.Items.Contains(dir))
                    _fileList.Items.Add(item);
            }
        }

        private void _ctlRemove_Click(object sender, EventArgs e)
        {
            object[] values = _fileList.SelectedItems.CastAsObject();

            _fileList.BeginUpdate();
            foreach (object value in values)
            {
                _fileList.Items.Remove(value);
            }
            _fileList.EndUpdate();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (_fileList.Items.Count == 0)
                {
                    this.ShowError("尚未添加任何文件");
                    e.Cancel = true;
                    return;
                }

                ValidateResult vr;
                if (_validator != null && !(vr = _validator.Validate(_fileList.Items.CastAsObject())))
                {
                    this.ShowError(vr.Message);
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 获取安装包的信息及内容
        /// </summary>
        /// <param name="packageInfo"></param>
        /// <param name="content"></param>
        public void GetPackage(DeployPackageInfo packageInfo, out byte[] content)
        {
            content = StreamUtility.CompressDirectory(_GetFileItems());
            DateTime now = DateTime.UtcNow;

            packageInfo.Format = DeployPackageFormat.GZipCompress;
            packageInfo.Id = Guid.NewGuid();
            packageInfo.LastUpdate = now;
            packageInfo.Size = content.Length;
            packageInfo.Title = _ctlTitle.Text;
            packageInfo.UploadTime = now;
        }

        private IEnumerable<CompressFileItem> _GetFileItems()
        {
            foreach (object item in _fileList.Items)
            {
                FileItem fileItem = item as FileItem;
                DirectoryItem direcotryItem;
                if (fileItem != null)
                {
                    yield return new CompressFileItem(fileItem.FileName, Path.GetFileName(fileItem.FileName));
                }
                else if ((direcotryItem = item as DirectoryItem) != null)
                {
                    foreach (CompressFileItem fi in _GetFileItemsFromDirectory(direcotryItem.DirectoryName))
                    {
                        yield return fi;
                    }
                }
            }
        }

        private IEnumerable<CompressFileItem> _GetFileItemsFromDirectory(string directory)
        {
            directory = directory.TrimEnd('\\');
            string pathName = Path.GetFileName(directory);
            foreach (string file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
            {
                string relPath = Path.Combine(pathName, file.Substring(directory.Length).TruncateFrom('\\'));
                yield return new CompressFileItem(file, relPath);
            }
        }
    }
}
