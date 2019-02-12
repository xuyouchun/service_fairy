using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 数组表达式
    /// </summary>
    class ArrayExpression : VariableExpression
    {
        public ArrayExpression(string arrrayName, Expression[] expressions)
            : base(arrrayName)
        {
            Expressions = expressions;
        }

        public Expression[] Expressions { get; private set; }

        protected override Value OnExecute(IExpressionContext context)
        {
            ArrayObject array = _GetArray(context);

            return array.GetValue(_GetIndexes(context));
        }

        private ArrayObject _GetArray(IExpressionContext context)
        {
            ArrayObject array = base.OnExecute(context) as ArrayObject;
            if (array == null)
                throw new ScriptRuntimeException(string.Format("变量{0}并非数组类型", Name));
            return array;
        }

        public override void SetValue(IExpressionContext context, Value value)
        {
            ArrayObject array = _GetArray(context);
            array.SetValue(_GetIndexes(context), value);
        }

        private int[] _GetIndexes(IExpressionContext context)
        {
            int[] indexes = new int[Expressions.Length];
            for (int k = 0; k < indexes.Length; k++)
            {
                Value v = Expressions[k].Execute(context);
                try
                {
                    indexes[k] = v.GetValue<int>();
                }
                catch (InvalidCastException)
                {
                    throw new ScriptException(string.Format("访问数组时，无法将{0}转换为整型", Expressions[k]));
                }
            }

            return indexes;
        }

        public override void Declare(IExpressionContext context)
        {
            context.Declare(Name);
            context.SetValue(Name, new ArrayObject(_GetIndexes(context)));
        }

        public override string ToString()
        {
            return "Array";
        }
    }
}
