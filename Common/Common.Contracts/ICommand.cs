using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 命令
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行该命令
        /// </summary>
        /// <param name="context"></param>
        void Execute(object context);

        /// <summary>
        /// 该命令是否有效
        /// </summary>
        /// <param name="context"></param>
        bool IsAvaliable(object context);

        /// <summary>
        /// 重做
        /// </summary>
        void Redo();

        /// <summary>
        /// 撤销
        /// </summary>
        void Undo();

        /// <summary>
        /// 是否支持重做
        /// </summary>
        /// <returns></returns>
        bool CanRedo();

        /// <summary>
        /// 是否支持撤销
        /// </summary>
        /// <returns></returns>
        bool CanUndo();
    }
}
