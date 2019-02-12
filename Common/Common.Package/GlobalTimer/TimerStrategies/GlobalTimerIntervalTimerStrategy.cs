using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;

namespace Common.Package.GlobalTimer.TimerStrategies
{
    /// <summary>
    /// 基于定时器频率的定时策略
    /// </summary>
    [TimerStrategy("globalTimerInterval", typeof(Creator))]
    [System.Diagnostics.DebuggerStepThrough]
    public class GlobalTimerIntervalTimerStrategy : ITimerStrategy
    {
        #region ITimerStrategy Members

        public bool IsTimeUp()
        {
            return true;
        }

        public void RunNotify()
        {
            
        }

        public void CompletedNotify()
        {
            
        }

        #endregion

        #region Class Creator ...

        class Creator : IObjectCreator
        {

            public object CreateObject(object context = null, IObjectCreator innerCreator = null)
            {
                return new GlobalTimerIntervalTimerStrategy();
            }
        }

        #endregion

    }
}
