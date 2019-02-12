using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 参数信息
    /// </summary>
    class ParameterInfo
    {
        public ParameterInfo(int parameterCount, int parameterCountOnLeft)
        {
            ParameterCount = parameterCount;
            ParameterCountOnLeft = parameterCountOnLeft;
        }

        /// <summary>
        /// 参数个数
        /// </summary>
        public int ParameterCount { get; private set; }

        /// <summary>
        /// 左侧的参数个数
        /// </summary>
        public int ParameterCountOnLeft { get; private set; }

        /// <summary>
        /// 右侧的参数个数
        /// </summary>
        public int ParameterCountOnRight { get { return ParameterCount - ParameterCountOnLeft; } }
    }
}
