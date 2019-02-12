using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 用于类内部的成员表达式的执行环境
    /// </summary>
    class ObjectExpressionContext : IExpressionContext
    {
        public ObjectExpressionContext()
        {
            
        }

        readonly Dictionary<string, Value> _Dict = new Dictionary<string, Value>();

        #region IExpressionContext 成员

        public Value GetValue(string name)
        {
            if (name == null)
                return null;

            Value value;
            _Dict.TryGetValue(name.ToUpper(), out value);
            return value;
        }

        public bool IsDeclared(string name)
        {
            return _Dict.ContainsKey(name.ToUpper());
        }

        public void SetValue(string name, Value value)
        {
            if (name == null)
                return;

            name = name.ToUpper();
            if (value == null)
                _Dict.Remove(name);
            else
                _Dict[name] = value;
        }

        public void Declare(string name)
        {
            if (!IsDeclared(name))
                _Dict.Add(name.ToUpper(), Value.Void);
        }

        public Value Input(Value title, Value defValue, Value description)
        {
            return Value.Void;
        }

        public void Output(Value value)
        {
            
        }

        public Value ShowMsgBox(Value value, Value title, Value butons)
        {
            Output(value);
            return VbConstValues.vbOK;
        }

        public bool ExecuteFunction(string funcName, Value[] parameters, out Value returnValue)
        {
            returnValue = null;
            return false;
        }

        public FunctionDefineInfo GetFunctionInfo(string funcName)
        {
            return null;
        }

        public FunctionDefineInfo[] GetAllFunctionInfos()
        {
            return new FunctionDefineInfo[0];
        }

        #endregion
    }
}
