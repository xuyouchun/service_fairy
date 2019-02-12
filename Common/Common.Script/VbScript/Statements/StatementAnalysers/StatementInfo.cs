using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 语句块的属性信息
    /// </summary>
    class StatementInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="statementType"></param>
        /// <param name="creator"></param>
        public StatementInfo(string name, Type statementType, IStatementCreator creator)
        {
            Name = name;
            StatementType = statementType;
            Creator = creator;
        }

        /// <summary>
        /// 语句块的名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 语句块类的类型
        /// </summary>
        public Type StatementType { get; private set; }

        /// <summary>
        /// 创建器
        /// </summary>
        public IStatementCreator Creator { get; private set; }

        /// <summary>
        /// 创建语句块
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expressions"></param>
        /// <param name="statements"></param>
        /// <returns></returns>
        public Statement CreateStatement(Expression[] expressions, Statement[] statements)
        {
            return Creator.Create(expressions, statements);
        }
    }
}
