using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.Dim)]
    class DimStatement : StatementBase
    {
        public DimStatement(VariableDefineInfo[] items)
        {
            Items = items;
        }

        public VariableDefineInfo[] Items { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            foreach (var item in Items)
            {
                item.Variable.Declare(context.ExpressionContext);
                if (item.Value != null)
                    item.Variable.SetValue(context.ExpressionContext, item.Value.Execute(context.ExpressionContext));
            }
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            foreach (var item in Items)
            {
                yield return item.Variable;
                if (item.Value != null)
                    yield return item.Value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Items)
            {
                if (sb.Length > 0)
                    sb.Append(", ");

                sb.Append(item.ToString());
            }

            return "DIM " + sb.ToString();
        }
    }
}
