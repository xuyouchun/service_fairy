using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 用于实现大多数命令的基类
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        public abstract void Execute(object context);

        public virtual bool IsAvaliable(object context)
        {
            return true;
        }

        public virtual void Redo()
        {
            throw new NotSupportedException("不支持的操作:Redo");
        }

        public virtual void Undo()
        {
            throw new NotSupportedException("不支持的操作:Undo");
        }

        public bool CanRedo()
        {
            return false;
        }

        public bool CanUndo()
        {
            return false;
        }
    }
}
