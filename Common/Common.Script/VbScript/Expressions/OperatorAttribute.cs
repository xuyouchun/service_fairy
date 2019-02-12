using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common.Script.VbScript.Expressions
{
    [global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    class OperatorAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        /// <param name="operatorName">操作符</param>
        /// <param name="priority">优先级</param>
        /// <param name="parameterCount">参数个数</param>
        /// <param name="parameterCountOnLeft">左侧的参数个数</param>
        public OperatorAttribute(string operatorName, int priority, int parameterCount, int parameterCountOnLeft)
        {
            OperatorName = operatorName;
            Priority = priority;
            ParameterCount = parameterCount;
            ParameterCountOnLeft = parameterCountOnLeft;
            ShowInList = false;
        }

        /// <summary>
        /// 操作符名称
        /// </summary>
        public string OperatorName { get; private set; }

        /// <summary>
        /// 操作符优先级
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 参数个数
        /// </summary>
        public int ParameterCount { get; private set; }

        /// <summary>
        /// 左侧的参数个数
        /// </summary>
        public int ParameterCountOnLeft { get; private set; }

        /// <summary>
        /// 是否应该显示在编辑器的自动提示列表中
        /// </summary>
        public bool ShowInList { get; set; }

        static Dictionary<Operators, OperatorAttribute> _Dict = new Dictionary<Operators, OperatorAttribute>();

        public static OperatorAttribute GetAttribute(Operators operatorType)
        {
            OperatorAttribute attr;
            if (!_Dict.TryGetValue(operatorType, out attr))
            {
                _Dict.Add(operatorType, attr = _GetAttr(operatorType));
            }

            return attr;
        }

        private static OperatorAttribute _GetAttr(Operators operatorType)
        {
            object[] attrs = (object[])typeof(Operators)
                .GetField(operatorType.ToString()).GetCustomAttributes(typeof(OperatorAttribute), false);

            if (attrs.Length == 0)
                return null;

            return (OperatorAttribute)attrs[0];
        }

        public static IEnumerable<OperatorAttribute> GetAllOperatorInfos()
        {
            foreach (object value in Enum.GetValues(typeof(Operators)))
            {
                yield return GetAttribute((Operators)value);
            }
        }
    }
}
