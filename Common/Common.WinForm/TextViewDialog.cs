using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.WinForm
{
    public partial class TextViewDialog : XDialog
    {
        public TextViewDialog()
        {
            InitializeComponent();
        }

        public new string Text
        {
            get
            {
                return _txt.Text;
            }
            set
            {
                _txt.Text = value;
            }
        }

        public string Title
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text"></param>
        /// <param name="title"></param>
        public static void Show(IWin32Window owner, string text, string title = null)
        {
            TextViewDialog dlg = new TextViewDialog();
            dlg.Text = text ?? string.Empty;
            dlg.Title = title ?? string.Empty;
            dlg.ShowDialog(owner);
        }
    }
}
