using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Common.Package
{
    /// <summary>
    /// 可取消的操作
    /// </summary>
    public class CancelableOperation : ICancelableOperation
    {
        public bool Execute(ICancelableTask task, ICancelableController cancelControl)
        {
            Contract.Requires(task != null && cancelControl != null);

            Thread thread = new Thread(_RunningFunc);
            thread.IsBackground = true;
            Context ctx = new Context() { Task = task, CancelControl = cancelControl };
            EventHandler eh = delegate(object sender, EventArgs e) { cancelControl.ShowProgress(task.GetProgress()); };

            try
            {
                task.ProgressChanged += eh;
                thread.Start(ctx);
                if (cancelControl.Wait())
                {
                    if (ctx.Error != null)
                        throw ctx.Error;

                    return true;
                }
                else
                {
                    task.Cancel();
                    return false;
                }
            }
            finally
            {
                task.ProgressChanged -= eh;
            }
        }

        class Context
        {
            public ICancelableTask Task { get; set; }

            public ICancelableController CancelControl { get; set; }

            public Exception Error { get; set; }
        }

        private void _RunningFunc(object context)
        {
            Context ctx = (Context)context;

            try
            {
                ctx.Task.Execute();
            }
            catch (Exception ex)
            {
                ctx.Error = ex;
            }
            finally
            {
                ctx.CancelControl.CompletedNotify();
            }
        }
    }
}
