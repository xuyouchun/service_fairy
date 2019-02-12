using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    /// <summary>
    /// 函数管理器
    /// </summary>
    static class FunctionManager
    {
        static readonly Dictionary<string, FunctionInfo> _FuncDict = new Dictionary<string, FunctionInfo>();

        static FunctionManager()
        {
            foreach (Type type in typeof(FunctionManager).Assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(FunctionAttribute), false);
                if (attrs.Length > 0)
                {
                    FunctionAttribute attr = (FunctionAttribute)attrs[0];
                    FunctionInfoAttribute[] infoAttrs = (FunctionInfoAttribute[])type.GetCustomAttributes(typeof(FunctionInfoAttribute), false);
                    FunctionInfoAttribute infoAttr = infoAttrs.Length > 0 ? infoAttrs[0] : null;

                    FunctionInfo fInfo = new FunctionInfo(attr.FunctionName, type, attr.ParameterTypes,
                        infoAttr == null ? string.Empty : infoAttr.Description, infoAttr == null ? string.Empty : infoAttr.ParamInfo);

                    _FuncDict.Add(attr.FunctionName.ToUpper(), fInfo);
                }
            }
        }

        /// <summary>
        /// 获取函数信息
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static FunctionInfo GetFunctionInfo(string functionName)
        {
            if (functionName == null)
                return null;

            FunctionInfo fInfo;
            _FuncDict.TryGetValue(functionName.ToUpper(), out fInfo);

            return fInfo;
        }

        /// <summary>
        /// 获取所有的函数信息
        /// </summary>
        /// <returns></returns>
        public static FunctionInfo[] GetAllFunctionInfos()
        {
            List<FunctionInfo> infos = new List<FunctionInfo>(_FuncDict.Values);
            return infos.ToArray();
        }
    }
}
