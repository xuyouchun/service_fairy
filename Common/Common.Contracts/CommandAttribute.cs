using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 用于标注命令
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }

        /// <summary>
        /// 命令
        /// </summary>
        public string CommandName { get; private set; }
    }
}
