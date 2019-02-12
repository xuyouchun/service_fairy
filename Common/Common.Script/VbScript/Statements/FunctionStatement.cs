using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 函数
    /// </summary>
    [Statement(StatementType.Function)]
    class FunctionStatement : Statement, IStatementField
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">函数名</param>
        /// <param name="parameters">参数</param>
        /// <param name="body">函数体</param>
        /// <param name="ownerClassName">所属类名</param>
        public FunctionStatement(string name, VariableExpression[] parameters, Statement body, string ownerClassName)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
            OwnerClassName = ownerClassName;
        }

        /// <summary>
        /// 函数名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 所在的类名
        /// </summary>
        public string OwnerClassName { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        public VariableExpression[] Parameters { get; private set; }

        /// <summary>
        /// 函数体
        /// </summary>
        public Statement Body { get; private set; }

        /// <summary>
        /// 获取结果
        /// </summary>
        public virtual Value GetResult(RunningContext context)
        {
            return context.ExpressionContext.GetValue(Name);
        }

        protected override void OnExecute(RunningContext context)
        {
            context.ExpressionContext.Declare(Name);

            Body.Execute(context);
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Function;
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            yield return Body;
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            return Parameters;
        }

        public override string ToString()
        {
            return Name;
        }

        #region IStatementField 成员

        private LabelLibrary _LabelLibrary;

        public LabelLibrary GetLabelLibrary()
        {
            return _LabelLibrary ?? (_LabelLibrary = LabelLibrary.LoadFromStatement(this));
        }

        public bool OnErrorResumeNext { get; set; }

        #endregion
    }
}
