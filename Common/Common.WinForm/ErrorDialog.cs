using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Utility;

namespace Common.WinForm
{
    public partial class ErrorDialog : XDialog
    {
        public ErrorDialog(Exception error, string caption)
        {
            InitializeComponent();

            _error = error;
            _txtMsg.Text = error.Message;
            Text = caption;
        }

        private readonly Exception _error;

        private void _btnDetail_Click(object sender, EventArgs e)
        {
            this.FindControls<Button>().ForEach(c => c.Anchor = (c.Anchor & ~AnchorStyles.Bottom) | AnchorStyles.Top);
            _txtMsg.Anchor &= ~AnchorStyles.Bottom;

            if (_txtDetail == null)
            {
                _txtDetail = new TextBox() {
                    Width = _txtMsg.Width, Left = _txtMsg.Left, Top = _btnDetail.Bottom + 10, Multiline = true, ReadOnly = true,
                    Text = _error.ToString(), Height = _detailHeight - 20, HideSelection = false, ScrollBars = ScrollBars.Vertical
                };
                this.Controls.Add(_txtDetail);
            }

            _detailShown = !_detailShown;
            if (_detailShown)
                this.Height += _detailHeight;
            else
                this.Height -= _detailHeight;

            _txtDetail.Visible = _detailShown;
            _btnCopy.Visible = _detailShown;
        }

        private void _btnCopy_Click(object sender, EventArgs e)
        {
            _txtDetail.SelectAll();
            _txtDetail.Copy();
        }

        private bool _detailShown = false;
        private const int _detailHeight = 150;
        private TextBox _txtDetail;

        /// <summary>
        /// 显示错误对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="error"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, Exception error, string caption = "错误")
        {
            if (error == null)
                return DialogResult.OK;

            ErrorDialog dlg = new ErrorDialog(error, caption);
            return dlg.ShowDialog(owner);
        }
    }
}
