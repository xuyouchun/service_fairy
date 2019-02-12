using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Contracts.Log;
using Common.Utility;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.WinForm
{
    public partial class LogItemViewDialog : XDialog
    {
        public LogItemViewDialog()
        {
            InitializeComponent();
        }

        public void SetValue(LogItem logItem)
        {
            logItem = logItem ?? LogItem.Empty;

            _ctlTitle.Text = logItem.Title ?? string.Empty;
            _ctlDetail.Text = logItem.Detail ?? string.Empty;
            _ctlTime.Text = _GetLocalTimeString(logItem.Time);
            _ctlSource.Text = logItem.Source ?? string.Empty;

            _logString = logItem.ToString();
        }

        public void SetValue(SystemLogItem logItem)
        {
            _ctlTitle.Text = logItem.Message ?? string.Empty;
            _ctlDetail.Text = logItem.Message ?? string.Empty;
            _ctlTime.Text = _GetLocalTimeString(logItem.Time);
            _ctlSource.Text = logItem.Source ?? string.Empty;

            _logString = logItem.ToString();
        }

        private string _logString;

        /// <summary>
        /// 获取转换为本地时间的字符串形式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string _GetLocalTimeString(DateTime dt)
        {
            if (dt.IsEmpty())
                return string.Empty;

            return dt.ToLocalTime().ToString();
        }

        private void _ctlCopy_Click(object sender, EventArgs e)
        {
            string txt = _logString;
             if (!string.IsNullOrWhiteSpace(txt))
                 Clipboard.SetText(txt);
        }
    }
}
