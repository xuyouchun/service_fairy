using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;

namespace ServiceFairy.Cluster.WinForm.ObjectViewers
{
    public partial class AppClientObjectViewControl : ObjectViewControl
    {
        public AppClientObjectViewControl(ClusterContext context)
        {
            InitializeComponent();

            _context = context;
        }

        private readonly ClusterContext _context;
        private AppClient _appClient;

        public override void SetData(object data)
        {
            _appClient = (AppClient)data;
            if (_appClient == null)
            {
                // Clear...
                return;
            }

            AppClient client = _appClient;
            _txtClientId.Text = client.ClientID.ToString();
            _txtStatus.Text = client.Running ? "正在运行" : "已停止";
            _btnStartOrStop.Text = client.Running ? "停止" : "启动";
        }

        private void _btnStartOrStop_Click(object sender, EventArgs e)
        {
            AppClient client = _appClient;
            if (client == null)
                return;

            try
            {
                if (client.Running)
                    client.Stop();
                else
                    client.Start();

                _btnStartOrStop.Text = client.Running ? "停止" : "启动";
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }
    }
}
