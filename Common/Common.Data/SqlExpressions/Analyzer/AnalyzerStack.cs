using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Data.SqlExpressions.Analyzer
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

        private readonly Stack<SqlExpression> _expStack = new Stack<SqlExpression>();  // 保存表达式
        private readonly Stack<OperatorSymbol> _opSymbolStack = new Stack<OperatorSymbol>(); // 保存运算符
        private readonly List<SqlExpression> _parsedList = new List<SqlExpression>();  // 保存已经解析完毕的表达式
        private bool _metComma = false;   // 记录是否遇到逗号，格式要求逗号之后必须有另一个表达式

        // 将堆栈中的表达式合并，确保最后合并的结果只有一个表达式
        private SqlExpression _AnalyzeOne()
        {
            SqlExpression[] exps = _AnalyzeAll();
            if (exps.Length != 1)
                throw _CreateFormatException();

            return exps[0];
        }

        // 将堆栈中的所有表达式合并，并返回所有合并的结果
        private SqlExpression[] _AnalyzeAll()
        {
            while (_enumerator.MoveNext())
            {
                if (!_Push(_enumerator.Current))
                    break;
            }

            _CombineAndPop();
            return _parsedList.ToArray();
        }

        // 将堆栈中的所有表达式合并，并放入分析结果列表
        private void _CombineAndPop()
        {
            if (_StackIsEmpty())
            {
                if (_metComma)
                    throw _CreateFormatException();
            }
            else
            {
                _CombineExpressions();
                if (_opSymbolStack.Count != 0 || _expStack.Count != 1)
                    throw _CreateFormatException();

                _parsedList.Add(_expStack.Pop());
                _metComma = false;
            }
        }

        // 判断堆栈是否为空
        private bool _StackIsEmpty()
        {
            return _opSymbolStack.Count == 0 && _expStack.Count == 0;
        }

        // 将符号入栈
        private bool _Push(Symbol symbol)
        {
            switch (symbol.SymbolType)
            {
                case SymbolType.Bricket:
                    return _PushBricket(symbol);

                case SymbolType.Comma:
                    return _PushComma(symbol);

                case SymbolType.Value:
                    return _PushValue((ValueSymbol)symbol);

                case SymbolType.Variable:
                    return _PushVariable((VariableSymbol)symbol);

                case SymbolType.Function:
                    return _PushFunction((FunctionSymbol)symbol);

                case SymbolType.Parameter:
                    return _PushParameter((ParameterSymbol)symbol);

                case SymbolType.Operation:
                    return _PushOperation((OperatorSymbol)symbol);
            }

            throw _CreateFormatException();
        }

        // 左括号或右括号
        private bool _PushBricket(Symbol symbol)
        {
            if (symbol.Text == "(")
            {
                AnalyzerStack stack = new AnalyzerStack(_enumerator);
                SqlExpressionOperator lastOp = _opSymbolStack.Count == 0 ? SqlExpressionOperator.Unknown : _opSymbolStack.Peek().Operator;
                _expStack.Push(lastOp == SqlExpressionOperator.In ? SqlExpression.Array(stack._AnalyzeAll()) : stack._AnalyzeOne());
                return true;
            }
            else
            {
                return false;
            }
        }

        // 逗号
        private bool _PushComma(Symbol symbol)
        {
            _CombineAndPop();
            _metComma = true;
            return true;
        }

        // 常量
        private bool _PushValue(ValueSymbol symbol)
        {
            _expStack.Push(SqlExpression.Value(symbol.Value, symbol.DataType));
            return true;
        }

        // 变量
        private bool _PushVariable(VariableSymbol symbol)
        {
            _expStack.Push(SqlExpression.Variable(symbol.VariableName));
            return true;
        }

        // 函数
        private bool _PushFunction(FunctionSymbol symbol)
        {
            AnalyzerStack stack = new AnalyzerStack(_enumerator);
            _expStack.Push(SqlExpression.Function(symbol.FunctionName, stack._AnalyzeAll()));
            return true;
        }

        // 参数
        private bool _PushParameter(ParameterSymbol symbol)
        {
            _expStack.Push(SqlExpression.Parameter(symbol.ParameterName));
            return true;
        }

        // 运算符
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

        // 将指定优先级的表达式合并
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
                _expStack.Push(_CreateUnarySqlExpression(op.Operator, exp));
                return _CombineEqualityPriority(curWeight);
            }
            else
            {
                return _CreateDualitySqlExpression(op.Operator, _CombineEqualityPriority(curWeight), exp);
            }
        }

        // 创建一元运算符表达式
        private static UnarySqlExpression _CreateUnarySqlExpression(SqlExpressionOperator op, SqlExpression exp)
        {
            return SqlExpression.Unary(op, exp);
        }

        // 创建二元运算符表达式
        private static DualitySqlExpression _CreateDualitySqlExpression(SqlExpressionOperator op, SqlExpression exp1, SqlExpression exp2)
        {
            return SqlExpression.Duality(op, exp1, exp2);
        }

        // 创建格式错误异常
        private static Exception _CreateFormatException()
        {
            return new FormatException("表达式格式错误");
        }

        // 开始分析
        public static SqlExpression Analyze(IEnumerable<Symbol> symbols)
        {
            using (IEnumerator<Symbol> enumerator = symbols.GetEnumerator())
            {
                AnalyzerStack stack = new AnalyzerStack(enumerator);
                return stack._AnalyzeOne();
            }
        }
    }
}
