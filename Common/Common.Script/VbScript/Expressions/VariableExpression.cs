using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 用于表示变量的表达式
    /// </summary>
    class VariableExpression : Expression, ILeftValueExpression
    {
        public VariableExpression(string valueName)
        {
            Name = valueName;
        }

        /// <summary>
        /// 变量的名称
        /// </summary>
        public string Name { get; private set; }

        protected override Value OnExecute(IExpressionContext context)
        {
            return GetValue(context);
        }

        public virtual Value GetValue(IExpressionContext context)
        {
            Value value = context.GetValue(Name);
            if (value != null)
            {
                if (RunningContext.Current != null)
                    RunningContext.Current.LastValueCacheManager.AddValue(Name, value);

                return value;
            }

            if (RunningContext.Current == null || RunningContext.Current.IsOptionExplicit)
                throw new ScriptRuntimeException("未声明变量“" + Name + "”");

            return Value.Void;
        }

        public virtual void SetValue(IExpressionContext context, Value value)
        {
            if ((RunningContext.Current == null || RunningContext.Current.IsOptionExplicit) && context.GetValue(Name) == null)
                throw new ScriptRuntimeException("未声明变量“" + Name + "”");

            context.SetValue(Name, value);
        }

        /// <summary>
        /// 声明
        /// </summary>
        /// <param name="context"></param>
        public virtual void Declare(IExpressionContext context)
        {
            context.Declare(Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual Expression[] GetParameters()
        {
            return new Expression[0];
        }
    }
}
