using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Common.Package.Service
{
    /// <summary>
    /// 使用对象类型作为命名空间的信息标注
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class SoInfoAttribute : ServiceObjectInfoAttributeBase
    {
        public SoInfoAttribute()
        {

        }

        public SoInfoAttribute(string title, string desc = "")
        {
            Title = title;
            Desc = desc;
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 帮助
        /// </summary>
        public string Help { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        public override ServiceObjectInfo GetServiceObjectInfo(ICustomAttributeProvider attrProvider)
        {
            Contract.Requires(attrProvider != null);

            return new ServiceObjectInfo(
                !string.IsNullOrEmpty(Namespace) ? Namespace : GetNameSpaceFromType(attrProvider),
                !string.IsNullOrEmpty(Name) ? Name : GetName(attrProvider),
                !string.IsNullOrEmpty(Title) ? Title : Name,
                Desc, Help, Weight);
        }
    }
}
