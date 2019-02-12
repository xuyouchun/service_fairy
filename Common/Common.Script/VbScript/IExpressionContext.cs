using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    public interface IExpressionContext
    {
        /// <summary>
        /// 根据名字读取值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Value GetValue(string name);

        /// <summary>
        /// 是否定义了该变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsDeclared(string name);

        /// <summary>
        /// 设置指定名字的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetValue(string name, Value value);

        /// <summary>
        /// 定义指定名字
        /// </summary>
        /// <param name="name"></param>
        void Declare(string name);

        /// <summary>
        /// 输入
        /// </summary>
        /// <param name="defValue"></param>
        /// <param name="description"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        Value Input(Value title, Value defValue, Value description);

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="value"></param>
        void Output(Value value);

        /// <summary>
        /// 对显示对话框的形式输出
        /// </summary>
        /// <param name="value"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        Value ShowMsgBox(Value value, Value title, Value buttons);

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <param name="returnValue">返回值</param>
        /// <returns>是否支持该函数的执行并且执行完毕</returns>
        bool ExecuteFunction(string funcName, Value[] parameters, out Value returnValue);

        /// <summary>
        /// 是否定义了指定的函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <returns></returns>
        FunctionDefineInfo GetFunctionInfo(string funcName);

        /// <summary>
        /// 获取所有的函数定义信息
        /// </summary>
        /// <returns></returns>
        FunctionDefineInfo[] GetAllFunctionInfos();
    }
}
