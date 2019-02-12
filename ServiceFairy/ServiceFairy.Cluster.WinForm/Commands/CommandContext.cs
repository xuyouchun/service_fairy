using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Cluster.WinForm.Commands
{
    class CommandContext
    {
        public CommandContext(MainForm mainForm, ClusterContext context, ClusterOperations operations)
        {
            MainForm = mainForm;
            Context = context;
            Operations = operations;
        }

        /// <summary>
        /// 主窗体
        /// </summary>
        public MainForm MainForm { get; private set; }

        /// <summary>
        /// 上下文执行环境
        /// </summary>
        public ClusterContext Context { get; private set; }

        /// <summary>
        /// 集群的一些操作
        /// </summary>
        public ClusterOperations Operations { get; private set; }
    }
}
