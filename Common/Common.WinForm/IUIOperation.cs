using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using System.Collections;
using DevComponents.DotNetBar;
using System.ComponentModel;
using System.Threading;

namespace Common.WinForm
{
    /// <summary>
    /// 界面的操作
    /// </summary>
    public interface IUIOperation
    {
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        DialogResult ShowDialog(Form dialog);

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="form"></param>
        void ShowForm(Form form);

        /// <summary>
        /// 显示选择窗体
        /// </summary>
        /// <param name="items"></param>
        /// <param name="selectionMode"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        T[] Select<T>(T[] items, string title = "", SelectionMode selectionMode = SelectionMode.One);

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="error"></param>
        /// <param name="title"></param>
        void ShowError(Exception error, string title = "");

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="title"></param>
        void ShowError(string errorMsg, string title = "");

        /// <summary>
        /// 显示询问对话框
        /// </summary>
        /// <param name="queryMsg"></param>
        /// <param name="title"></param>
        /// <param name="showCancelButton"></param>
        /// <param name="defaultButton">默认的按钮，true/false表示是/否，null表示取消</param>
        /// <returns>true/false表示是/否，null表示取消</returns>
        bool? ShowQuery(string queryMsg, string title = "", bool showCancelButton = false, bool? defaultButton = null);

        /// <summary>
        /// 显示普通信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        void ShowMessage(string message, string title = "");

        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="defaultText"></param>
        /// <param name="title"></param>
        /// <param name="mutiline"></param>
        /// <returns></returns>
        string ShowInput(string title = "", string defaultText = "", bool mutiline = false);

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        void ShowText(string message, string title = "");

        /// <summary>
        /// 弹出消息气泡
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="autoCloseTime"></param>
        /// <returns></returns>
        void ShowPopup(string category, string message, string title = "",
            UIOperationPopupType type = UIOperationPopupType.Message, TimeSpan autoCloseTime = default(TimeSpan));

        /// <summary>
        /// 显示进度窗口
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="waitHandle"></param>
        /// <param name="showDeply">等待多长时间之后再显示</param>
        /// <param name="showCancelButton">是否显示“取消”按钮</param>
        /// <param name="topMost">是否置顶显示</param>
        /// <returns></returns>
        bool ShowProgressWindow(string title, WaitHandle waitHandle, TimeSpan showDeply = default(TimeSpan), bool showCancelButton = false, bool topMost = false);

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="description"></param>
        /// <param name="showNewFolderButton"></param>
        /// <returns></returns>
        string SelectFolder(string description = null, bool showNewFolderButton = true);

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="fileName"></param>
        /// <param name="filter"></param>
        /// <param name="filterIndex"></param>
        /// <param name="defaultExt"></param>
        /// <param name="readonlyChecked"></param>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        string SelectFile(string title = null, string fileName = null, string filter = null, int filterIndex = 0, string defaultExt = null, bool readonlyChecked = false, bool saveModel = false);

        /// <summary>
        /// 批量选择文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <param name="filterIndex"></param>
        /// <param name="defaultExt"></param>
        /// <param name="readonlyChecked"></param>
        /// <returns></returns>
        string[] SelectFiles(string title = null, string filter = null, int filterIndex = 0, string defaultExt = null, bool readonlyChecked = false);
    }

    /// <summary>
    /// 弹出消息气泡的类型
    /// </summary>
    public enum UIOperationPopupType
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        Message,

        /// <summary>
        /// 警告信息
        /// </summary>
        Warning,

        /// <summary>
        /// 错误
        /// </summary>
        Error,
    }

    /// <summary>
    /// 界面操作的基类
    /// </summary>
    public class UIOperation : IUIOperation
    {
        public UIOperation(IWin32Window owner)
        {
            _owner = owner;
            _control = _owner as Control;
            _pathSelectDialog = new PathSelectDialog(owner);
        }

        private readonly IWin32Window _owner;
        private readonly Control _control;
        private readonly PathSelectDialog _pathSelectDialog;

        private void _Invoke(Action action)
        {
            if (_control != null)
                _control.Invoke(action);
            else
                action();
        }

        private T _Invoke<T>(Func<T> func)
        {
            if (_control != null)
            {
                T result = default(T);
                _control.Invoke(delegate {
                    result = func();
                });

                return result;
            }
            else
            {
                return func();
            }
        }

        public virtual void ShowForm(Form form)
        {
            Contract.Requires(form != null);

            _Invoke(() => form.Show(_owner));
        }

        public virtual DialogResult ShowDialog(Form dialog)
        {
            Contract.Requires(dialog != null);

            return _Invoke<DialogResult>(() => dialog.ShowDialog(_owner));
        }

        public virtual T[] Select<T>(T[] items, string title, SelectionMode selectionMode)
        {
            Contract.Requires(items != null);

            return _Invoke<T[]>(() => SelectDialog.Show<T>(_owner, items, title, selectionMode));
        }

        public virtual void ShowError(Exception error, string title)
        {
            Contract.Requires(error != null);

            _Invoke(() => ErrorDialog.Show(_owner, error, title));
        }

        public virtual void ShowError(string errorMsg, string title)
        {
            _Invoke(() => WinFormUtility.ShowError(_owner, errorMsg, title));
        }

        public virtual bool? ShowQuery(string queryMsg, string title, bool showCancelButton, bool? defaultButton)
        {
            if (showCancelButton)
            {
                return _Invoke<bool?>(() => WinFormUtility.ShowQuestion(_owner, queryMsg, title, defaultButton));
            }

            return _Invoke<bool?>(()=> WinFormUtility.ShowQuestion(_owner, queryMsg, title, (bool)(defaultButton ?? false)));
        }

        public virtual void ShowMessage(string message, string title)
        {
            _Invoke(() => WinFormUtility.ShowMessage(_owner, message, title));
        }

        public virtual string ShowInput(string title, string defaultText, bool mutiline)
        {
            return _Invoke<string>(() => InputDialog.Show(_owner, defaultText, title, mutiline));
        }

        public void ShowText(string text, string title = "")
        {
            _Invoke(() => TextViewDialog.Show(_owner, text, title));
        }

        public virtual void ShowPopup(string category, string message, string title, UIOperationPopupType type, TimeSpan autoCloseTime)
        {
            
        }

        public virtual bool ShowProgressWindow(string prompt, WaitHandle waitHandle, TimeSpan showDelay = default(TimeSpan), bool showCancelButton = false, bool topMost = false)
        {
            return ProgressWindow.Show(_owner, prompt, waitHandle, showDelay, showCancelButton, topMost: topMost);
        }

        public string SelectFolder(string description = null, bool showNewFolderButton = true)
        {
            return _pathSelectDialog.SelectFolder(description, showNewFolderButton);
        }

        public string SelectFile(string title = null, string fileName = null, string filter = null, int filterIndex = 0, string defaultExt = null, bool readonlyChecked = false, bool saveModel = false)
        {
            return _pathSelectDialog.SelectFile(title, fileName, filter, filterIndex, defaultExt, readonlyChecked, saveModel);
        }

        public string[] SelectFiles(string title = null, string filter = null, int filterIndex = 0, string defaultExt = null, bool readonlyChecked = false)
        {
            return _pathSelectDialog.SelectFiles(title, filter, filterIndex, defaultExt, readonlyChecked);
        }
    }
}
