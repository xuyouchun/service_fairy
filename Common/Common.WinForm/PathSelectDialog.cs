using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Common.Utility;

namespace Common.WinForm
{
    /// <summary>
    /// 文件或目录路径选择对话框
    /// </summary>
    public class PathSelectDialog
    {
        public PathSelectDialog(IWin32Window window, string initDirectory = null)
        {
            _window = window;

            if (initDirectory != null)
                _initDirectory = initDirectory;
        }

        private readonly IWin32Window _window;
        private static string _initDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// 选择一个文件夹
        /// </summary>
        /// <param name="showNewFolderButton"></param>
        /// <param name="description"></param>
        /// <param name="rootFolder"></param>
        /// <returns></returns>
        public string SelectFolder(string description = null, bool showNewFolderButton = true)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (description != null)
                dlg.Description = description;

            dlg.ShowNewFolderButton = showNewFolderButton;

            if (Directory.Exists(_initDirectory))
                dlg.SelectedPath = _initDirectory;

            if (dlg.ShowDialog(_window) == DialogResult.OK)
            {
                _initDirectory = dlg.SelectedPath;
                return dlg.SelectedPath;
            }

            return null;
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="filter">过滤器</param>
        /// <param name="fileName">默认文件</param>
        /// <param name="filterIndex">默认过滤器索引</param>
        /// <param name="defaultExt">默认的扩展名</param>
        /// <param name="readonlyChecked">是否进行文件只读检查</param>
        /// <param name="saveModel">是否为选择要保存的文件</param>
        /// <returns></returns>
        public string SelectFile(string title = null, string fileName = null, string filter = null, int filterIndex = 0, string defaultExt = null, bool readonlyChecked = false, bool saveModel = false)
        {
            FileDialog dlg;
            if (saveModel)
            {
                dlg = _InitSaveFileDialog(title, fileName, filter, filterIndex, defaultExt);
            }
            else
            {
                dlg = _InitOpenFileDialog(title, fileName, filter, filterIndex, defaultExt, readonlyChecked);
                ((OpenFileDialog)dlg).Multiselect = false;
            }

            if (dlg.ShowDialog(_window) == DialogResult.OK)
            {
                fileName = dlg.FileName;
                _initDirectory = Path.GetDirectoryName(fileName);
                return fileName;
            }

            return null;
        }

        private SaveFileDialog _InitSaveFileDialog(string title, string fileName, string filter, int filterIndex, string defaultExt)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            _InitFileDialog(dlg, title, fileName, filter, filterIndex, defaultExt);
            return dlg;
        }

        private void _InitFileDialog(FileDialog dlg, string title, string fileName, string filter, int filterIndex, string defaultExt)
        {
            if (Directory.Exists(_initDirectory))
                dlg.InitialDirectory = _initDirectory;

            if (filter != null)
                dlg.Filter = filter;

            if (filterIndex > 0)
                dlg.FilterIndex = filterIndex;
            else if (!string.IsNullOrEmpty(filter))
                dlg.FilterIndex = 1;

            if (title != null)
                dlg.Title = title;

            if (defaultExt != null)
                dlg.DefaultExt = defaultExt;
            else if (!string.IsNullOrEmpty(filter))
            {
                string[] filters = filter.Split('|');
                if (dlg.FilterIndex * 2 < filters.Length)
                    dlg.DefaultExt = filters[dlg.FilterIndex * 2].TruncateFromLast('.');
            }

            if (fileName != null)
                dlg.FileName = fileName;

            dlg.AddExtension = !string.IsNullOrEmpty(dlg.DefaultExt);
        }

        private OpenFileDialog _InitOpenFileDialog(string title, string fileName, string filter, int filterIndex, string defaultExt, bool readonlyChecked)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.ReadOnlyChecked = readonlyChecked;
            _InitFileDialog(dlg, title, fileName, filter, filterIndex, defaultExt);
            return dlg;
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <param name="filterIndex"></param>
        /// <param name="defaultExt"></param>
        /// <param name="readonlyChecked"></param>
        /// <returns></returns>
        public string[] SelectFiles(string title = null, string filter = null, int filterIndex = 0, string defaultExt = null, bool readonlyChecked = false)
        {
            OpenFileDialog dlg = _InitOpenFileDialog(title, null, filter, filterIndex, defaultExt, readonlyChecked);
            dlg.Multiselect = true;
            if (dlg.ShowDialog(_window) == DialogResult.OK)
            {
                string[] fileNames = dlg.FileNames;
                if (fileNames.Length > 0)
                {
                    _initDirectory = Path.GetDirectoryName(fileNames[0]);
                }

                return fileNames;
            }

            return null;
        }
    }
}
