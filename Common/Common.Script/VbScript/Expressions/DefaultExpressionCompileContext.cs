using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    public class DefaultExpressionCompileContext : IExpressionCompileContext
    {
        public DefaultExpressionCompileContext()
        {
            
        }

        #region IExpressionCompileContext 成员

        public Value GetConstValue(string name)
        {
            return null;
        }

        #endregion
    }
}
