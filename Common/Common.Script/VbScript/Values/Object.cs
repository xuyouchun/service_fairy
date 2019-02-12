using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 对象
    /// </summary>
    class Object : Value
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ownerClass"></param>
        public Object(RunningContext context, Class ownerClass, object innerValue)
            : base(innerValue)
        {
            OwnerClass = ownerClass;
            InnerValue = innerValue ?? this;

            _Init(context);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ownerClass"></param>
        public Object(RunningContext context, Class ownerClass)
            : this(context, ownerClass, null)
        {

        }

        private void _Init(RunningContext context)
        {
            ExpressionContext = new ObjectExpressionContext();
            IExpressionContext oldCtx = context.ExpressionContext;

            try
            {
                context.ExpressionContext = ExpressionContext;

                foreach (ObjectMember variable in OwnerClass.GetMembers())
                {
                    ObjectVariableMember variableMember = variable as ObjectVariableMember;
                    if (variableMember != null)
                        variableMember.VariableExpression.Declare(context.ExpressionContext);
                }
            }
            finally
            {
                context.ExpressionContext = oldCtx;
            }
        }

        public ObjectExpressionContext ExpressionContext { get; private set; }

        /// <summary>
        /// 类信息
        /// </summary>
        public Class OwnerClass { get; private set; }

        public override string ToString()
        {
            return "Object";
        }
    }
}
