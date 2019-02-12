using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm;
using ServiceFairy.Cluster.WinForm.ObjectViewers;
using System.Windows.Forms;

namespace ServiceFairy.Cluster.WinForm.UIActions
{
    /// <summary>
    /// 
    /// </summary>
    class AppClientTreeNodeUIAction : UIActionBase
    {
        public AppClientTreeNodeUIAction(ClusterContext context, Control container, AppClient appClient)
        {
            _context = context;
            _control = new AppClientObjectViewControl(context);
            _container = container;
            _appClient = appClient;
        }

        private readonly ClusterContext _context;
        private readonly AppClientObjectViewControl _control;
        private readonly Control _container;
        private readonly AppClient _appClient;

        public override void OnAction(UIActionType actionType, object sender, EventArgs e)
        {
            if (actionType == UIActionType.TreeNodeAfterSelect)
            {
                _control.Dock = DockStyle.Fill;
                _container.Controls.Clear();
                _container.Controls.Add(_control);

                _control.SetData(_appClient);
            }
        }
    }
}
