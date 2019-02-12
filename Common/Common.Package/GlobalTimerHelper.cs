using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace Common.Package
{
    public static class GlobalTimerHelper
    {
        public static IGlobalTimerTaskHandle Add(this GlobalTimer<ITask> gt, TimeSpan interval, Action func, bool enableReenter = true, bool enable = true)
        {
            Contract.Requires(gt != null);

            return gt.Add(interval, TaskFuncAdapter.Create(func), enableReenter: enableReenter, enable: enable);
        }

        public static IGlobalTimerTaskHandle Add(this GlobalTimer<ITask> gt, TimeSpan interval, TimeSpan dueTime, Action func, bool enableReenter = true, bool enable = true)
        {
            Contract.Requires(gt != null);

            return gt.Add(interval, dueTime, TaskFuncAdapter.Create(func), enableReenter: enableReenter, enable: enable);
        }
    }
}
