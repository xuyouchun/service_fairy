using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Reflection;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 在类上用于标注程序集的入口点
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AppEntryPointAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppEntryPointAttribute()
        {

        }
    }

    /// <summary>
    /// 在程序集上标注程序集的入口点
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public sealed class AssemblyEntryPointAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entryPointType"></param>
        public AssemblyEntryPointAttribute(Type entryPointType)
        {
            Contract.Requires(entryPointType != null);

            EntryPointType = entryPointType;
        }

        /// <summary>
        /// 入口点类型
        /// </summary>
        public Type EntryPointType { get; private set; }
    }
}
