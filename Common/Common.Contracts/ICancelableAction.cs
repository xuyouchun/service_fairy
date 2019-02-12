using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;
using Common.Contracts.Service;

namespace Common.Contracts
{
    /// <summary>
    /// 支持可取消的操作
    /// </summary>
    public interface ICancelableOperation
    {
        /// <summary>
        /// 执行可取消的操作
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancelController"></param>
        bool Execute(ICancelableTask task, ICancelableController cancelController);
    }

    /// <summary>
    /// 支持可取消操作的任务
    /// </summary>
    public interface ICancelableTask : ITask
    {
        /// <summary>
        /// 取消
        /// </summary>
        void Cancel();

        /// <summary>
        /// 是否支持进度
        /// </summary>
        /// <returns></returns>
        bool SupportProgress();

        /// <summary>
        /// 获取当前进度，1为完成
        /// </summary>
        /// <returns></returns>
        float GetProgress();

        /// <summary>
        /// 进度变化事件
        /// </summary>
        event EventHandler ProgressChanged;
    }

    /// <summary>
    /// 可取消操作的控制逻辑
    /// </summary>
    public interface ICancelableController
    {
        /// <summary>
        /// 等待操作完成
        /// </summary>
        /// <returns></returns>
        bool Wait();

        /// <summary>
        /// 操作完成的通知
        /// </summary>
        void CompletedNotify();

        /// <summary>
        /// 显示进度
        /// </summary>
        /// <param name="progress"></param>
        void ShowProgress(float progress);
    }
}
