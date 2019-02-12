using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 函数信息
    /// </summary>
    public class FunctionDefineInfo
    {
        public FunctionDefineInfo(string name)
            : this(name, FunctionType.Inline, string.Empty, string.Empty, string.Empty)
        {

        }

        public FunctionDefineInfo(string name, FunctionType type, string description, string paramInfo)
            : this(name, type, description, paramInfo, string.Empty)
        {

        }

        public FunctionDefineInfo(string name, FunctionType type, string description, string paramInfo, string refFile)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            Type = type;
            Description = description;
            ParamInfo = paramInfo;
            RefFile = refFile;
        }

        /// <summary>
        /// 函数名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 函数类型
        /// </summary>
        public FunctionType Type { get; private set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public string ParamInfo { get; private set; }

        /// <summary>
        /// 相关文件（用于该函数是从外部导入的情况）
        /// </summary>
        public string RefFile { get; private set; }

        /// <summary>
        /// 寻找内置函数信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FunctionDefineInfo GetInnerFunctionInfo(string name)
        {
            if (name == null)
                return null;

            var fInfo = Common.Client.Script.Expressions.Functions.FunctionManager.GetFunctionInfo(name);
            if (fInfo != null)
                return new FunctionDefineInfo(fInfo.FunctionName, FunctionType.Inline, fInfo.Description, fInfo.ParamInfo);

            return null;
        }

        /// <summary>
        /// 获取所有的内置函数信息
        /// </summary>
        /// <returns></returns>
        public static FunctionDefineInfo[] GetAllInnerFunctionInfos()
        {
            List<FunctionDefineInfo> infos = new List<FunctionDefineInfo>();

            foreach (var fInfo in Common.Client.Script.Expressions.Functions.FunctionManager.GetAllFunctionInfos())
            {
                infos.Add(new FunctionDefineInfo(fInfo.FunctionName, FunctionType.Inline, fInfo.Description, fInfo.ParamInfo));
            }

            return infos.ToArray();
        }

        /// <summary>
        /// 获取用vbscript代码自定义的函数信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static FunctionDefineInfo GetStatementDefinedFuncInfo(Statement statement, string name)
        {
            if (name == null || statement==null)
                return null;

            return statement.GetFuncInfo(name);
        }

        /// <summary>
        /// 获取所有模块内部定义的函数信息
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static FunctionDefineInfo[] GetAllStatementDefinedFuncInfo(Statement statement)
        {
            if (statement == null)
                return new FunctionDefineInfo[0];

            return statement.GetAllFuncInfos();
        }

        public override string ToString()
        {
            return Name + string.Format("({0})", ParamInfo);
        }
    }

    public enum FunctionType
    {
        Inline,

        Private,

        Extern
    }
}
