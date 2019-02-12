using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IDescProvider
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <returns></returns>
        string GetDesc();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class DescAttributeBase : Attribute, IDescProvider
    {
        public abstract string GetDesc();
    }

    /// <summary>
    /// 用于标注类及类成员的描述
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DescAttribute : DescAttributeBase
    {
        public DescAttribute(string desc)
        {
            _desc = desc ?? string.Empty;
        }

        private readonly string _desc;

        public override string GetDesc()
        {
            return _desc;
        }
    }
}
