using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.Threading;
using Common.Contracts;

namespace Common.Package.Service
{
    /// <summary>
    /// AppCommand的基类
    /// </summary>
    public abstract class AppCommandBase : MarshalByRefObjectEx, IAppCommand
    {
        public AppCommandBase()
        {
            _objectPropertyLoader = new ObjectPropertyLoader(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <returns></returns>
        public AppCommandInfo GetInfo()
        {
            return _info ?? (_info = OnCreateInfo());
        }

        private AppCommandInfo _info;

        protected virtual AppCommandInfo OnCreateInfo()
        {
            Type t = this.GetType();
            AppCommandAttribute attr = t.GetAttribute<AppCommandAttribute>();
            return _Combine(attr == null ? _GetDefaultCmdInfo() : AppCommandAttribute.GetAppCommandInfo(t),
                DocUtility.GetSummary(t), DocUtility.GetRemarks(t));
        }

        private AppCommandInfo _GetDefaultCmdInfo()
        {
            Type t = this.GetType();
            string cmdName = t.GetName().TrimEnd("AppCommand");
            return new AppCommandInfo(new CommandDesc(cmdName, SVersion.Version_1));
        }

        private AppCommandInfo _Combine(AppCommandInfo info, string title, string desc)
        {
            return new AppCommandInfo(info.CommandDesc,
                inputParameter: info.InputParameter,
                outputParameter: info.OutputParameter,
                title: StringUtility.GetFirstNotNullOrWhiteSpaceString(info.Title, title) ?? string.Empty,
                desc: StringUtility.GetFirstNotNullOrWhiteSpaceString(info.Desc, desc) ?? string.Empty,
                category: info.Category,
                securityLevel: info.SecurityLevel,
                usable: info.Usable,
                usableDesc: info.UsableDesc);
        }

        /// <summary>
        /// 执行该方法
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public virtual OutputAppCommandArg Execute(AppCommandExecuteContext context, InputAppCommandArg arg)
        {
            Contract.Requires(context != null && arg != null);

            return OnExecute(context, arg);
        }

        /// <summary>
        /// 执行该方法
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected abstract OutputAppCommandArg OnExecute(AppCommandExecuteContext context, InputAppCommandArg arg);

        public virtual void Dispose()
        {

        }

        [ObjectProperty(false)]
        private readonly ObjectPropertyLoader _objectPropertyLoader;

        public virtual ObjectProperty[] GetAllProperties()
        {
            return _objectPropertyLoader.GetAllProperties();
        }

        public virtual ObjectPropertyValue GetPropertyValue(string propertyName)
        {
            return _objectPropertyLoader.GetPropertyValue(propertyName);
        }

        public virtual void SetPropertyValue(ObjectPropertyValue value)
        {
            _objectPropertyLoader.SetPropertyValue(value);
        }
    }
}
