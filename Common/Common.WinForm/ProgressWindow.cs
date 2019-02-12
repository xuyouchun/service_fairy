using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Common.Utility;

namespace Common.WinForm
{
    public partial class ProgressWindow : XForm
    {
        private ProgressWindow(string prompt, ManualResetEvent waitHandle, bool showCancelButton)
        {
            InitializeComponent();

            _lbTxt.Text = prompt;
            _waitHandle = waitHandle;

            _showCancelButton = showCancelButton;
            if (!(_ctlCancel.Visible = showCancelButton))
                Height -= 20;
        }

        private void _ctlCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool _showCancelButton;
        private bool? _canceled;
        private bool _closed;

        private void ProgressWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _closed = true;
            _SetCanceled(true);
            _waitHandle.Set();
        }

        private void _Close()
        {
            _SetCanceled(false);

            Close();
        }

        private void _SetCanceled(bool canceled)
        {
            if (_canceled == null)
                _canceled = canceled;
        }

        private readonly ManualResetEvent _waitHandle;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams mdiCp = base.CreateParams;

                if (!_showCancelButton)
                {
                    const int CP_NOCLOSE_BUTTON = 0x200;
                    mdiCp.ClassStyle = mdiCp.ClassStyle | CP_NOCLOSE_BUTTON;
                }

                return mdiCp;
            }
        }

        public static bool Show(IWin32Window window, string prompt, WaitHandle waitHandle,
            TimeSpan showDelay = default(TimeSpan), bool showCancelButton = true, bool topMost = false)
        {
            if (showDelay > TimeSpan.Zero)
            {
                if (waitHandle.WaitOne(showDelay))
                    return true;
            }

            ManualResetEvent e = new ManualResetEvent(false);
            ProgressWindow pw = new ProgressWindow(prompt, e, showCancelButton);
            pw.TopMost = topMost;

            Thread thread = ThreadUtility.StartNew(delegate {
                WaitHandle.WaitAny(new WaitHandle[] { waitHandle, e });
                _CloseWindow(pw);
            });

            pw.ShowDialog(window);
            return pw._canceled != true;
        }

        private static void _CloseWindow(ProgressWindow pw)
        {
            if (!pw._closed)
            {
                try
                {
                    pw.Invoke(pw._Close);
                }
                catch { }
            }
        }
    }
}
