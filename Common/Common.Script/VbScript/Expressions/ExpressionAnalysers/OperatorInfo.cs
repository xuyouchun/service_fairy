using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 操作符的信息
    /// </summary>
    class OperatorInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="opName">运算符名称</param>
        /// <param name="priority">优先级</param>
        /// <param name="operatorInfo">参数信息</param>
        /// <param name="creator">创建器</param>
        public OperatorInfo(string opName, int priority, ParameterInfo parameterInfo, IExpressionCreator creator)
        {
            if (creator == null || parameterInfo == null)
                throw new ArgumentNullException(creator == null ? "creator" : "parameterInfo");

            OperatorName = opName;
            ParameterInfo = parameterInfo;
            Priority = priority;
            Creator = creator;
        }

        public string OperatorName { get; private set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public ParameterInfo ParameterInfo { get; private set; }

        /// <summary>
        /// 创建器
        /// </summary>
        public IExpressionCreator Creator { get; private set; }

        public Expression CreateExpression(Expression[] exps)
        {
            return Creator.Create(exps);
        }

        public override string ToString()
        {
            return OperatorName;
        }
    }
}
