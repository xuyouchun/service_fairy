using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements.StatementAnalysers;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 语句加载器
    /// </summary>
    unsafe class StatementAnalyser
    {
        public StatementAnalyser(string code, IStatementCompileContext context)
        {
            _Code = code;
            _Context = context;
        }

        private readonly string _Code;
        private readonly IStatementCompileContext _Context;

        /// <summary>
        /// 执行加载
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Statement Parse()
        {
            fixed (char* ptr = _Code)
            {
                return new StatementAnalyserExecutor(ptr, _Context).Analyse();
            }
        }
    }
}
