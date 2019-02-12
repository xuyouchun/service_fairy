using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    /// <summary>
    /// 函数的信息
    /// </summary>
    class FunctionInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionType"></param>
        /// <param name="parameterTypes"></param>
        public FunctionInfo(string functionName, Type functionType, Type[] parameterTypes, string description, string paramInfo)
        {
            FunctionName = functionName;
            FunctionType = functionType;
            ParameterTypes = parameterTypes;
            Description = description;
            ParamInfo = paramInfo;
        }

        /// <summary>
        /// 函数名字
        /// </summary>
        public string FunctionName { get; private set; }

        /// <summary>
        /// 函数执行器的类型
        /// </summary>
        public Type FunctionType { get; private set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public Type[] ParameterTypes { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public string ParamInfo { get; private set; }

        private IFunction _Function;

        /// <summary>
        /// 创建函数执行器
        /// </summary>
        /// <returns></returns>
        public IFunction CreateFunction()
        {
            if (_Function == null)
                _Function = (IFunction)Activator.CreateInstance(FunctionType);

            return _Function;
        }
    }
}
