using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    /// <summary>
    /// 用于标注函数的属性
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class FunctionAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="forceValudate"></param>
        /// <param name="parameterTypes"></param>
        public FunctionAttribute(string funcName, bool forceValudate, params Type[] parameterTypes)
        {
            FunctionName = funcName;
            ForceValidate = forceValudate;
            ParameterTypes = parameterTypes;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="parameterTypes"></param>
        public FunctionAttribute(string funcName, params Type[] parameterTypes)
            : this(funcName, true, parameterTypes)
        {

        }

        /// <summary>
        /// 函数名字
        /// </summary>
        public string FunctionName { get; private set; }

        /// <summary>
        /// 函数参数类型
        /// </summary>
        public Type[] ParameterTypes { get; private set; }

        /// <summary>
        /// 是否严格地验证参数个数，如果为false，则忽略参数个数的验证
        /// </summary>
        public bool ForceValidate { get; private set; }
    }
}
