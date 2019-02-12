using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 函数调用信息
    /// </summary>
    public class FunctionInvokeInfo
    {
        public FunctionInvokeInfo(string name, Expression[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        /// <summary>
        /// 函数名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Expression[] Parameters { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append("(");

            for (int k = 0; k < Parameters.Length; k++)
            {
                if (k > 0)
                    sb.Append(", ");

                sb.Append(Parameters[k]);
            }

            sb.Append(")");

            return sb.ToString();
        }
    }
}
