using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 操作符信息管理器
    /// </summary>
    static class OperatorInfoManager
    {
        static OperatorInfoManager()
        {
            _LoadInfos();
        }

        /// <summary>
        /// 获取操作符的信息
        /// </summary>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        public static OperatorInfo GetInfo(Operators operatorType)
        {
            OperatorInfo info;
            if (!_Dict.TryGetValue(operatorType, out info))
                throw new ScriptException("不支持运算符“" + operatorType + "”");

            return info;
        }

        /// <summary>
        /// 获取操作符的信息
        /// </summary>
        /// <param name="opName"></param>
        /// <returns></returns>
        public static OperatorInfo GetInfo(string opName)
        {
            if (opName == null)
                return null;

            OperatorInfo info;
            _Dict2.TryGetValue(opName.ToUpper(), out info);
            return info;
        }

        private static readonly Dictionary<Operators, OperatorInfo> _Dict = new Dictionary<Operators, OperatorInfo>();
        private static readonly Dictionary<string, OperatorInfo> _Dict2 = new Dictionary<string, OperatorInfo>();

        private static void _LoadInfos()
        {
            foreach (Type type in typeof(OperatorInfoManager).Assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(ExpressionAttribute), false);
                if (attrs.Length > 0)
                {
                    ExpressionAttribute attr = (ExpressionAttribute)attrs[0];
                    OperatorAttribute opAttr = OperatorAttribute.GetAttribute(attr.OperatorType);
                    ParameterInfo pInfo = new ParameterInfo(opAttr.ParameterCount, opAttr.ParameterCountOnLeft);
                    OperatorInfo opInfo = new OperatorInfo(opAttr.OperatorName, opAttr.Priority, pInfo, _CreateCreator(attr.CreatorType));
                    _Dict.Add(attr.OperatorType, opInfo);
                    _Dict2[opInfo.OperatorName.ToUpper()] = opInfo;
                }
            }
        }

        private static IExpressionCreator _CreateCreator(Type creatorType)
        {
            return (IExpressionCreator)Activator.CreateInstance(creatorType);
        }
    }
}
