using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using Common.Utility;

namespace Common.WinForm
{
    /// <summary>
    /// 目录选择按钮
    /// </summary>
    public class SelectPathButton : Button
    {
        public SelectPathButton()
        {
            _dlg = new PathSelectDialog(this);
        }

        private Control _relationControl;
        private readonly PathSelectDialog _dlg;

        /// <summary>
        /// 相关联的控件
        /// </summary>
        [Browsable(true), Category("XControl")]
        public Control RelationControl
        {
            get { return _relationControl; }
            set
            {
                _relationControl = value;
                if (_relationControl != null)
                    _relationControl.Text = _value ?? string.Empty;
            }
        }

        private string _value;

        /// <summary>
        /// 值
        /// </summary>
        [Browsable(true), Category("XControl")]
        public string Value
        {
            get
            {
                if (RelationControl != null)
                    return _value = RelationControl.Text;

                return _value;
            }
            set
            {
                if (RelationControl != null)
                    RelationControl.Text = value ?? string.Empty;

                _value = value ?? string.Empty;
            }
        }

        /// <summary>
        /// 按钮的动作
        /// </summary>
        [Browsable(true), Category("XControl")]
        public SelectPathActionType ActionType { get; set; }

        /// <summary>
        /// 默认的扩展名
        /// </summary>
        [Browsable(true), Category("XControl")]
        public string DefaultExt { get; set; }

        /// <summary>
        /// 过滤器
        /// </summary>
        [Browsable(true), Category("XControl")]
        public string Filter { get; set; }

        /// <summary>
        /// 过滤器索引
        /// </summary>
        [Browsable(true), Category("XControl")]
        public int FilterIndex { get; set; }

        /// <summary>
        /// 是否在目录对话框上显示“新建”按钮
        /// </summary>
        [Browsable(true), Category("XControl")]
        public bool ShowNewButton { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Browsable(true), Category("XControl")]
        public string Title { get; set; }

        /// <summary>
        /// 只读检查
        /// </summary>
        [Browsable(true), Category("XControl")]
        public bool ReadOnlyChecked { get; set; }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            string path = _Select();
            if (path != null)
                Value = path;
        }

        private string _Select()
        {
            switch (ActionType)
            {
                case SelectPathActionType.Folder:
                    return _dlg.SelectFolder(description:Title, showNewFolderButton:ShowNewButton);

                case SelectPathActionType.Open:
                    return _dlg.SelectFile(title: Title, fileName: _GetFileName(), filter: Filter,
                    filterIndex: FilterIndex, defaultExt: DefaultExt, readonlyChecked: ReadOnlyChecked, saveModel: false);

                case SelectPathActionType.OpenMany:
                    return _dlg.SelectFiles(title: Title, filter: Filter, filterIndex: FilterIndex,
                    defaultExt: DefaultExt, readonlyChecked: ReadOnlyChecked).JoinBy(";");

                case SelectPathActionType.Save:
                    return _dlg.SelectFile(title: Title, fileName: _GetFileName(), filter: Filter,
                    filterIndex: FilterIndex, defaultExt: DefaultExt, readonlyChecked: ReadOnlyChecked, saveModel: true);
            }

            return null;
        }

        private string _GetFileName()
        {
            string filename = Value;
            if (File.Exists(filename))
                return filename;

            return null;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SelectPathButton
            // 
            this.Size = new System.Drawing.Size(50, 23);
            this.Text = "...";
            this.ResumeLayout(false);

        }
    }

    /// <summary>
    /// 目录选择按钮的类型
    /// </summary>
    public enum SelectPathActionType
    {
        /// <summary>
        /// 选择目录
        /// </summary>
        Folder,

        /// <summary>
        /// 打开文件
        /// </summary>
        Open,

        /// <summary>
        /// 打开多个文件
        /// </summary>
        OpenMany,

        /// <summary>
        /// 保存文件
        /// </summary>
        Save,
    }
}
