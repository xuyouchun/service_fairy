using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// VB常量信息
    /// </summary>
    public class VbConstValueInfo
    {
        public VbConstValueInfo(string name, Value value, string description)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            Value = value ?? Value.Void;
            Description = description ?? string.Empty;
        }

        /// <summary>
        /// 常量名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 常量值
        /// </summary>
        public Value Value { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }
    }
}
