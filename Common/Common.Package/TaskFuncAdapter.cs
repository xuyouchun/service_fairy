using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Package
{
    [System.Diagnostics.DebuggerStepThrough]
    public class TaskFuncAdapter : ITask
    {
        public TaskFuncAdapter(Action action)
        {
            _Action = action ?? new Action(_EmptyAction);
        }

        private readonly Action _Action;

        private static void _EmptyAction()
        {

        }

        #region ITask Members

        public void Execute()
        {
            _Action();
        }

        #endregion

        public static TaskFuncAdapter Create(Action action)
        {
            return new TaskFuncAdapter(action);
        }
    }
}
