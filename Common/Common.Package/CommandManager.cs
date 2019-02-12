using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Common.Package
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    public class CommandManager
    {
        private readonly Dictionary<string, Type> _commands = new Dictionary<string, Type>();
        private readonly RwLocker _locker = new RwLocker();

        /// <summary>
        /// 查找命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public ICommand GetCommand(string commandName)
        {
            Contract.Requires(commandName != null);

            Type t;
            using (_locker.Read())
            {
                if (!_commands.TryGetValue(commandName, out t))
                    return null;
            }

            return ObjectFactory.CreateObject(t) as ICommand;
        }

        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandType"></param>
        public void AddCommand(string commandName, Type commandType)
        {
            Contract.Requires(commandName != null && commandType != null);

            using (_locker.Write())
            {
                _commands[commandName] = commandType;
            }
        }

        /// <summary>
        /// 从程序集中加载所有的命令
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static CommandManager LoadFromAssembly(Assembly assembly)
        {
            CommandManager cm = new CommandManager();

            foreach (KeyValuePair<string[], Type> item in assembly.SearchTypes<string[], CommandAttribute>((attrs, t) => attrs.ToArray(attr => attr.CommandName), true))
            {
                foreach (string commandName in item.Key)
                {
                    if (commandName != null && ReflectionUtility.IsImplementsInterface(item.Value, typeof(ICommand)))
                        cm._commands.Add(commandName, item.Value);
                }
            }

            return cm;
        }
    }
}
