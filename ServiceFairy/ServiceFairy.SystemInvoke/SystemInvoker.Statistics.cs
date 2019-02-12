using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private StatisticsInvoker _statistics;

        /// <summary>
         /// Statistics Service
        /// </summary>
        public StatisticsInvoker Statistics
        {
            get { return _statistics ?? (_statistics = new StatisticsInvoker(this)); }
        }

        public class StatisticsInvoker : Invoker
        {
            public StatisticsInvoker(SystemInvoker owner)
                : base(owner)
            {

            }
        }
    }
}
