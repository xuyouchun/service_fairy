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
    public partial class InputDialog : XDialog
    {
        public InputDialog(string initText = "", string prompt = "", string title = "", bool mutiline = false)
        {
            InitializeComponent();

            _lbText.Text = prompt;
            Text = title;

            if (mutiline)
            {
                _txtInput.Multiline = true;
                Height += 100;
            }

            _txtInput.Text = initText;
        }

        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="initText"></param>
        /// <param name="text"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <param name="mutiline"></param>
        /// <returns></returns>
        public static string Show(IWin32Window owner, string initText, string prompt, string title, bool mutiline = false)
        {
            InputDialog dlg = new InputDialog(initText, prompt, title, mutiline);
            if (dlg.ShowDialog(owner) == DialogResult.OK)
                return dlg._txtInput.Text;

            return null;
        }

        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="initText"></param>
        /// <param name="prompt"></param>
        /// <param name="mutiline"></param>
        /// <returns></returns>
        public static string Show(IWin32Window owner, string initText, string prompt = "", bool mutiline = false)
        {
            return Show(owner, initText, prompt, "", mutiline);
        }
    }
}
