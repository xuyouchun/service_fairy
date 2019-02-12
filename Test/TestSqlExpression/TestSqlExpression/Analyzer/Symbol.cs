using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Data.UnionTable.SqlExpressions.Analyzer
{
    /// <summary>
    /// 符号
    /// </summary>
    class Symbol
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="symbolType"></param>
        /// <param name="op"></param>
        protected Symbol(string text, SymbolType symbolType)
        {
            SymbolType = symbolType;
            Text = text;
        }

        /// <summary>
        /// 符号名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 符号
        /// </summary>
        public SymbolType SymbolType { get; set; }

        public override int GetHashCode()
        {
            return Text.GetHashCode() ^ SymbolType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            Symbol obj2 = (Symbol)obj;
            return Text == obj2.Text && SymbolType == obj2.SymbolType;
        }

        public static bool operator ==(Symbol obj1, Symbol obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(Symbol obj1, Symbol obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// 左括号
        /// </summary>
        public static readonly Symbol LeftBricket = new Symbol("(", SymbolType.Bricket);

        /// <summary>
        /// 右括号
        /// </summary>
        public static readonly Symbol RightBricket = new Symbol(")", SymbolType.Bricket);
    }

    /// <summary>
    /// 运算符
    /// </summary>
    class OperatorSymbol : Symbol
    {
        public OperatorSymbol(string opName, SqlExpressionOperator op, int weight, int opCount)
            : base(opName, SymbolType.Operation)
        {
            Operator = op;
            Weight = weight;
            OpCount = opCount;
        }

        /// <summary>
        /// 运算符
        /// </summary>
        public SqlExpressionOperator Operator { get; private set; }

        /// <summary>
        /// 优先级（值越小越高）
        /// </summary>
        public int Weight { get; private set; }

        /// <summary>
        /// 操作符的个数
        /// </summary>
        public int OpCount { get; private set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && Operator == ((OperatorSymbol)obj).Operator;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Operator.GetHashCode();
        }

        public static bool operator ==(OperatorSymbol symbol1, OperatorSymbol symbol2)
        {
            return object.Equals(symbol1, symbol2);
        }

        public static bool operator !=(OperatorSymbol symbol1, OperatorSymbol symbol2)
        {
            return !object.Equals(symbol1, symbol2);
        }
    }

    /// <summary>
    /// 变量符号
    /// </summary>
    class VariableSymbol : Symbol
    {
        public VariableSymbol(string variable)
            : base(variable, SymbolType.Variable)
        {
            VariableName = variable;
        }

        /// <summary>
        /// 变量名
        /// </summary>
        public string VariableName { get; private set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(VariableSymbol symbol1, VariableSymbol symbol2)
        {
            return object.Equals(symbol1, symbol2);
        }

        public static bool operator !=(VariableSymbol symbol1, VariableSymbol symbol2)
        {
            return !object.Equals(symbol1, symbol2);
        }
    }

    /// <summary>
    /// 常量符号
    /// </summary>
    class ValueSymbol : Symbol
    {
        public ValueSymbol(object value, FieldType dataType)
            : base(value.ToStringIgnoreNull(), SymbolType.Value)
        {
            Value = value;
            DataType = dataType;
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public FieldType DataType { get; private set; }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ((Value == null) ? 0 : Value.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            ValueSymbol obj2;
            return base.Equals(obj) && SymbolType == (obj2 = (ValueSymbol)obj).SymbolType && object.Equals(Value, obj2.Value);
        }

        public static bool operator ==(ValueSymbol symbol1, ValueSymbol symbol2)
        {
            return object.Equals(symbol1, symbol2);
        }

        public static bool operator !=(ValueSymbol symbol1, ValueSymbol symbol2)
        {
            return !object.Equals(symbol1, symbol2);
        }
    }


    /// <summary>
    /// 符号的类型
    /// </summary>
    enum SymbolType
    {
        /// <summary>
        /// 运算符
        /// </summary>
        Operation,

        /// <summary>
        /// 括号
        /// </summary>
        Bricket,

        /// <summary>
        /// 变量名
        /// </summary>
        Variable,

        /// <summary>
        /// 值
        /// </summary>
        Value,
    }
}
