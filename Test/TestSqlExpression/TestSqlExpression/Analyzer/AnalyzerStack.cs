using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable.SqlExpressions.Analyzer
{
    /// <summary>
    /// SQL表达式的分析栈
    /// </summary>
    class AnalyzerStack
    {
        private AnalyzerStack(IEnumerator<Symbol> enumerator)
        {
            _enumerator = enumerator;
        }

        private readonly IEnumerator<Symbol> _enumerator;

        private readonly Stack<SqlExpression> _expStack = new Stack<SqlExpression>();
        private readonly Stack<OperatorSymbol> _opSymbolStack = new Stack<OperatorSymbol>();

        private SqlExpression _Analyze()
        {
            while (_enumerator.MoveNext())
            {
                if (!_Push(_enumerator.Current))
                    break;
            }

            _CombineExpressions();
            if (_expStack.Count != 1 || _opSymbolStack.Count != 0)
                throw _CreateFormatException();

            return _expStack.Pop();
        }

        private bool _Push(Symbol symbol)
        {
            switch (symbol.SymbolType)
            {
                case SymbolType.Bricket:
                    return _PushBricket(symbol);

                case SymbolType.Value:
                    return _PushValue((ValueSymbol)symbol);

                case SymbolType.Variable:
                    return _PushVariable((VariableSymbol)symbol);

                case SymbolType.Operation:
                    return _PushOperation((OperatorSymbol)symbol);
            }

            throw _CreateFormatException();
        }

        private bool _PushBricket(Symbol symbol)
        {
            if (symbol.Text == "(")
            {
                AnalyzerStack stack = new AnalyzerStack(_enumerator);
                _expStack.Push(stack._Analyze());
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool _PushValue(ValueSymbol symbol)
        {
            _expStack.Push(SqlExpression.Value(symbol.Value, symbol.DataType));
            return true;
        }

        private bool _PushVariable(VariableSymbol symbol)
        {
            _expStack.Push(SqlExpression.Variable(symbol.VariableName));
            return true;
        }

        private bool _PushOperation(OperatorSymbol symbol)
        {
            if (_opSymbolStack.Count > 0 && _opSymbolStack.Peek().Weight < symbol.Weight)  // 如果上一个操作符优先级要高，则合并前面的操作符
            {
                _CombineExpressions(symbol.Weight);
            }

            if (symbol.OpCount == 1)  // 一元运算符
            {
                _opSymbolStack.Push(symbol);
            }
            else  // 二元运算符
            {
                if (_expStack.Count == 0)  // 如果栈为空，则格式错误
                    throw _CreateFormatException();

                _opSymbolStack.Push(symbol);
            }

            return true;
        }

        private void _CombineExpressions(int weight = int.MaxValue)
        {
            int curWeight;
            while (_opSymbolStack.Count > 0 && (curWeight = _opSymbolStack.Peek().Weight) < weight)
            {
                _expStack.Push(_CombineEqualityPriority(curWeight));
            }
        }

        // 提取出相同优先级的表达式
        private SqlExpression _CombineEqualityPriority(int curWeight)
        {
            if (_expStack.Count == 0)
                throw _CreateFormatException();

            SqlExpression exp = _expStack.Pop();

            if (_opSymbolStack.Count == 0 || curWeight != _opSymbolStack.Peek().Weight)
                return exp;

            OperatorSymbol op = _opSymbolStack.Pop();
            curWeight = op.Weight;
            if (op.OpCount == 1)
            {
                _expStack.Push(SqlExpression.Unary(op.Operator, exp));
                return _CombineEqualityPriority(curWeight);
            }
            else
            {
                return SqlExpression.Duality(op.Operator, _CombineEqualityPriority(curWeight), exp);
            }
        }

        private static Exception _CreateFormatException()
        {
            return new FormatException("表达式格式错误");
        }

        public static SqlExpression Analyze(IEnumerable<Symbol> symbols)
        {
            using (IEnumerator<Symbol> enumerator = symbols.GetEnumerator())
            {
                AnalyzerStack stack = new AnalyzerStack(enumerator);
                return stack._Analyze();
            }
        }
    }
}
