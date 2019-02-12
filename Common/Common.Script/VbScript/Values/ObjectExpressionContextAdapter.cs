using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    class ObjectExpressionContextAdapter : IExpressionContext
    {
        public ObjectExpressionContextAdapter(IExpressionContext runningContext, IExpressionContext objContext)
        {
            RunningContext = runningContext;
            ObjContext = objContext;
        }

        public IExpressionContext RunningContext { get; private set; }

        public IExpressionContext ObjContext { get; private set; }

        #region IExpressionContext 成员

        public Value GetValue(string name)
        {
            return ObjContext.GetValue(name);
        }

        public bool IsDeclared(string name)
        {
            return ObjContext.IsDeclared(name);
        }

        public void SetValue(string name, Value value)
        {
            ObjContext.SetValue(name, value);
        }

        public void Declare(string name)
        {
            ObjContext.Declare(name);
        }

        public Value Input(Value title, Value defValue, Value description)
        {
            return RunningContext.Input(title, defValue, description);
        }

        public void Output(Value value)
        {
            RunningContext.Output(value);
        }

        public Value ShowMsgBox(Value value, Value title, Value butons)
        {
            Output(value);
            return VbConstValues.vbOK;
        }

        public bool ExecuteFunction(string funcName, Value[] parameters, out Value returnValue)
        {
            return RunningContext.ExecuteFunction(funcName, parameters, out returnValue);
        }

        public FunctionDefineInfo GetFunctionInfo(string funcName)
        {
            return RunningContext.GetFunctionInfo(funcName);
        }

        public FunctionDefineInfo[] GetAllFunctionInfos()
        {
            return RunningContext.GetAllFunctionInfos();
        }

        #endregion
    }
}
