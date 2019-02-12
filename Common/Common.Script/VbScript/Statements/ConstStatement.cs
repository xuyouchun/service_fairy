using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.Const)]
    class ConstStatement : StatementBase
    {
        public ConstStatement(VariableDefineInfo[] items)
        {
            Items = items;
        }

        /// <summary>
        /// 
        /// </summary>
        public VariableDefineInfo[] Items { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected override void OnExecute(RunningContext context)
        {
            //foreach (var item in Items)
            //{
            //    context.ExpressionContext.Declare(item.Name);
            //    context.ExpressionContext.SetValue(item.Name, item.Variable.Execute(context.ExpressionContext));
            //}
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Const;
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

            return "CONST " + sb.ToString();
        }
    }
}
