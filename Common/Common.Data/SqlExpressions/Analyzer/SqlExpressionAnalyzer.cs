using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.SqlExpressions.Analyzer
{
    /// <summary>
    /// sql表达式的分析器
    /// </summary>
    unsafe class SqlExpressionAnalyzer
    {
        private SqlExpressionAnalyzer(char *p)
        {
            _p = p;
        }

        private char* _p;

        public SqlExpression Execute()
        {
            return AnalyzerStack.Analyze(new SymbolEnumerable(_p));
        }

        unsafe class SymbolEnumerable : IEnumerable<Symbol>
        {
            public SymbolEnumerable(char *p)
            {
                _p = p;
            }

            private char* _p;

            public IEnumerator<Symbol> GetEnumerator()
            {
                return new SymbolEnumerator(_p);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #region Class SymbolEnumerator ...

            class SymbolEnumerator : IEnumerator<Symbol>
            {
                public SymbolEnumerator(char* p)
                {
                    _p0 = p;
                    _p = p;
                }

                private char* _p, _p0;
                private Symbol _symbol;

                public Symbol Current
                {
                    get
                    {
                        return _symbol;
                    }
                }

                public void Dispose()
                {
                    
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    _symbol = SymbolReader.ReadNextSymbol(ref _p);
                    return _symbol != null;
                }

                public void Reset()
                {
                    _p = _p0;
                }
            }

            #endregion

        }

        public static SqlExpression Analyze(string exp)
        {
            fixed (char* p = exp)
            {
                SqlExpressionAnalyzer analyzer = new SqlExpressionAnalyzer(p);
                return analyzer.Execute();
            }
        }
    }
}
