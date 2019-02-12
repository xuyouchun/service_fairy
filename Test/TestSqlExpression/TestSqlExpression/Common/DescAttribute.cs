using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DescAttributeBase : Attribute
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
