using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;

namespace Common.WinForm
{
    public partial class ProgressBarDialog : XDialog
    {
        public ProgressBarDialog(string text = "", string title = "")
        {
            InitializeComponent();

            Text = title;
            _lbText.Text = text;
        }

        /// <summary>
        /// 控制器
        /// </summary>
        public CancelableController CreateController(IWin32Window owner)
        {
            return new CancelableController(this, owner);
        }

        public class CancelableController : ICancelableController
        {
            public CancelableController(ProgressBarDialog dialog, IWin32Window owner)
            {
                _dialog = dialog;
                _owner = owner;
            }

            private readonly ProgressBarDialog _dialog;
            private readonly IWin32Window _owner;

            public bool Wait()
            {
                return _dialog.ShowDialog(_owner) == DialogResult.OK;
            }

            public void CompletedNotify()
            {
                _dialog.DialogResult = DialogResult.OK;
            }

            public void ShowProgress(float progress)
            {
                progress = Math.Min(1, Math.Max(0, progress));
                ProgressBar pb = _dialog._pb;
                pb.Style = ProgressBarStyle.Blocks;
                pb.Value = (int)((pb.Maximum - pb.Minimum) * progress) + pb.Minimum;
            }
        }
    }
}
