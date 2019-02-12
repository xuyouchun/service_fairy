using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;
using Common.Script.VbScript.Values;
using VbValue = global::Common.Script.VbScript.Value;
using VbObject = global::Common.Script.VbScript.Values.Object;

namespace Common.Script.VbScript.Common
{
    /// <summary>
    /// 函数调用堆栈
    /// </summary>
    class FunctionCallStack
    {
        public FunctionCallStack(RunningContext context)
        {
            _Context = context;
        }

        private readonly RunningContext _Context;

        private readonly List<FunctionCallStackItem> _List = new List<FunctionCallStackItem>();

        public void Push(FunctionCallStackItem function)
        {
            _List.Add(function);
        }

        public void Push(FunctionStatement function, VbObject obj)
        {
            Push(new FunctionCallStackItem(function, obj));
        }

        public FunctionCallStackItem Pop()
        {
            if (_List.Count == 0)
                return null;

            FunctionCallStackItem statement = _List[_List.Count - 1];
            _List.RemoveAt(_List.Count - 1);
            return statement;
        }

        /// <summary>
        /// 当前函数
        /// </summary>
        public FunctionCallStackItem Current
        {
            get
            {
                if (_List.Count == 0)
                    return null;

                return _List[_List.Count - 1];
            }
        }

        /// <summary>
        /// 调用者
        /// </summary>
        public FunctionCallStackItem Caller
        {
            get
            {
                if (_List.Count < 2)
                    return null;

                return _List[_List.Count - 2];
            }
        }
    }

    class FunctionCallStackItem
    {
        public FunctionCallStackItem(FunctionStatement function, VbObject obj)
        {
            Function = function;
            Object = obj;
        }

        public FunctionStatement Function { get; private set; }

        public VbObject Object { get; private set; }
    }
}
