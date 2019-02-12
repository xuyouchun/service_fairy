using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common.Script.VbScript.Values.Objects
{
    class InnerObjectFunctionMember : ObjectFunctionMember
    {
        public InnerObjectFunctionMember(Class owner, string name, MethodInfo mInfo)
            : base(owner, name, ObjectMemberAccessRight.Public)
        {
            MethodInfo = mInfo;
        }

        public MethodInfo MethodInfo { get; private set; }

        protected override Value OnExecute(RunningContext context, Object obj, Expression[] parameters)
        {
            object result = MethodInfo.Invoke(obj.InnerValue, _GetParameterValues(context.ExpressionContext, parameters, MethodInfo.GetParameters()));
            Value v = result as Value;
            return v != null ? v : new Value(result);
        }

        private object[] _GetParameterValues(IExpressionContext context, Expression[] parameters, ParameterInfo[] pInfos)
        {
            object[] objs = new object[pInfos.Length];
            for (int k = 0; k < objs.Length; k++)
            {
                objs[k] = k < parameters.Length ? _Convert(parameters[k].Execute(context), pInfos[k].ParameterType) : pInfos[k].DefaultValue;
            }

            return objs;
        }

        private object _Convert(Value value, Type type)
        {
            try
            {
                return Convert.ChangeType(value.InnerValue, type);
            }
            catch (Exception ex)
            {
                throw new ScriptRuntimeException(string.Format("参数\"{0}\"无法转换为\"{1}\"", value, type.Name), ex);
            }
        }
    }
}
