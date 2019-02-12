using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    class LocalExpressionContext : IExpressionContext
    {
        public LocalExpressionContext(IExpressionContext context, IExpressionContext globalContext)
        {
            _Context = context;
            _GlobalContext = globalContext;
        }

        readonly IExpressionContext _Context, _GlobalContext;

        readonly Dictionary<string, Value> _Dict = new Dictionary<string, Value>();

        #region IExpressionContext 成员

        public Value GetValue(string name)
        {
            if (name == null)
                return null;

            Value value;
            if (_Dict.TryGetValue(name.ToUpper(), out value))
                return value;

            return _Context.GetValue(name) ?? _GlobalContext.GetValue(name);
        }

        public bool IsDeclared(string name)
        {
            return _Dict.ContainsKey(name.ToUpper());
        }

        public void SetValue(string name, Value value)
        {
            name = name.ToUpper();
            if (IsDeclared(name))
            {
                if (value == null)
                    _Dict.Remove(name);
                else
                    _Dict[name] = value;
            }
            else if (_GlobalContext.IsDeclared(name))
            {
                _GlobalContext.SetValue(name, value);
            }
            else
            {
                _Context.SetValue(name, value);
            }
        }

        public void Declare(string name)
        {
            if (!IsDeclared(name))
                _Dict.Add(name.ToUpper(), Value.Void);
        }

        public Value Input(Value title, Value defValue, Value description)
        {
            return _Context.Input(title, defValue, description);
        }

        public void Output(Value value)
        {
            _Context.Output(value);
        }

        public Value ShowMsgBox(Value value, Value title, Value butons)
        {
            Output(value);
            return VbConstValues.vbOK;
        }

        public bool ExecuteFunction(string funcName, Value[] parameters, out Value returnValue)
        {
            return _Context.ExecuteFunction(funcName, parameters, out returnValue);
        }

        public FunctionDefineInfo GetFunctionInfo(string funcName)
        {
            return _Context.GetFunctionInfo(funcName);
        }

        public FunctionDefineInfo[] GetAllFunctionInfos()
        {
            return _Context.GetAllFunctionInfos();
        }

        #endregion
    }
}
