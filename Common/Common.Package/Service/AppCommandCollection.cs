using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts.Service;
using System.Reflection;
using Common.Contracts;

namespace Common.Package.Service
{
    /// <summary>
    /// AppCommand的集合
    /// </summary>
    public class AppCommandCollection : IEnumerable<IAppCommand>
    {
        public AppCommandCollection()
        {

        }

        public AppCommandCollection(IEnumerable<IAppCommand> commands)
        {
            Contract.Requires(commands != null);

            AddRange(commands);
        }

        public AppCommandCollection(IAppCommand[] commands)
            : this((IEnumerable<IAppCommand>)commands)
        {
            
        }

        /// <summary>
        /// 添加一个Command
        /// </summary>
        /// <param name="info"></param
        /// <param name="cmd"></param>
        public void Add(IAppCommand command)
        {
            using (_locker.Write())
            {
                _Add(command);
            }
        }

        private void _Add(IAppCommand command)
        {
            Contract.Requires(command != null);

            AppCommandInfo info = command.GetInfo();
            var versionDict = _dict.GetOrSet(info.CommandDesc.Name);
            if (versionDict.ContainsKey(info.CommandDesc.Version))
                throw new InvalidOperationException("接口" + info.CommandDesc + "已经存在");

            versionDict.Add(info.CommandDesc.Version, new AppCommandItem { Info = info, Command = command });
        }

        /// <summary>
        /// 批量添加Command
        /// </summary>
        /// <param name="commands"></param>
        public void AddRange(IEnumerable<IAppCommand> commands)
        {
            using (_locker.Write())
            {
                foreach (IAppCommand cmd in commands)
                {
                    _Add(cmd);
                }
            }
        }

        /// <summary>
        /// 获取一个Command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private AppCommandItem _GetCommandItem(string name, SVersion version)
        {
            Contract.Requires(name != null);

            using (_locker.Read())
            {
                Dictionary<SVersion, AppCommandItem> versionDict = _dict.GetOrDefault(name);
                if (versionDict == null || versionDict.Count == 0)
                    return null;

                if (version == default(SVersion))
                    return versionDict[versionDict.Keys.Max()];  // 未指定版本号时取最大的版本号
                else
                    return versionDict.GetOrDefault(version);
            }
        }

        /// <summary>
        /// 获取一个Command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public IAppCommand GetAppCommand(string name, SVersion version = default(SVersion))
        {
            Contract.Requires(name != null);

            AppCommandItem item = _GetCommandItem(name, version);
            return item == null ? null : item.Command;
        }

        /// <summary>
        /// 获取一个CommandInfo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public AppCommandInfo GetAppCommandInfo(string name, SVersion version = default(SVersion))
        {
            Contract.Requires(name != null);

            AppCommandItem item = _GetCommandItem(name, version);
            return item == null ? null : item.Info;
        }

        /// <summary>
        /// 删除一个Command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool RemoveCommand(string name, SVersion version)
        {
            if (name == null)
                return false;

            using (_locker.Write())
            {
                Dictionary<SVersion, AppCommandItem> versionDict = _dict.GetOrDefault(name);
                if (versionDict == null)
                    return false;

                return versionDict.Remove(version);
            }
        }

        /// <summary>
        /// 调用指定的Command
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <param name="arg"></param>
        /// <param name="version"></param>
        public OutputAppCommandArg Call(AppCommandExecuteContext context, string command, InputAppCommandArg arg, SVersion version = default(SVersion))
        {
            Contract.Requires(command != null);

            IAppCommand cmd = GetAppCommand(command, version);
            if (cmd == null)
                throw _CreateNotFoundError(command, version);

            return cmd.Execute(context, arg);
        }

        private static ServiceException _CreateNotFoundError(string command, SVersion version)
        {
            return new ServiceException(ServerErrorCode.NotFound, "未找到接口:" + new CommandDesc(command, version));
        }

        /// <summary>
        /// 是否包含指定的服务
        /// </summary>
        /// <param name="command"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool Contains(string command, SVersion version = default(SVersion))
        {
            Contract.Requires(command != null);

            return GetAppCommand(command, version) != null;
        }

        /// <summary>
        /// 调用指定的Command
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public CommunicateData Call(AppCommandExecuteContext context, string command, CommunicateData data, SVersion version = default(SVersion))
        {
            Contract.Requires(command != null && data != null);

            AppCommandItem cmdItem = _GetCommandItem(command, version);
            if (cmdItem == null)
                throw _CreateNotFoundError(command, version);

            AppCommandInfo cmdInfo = GetAppCommandInfo(command, version);
            Type inputParameterType = cmdInfo.InputParameter.ParameterType;
            object inputData = (inputParameterType == null) ? null : DataBufferParser.Deserialize(data, inputParameterType);

            OutputAppCommandArg outputArg = cmdItem.Command.Execute(context, new InputAppCommandArg(inputData));
            return DataBufferParser.Serialize(outputArg, data.DataFormat);
        }

        /// <summary>
        /// 获取所有AppCommand的信息
        /// </summary>
        /// <returns></returns>
        public AppCommandInfo[] GetAppCommandInfos()
        {
            using (_locker.Read())
            {
                return _dict.Values.SelectMany(verDict => verDict.Values.Select(item => item.Info)).ToArray();
            }
        }

        /// <summary>
        /// 获取所有AppCommands
        /// </summary>
        /// <returns></returns>
        public IAppCommand[] GetAppCommands()
        {
            using (_locker.Read())
            {
                return _dict.Values.SelectMany(verDict => verDict.Values.Select(item => item.Command)).ToArray();
            }
        }

        public IEnumerator<IAppCommand> GetEnumerator()
        {
            return ((IEnumerable<IAppCommand>)GetAppCommands()).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Class AppCommandItem ...

        class AppCommandItem
        {
            public AppCommandInfo Info;
            public IAppCommand Command;
        }

        #endregion

        private readonly Dictionary<string, Dictionary<SVersion, AppCommandItem>> _dict = new Dictionary<string, Dictionary<SVersion, AppCommandItem>>();
        private readonly RwLocker _locker = new RwLocker();

        private static AppCommandCollection _LoadFromTypes(IDictionary<AppCommandAttribute, Type> dict, object[] arguments = null)
        {
            AppCommandCollection cmds = new AppCommandCollection();
            foreach (var item in dict)
            {
                AppCommandAttribute attr = item.Key;
                Type type = item.Value;

                if (!arguments.IsNullOrEmpty())
                {
                    if (type.GetConstructor(arguments.ToArray(a => a == null ? typeof(object) : a.GetType())) == null)
                        arguments = Array<object>.Empty;
                }

                IAppCommand cmd = Activator.CreateInstance(type, arguments) as IAppCommand;
                if (cmd == null)
                    throw new ServiceException(ServerErrorCode.ServerError, attr + "未实现接口" + typeof(IAppCommand).Name);

                cmds.Add(cmd);
            }

            return cmds;
        }

        /// <summary>
        /// 从程序集中加载AppCommandManager
        /// </summary>
        /// <param name="assemblies">程序集</param>
        /// <returns></returns>
        public static AppCommandCollection LoadFromAssemblies(Assembly[] assemblies)
        {
            Contract.Requires(assemblies != null);

            return _LoadFromTypes(assemblies.SearchTypes(delegate(AppCommandAttribute[] attrs, Type t) { return attrs[0]; }));
        }

        /// <summary>
        /// 从程序集中加载AppCommandManager
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static AppCommandCollection LoadFromAssembly(Assembly assembly)
        {
            return LoadFromAssemblies(new[] { assembly });
        }

        /// <summary>
        /// 从类型中加载AppCommandManager
        /// </summary>
        /// <param name="types">类型</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        public static AppCommandCollection LoadFromTypes(Type[] types, object[] arguments = null)
        {
            Contract.Requires(types != null);

            BindingFlags f = BindingFlags.Public | BindingFlags.NonPublic;
            return _LoadFromTypes(types.SearchNestedTypes(delegate(AppCommandAttribute[] attrs) { return attrs[0]; }, f, true), arguments);
        }

        /// <summary>
        /// 组合多个AppCommandCollection
        /// </summary>
        /// <param name="cmdsCollection"></param>
        /// <returns></returns>
        public static AppCommandCollection Combine(params AppCommandCollection[] cmdsCollection)
        {
            AppCommandCollection r = new AppCommandCollection();
            foreach (AppCommandCollection cmdCollection in cmdsCollection)
            {
                if (cmdCollection != null)
                    r.AddRange(cmdCollection);
            }

            return r;
        }

        /// <summary>
        /// 从类型中加载AppCommandManager
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        public static AppCommandCollection LoadFromType(Type type, object[] arguments = null)
        {
            Contract.Requires(type != null);
            return LoadFromTypes(new[] { type }, arguments);
        }
    }
}
