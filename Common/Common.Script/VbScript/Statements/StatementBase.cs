using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 语句块的基类
    /// </summary>
    abstract class StatementBase : Statement
    {
        internal override StatementType GetStatementType()
        {
            if (_StatementType == null)
            {
                object[] attrs = this.GetType().GetCustomAttributes(typeof(StatementAttribute), false);
                if (attrs.Length > 0)
                    _StatementType = ((StatementAttribute)attrs[0]).StatementType;
                else
                    throw new ScriptException("无法获取语句块“" + this.GetType().Name + "”的类型");
            }

            return (StatementType)_StatementType;
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            yield break;
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            yield break;
        }

        private StatementType? _StatementType;
    }
}
