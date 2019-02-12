using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.Class)]
    class ClassStatement : StatementBase
    {
        public ClassStatement(string className, VariableClassMember[] variables, FunctionClassMember[] functions)
        {
            ClassName = className;
            Variables = variables;
            Functions = functions;
        }

        public string ClassName { get; private set; }

        /// <summary>
        /// 变量的定义
        /// </summary>
        public VariableClassMember[] Variables { get; private set; }

        /// <summary>
        /// 函数的定义
        /// </summary>
        public FunctionClassMember[] Functions { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Class;
        }

        public override string ToString()
        {
            return "Class " + ClassName;
        }
    }

    class VariableClassMember
    {
        public VariableClassMember(VariableExpression expression, ObjectMemberAccessRight accessRight)
        {
            Expression = expression;
            AccessRight = accessRight;
        }

        public ObjectMemberAccessRight AccessRight { get; private set; }

        public VariableExpression Expression { get; private set; }
    }

    class FunctionClassMember
    {
        public FunctionClassMember(FunctionStatement function, ObjectMemberAccessRight accessRight)
        {
            Function = function;
            AccessRight = accessRight;
        }

        public ObjectMemberAccessRight AccessRight { get; private set; }

        public FunctionStatement Function { get; private set; }
    }
}
