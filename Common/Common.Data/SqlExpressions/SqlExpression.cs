using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;
using Common.Data.SqlExpressions.Analyzer;
using System.Collections.Concurrent;
using Common.Utility;
using System.Text.RegularExpressions;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 查询表达式
    /// </summary>
    public abstract class SqlExpression : IEnumerable<SqlExpression>
    {
        /// <summary>
        /// 转换为字符串形式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract string ToString(ISqlExpressionContext context);

        /// <summary>
        /// 转换为字符串表达式形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// 从字符串形式的表达式转化
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static explicit operator SqlExpression(string exp)
        {
            if (string.IsNullOrEmpty(exp))
                return null;

            return SqlExpression.Parse(exp);
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static explicit operator string(SqlExpression exp)
        {
            if (exp == null)
                return null;

            return exp.ToString();
        }

        /// <summary>
        /// 获取子表达式
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<SqlExpression> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static IEnumerable<SqlExpression> ForEach(SqlExpression expression)
        {
            Contract.Requires(expression != null);

            foreach (SqlExpression exp in expression)
            {
                yield return exp;

                foreach (SqlExpression exp0 in ForEach(exp))
                {
                    yield return exp0;
                }
            }
        }

        /// <summary>
        /// 获取全部字段名
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] GetAllFieldNames(SqlExpression expression)
        {
            var list = from exp in ForEach(expression)
                       let f = exp as ISqlExpressionWithFieldName
                       where f != null
                       select f.GetFieldNames();

            return list.SelectMany(v => v).Distinct(IgnoreCaseEqualityComparer.Instance).ToArray();
        }

        /// <summary>
        /// 创建二元运算符
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static DualitySqlExpression Duality(SqlExpressionOperator @operator, SqlExpression exp1, SqlExpression exp2)
        {
            return new DualitySqlExpression(@operator, exp1, exp2);
        }

        /// <summary>
        /// 创建一元运算符
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static UnarySqlExpression Unary(SqlExpressionOperator @operator, SqlExpression exp)
        {
            return new UnarySqlExpression(@operator, exp);
        }

        /// <summary>
        /// 值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="colType"></param>
        /// <returns></returns>
        public static ValueExpression Value(object value, DbColumnType colType)
        {
            return new ValueExpression(value, colType);
        }

        /// <summary>
        /// 变量表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static VariableSqlExpression Variable(string varName)
        {
            return new VariableSqlExpression(varName);
        }

        /// <summary>
        /// 参数表达式
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static ParameterSqlExpression Parameter(string parameterName)
        {
            return new ParameterSqlExpression(parameterName);
        }

        /// <summary>
        /// 函数表达式
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static FunctionSqlExpression Function(string funcName, ArraySqlExpression parameters)
        {
            return new FunctionSqlExpression(funcName, parameters);
        }

        /// <summary>
        /// 函数表达式
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static FunctionSqlExpression Function(string funcName, SqlExpression[] parameters)
        {
            return Function(funcName, Array(parameters));
        }

        /// <summary>
        /// In表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DualitySqlExpression In(string varName, SqlExpression[] values)
        {
            return Duality(SqlExpressionOperator.In, Variable(varName), Array(values));
        }

        /// <summary>
        /// In表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="values"></param>
        /// <param name="colType"></param>
        /// <returns></returns>
        public static DualitySqlExpression In(string varName, object[] values, DbColumnType colType = DbColumnType.Unknown)
        {
            return In(varName, (SqlExpression[])values.ToArray(v => Value(v, colType)));
        }

        /// <summary>
        /// In表达式
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <param name="values">值</param>
        /// <param name="colType">字段类型</param>
        /// <returns>In表达式</returns>
        public static DualitySqlExpression In<T>(string varName, T[] values, DbColumnType colType = DbColumnType.Unknown)
        {
            return In(varName, (values == null) ? null : values.CastAsObject(), colType);
        }

        /// <summary>
        /// 将字符串转换为表达式
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SqlExpression Parse(string exp, ISqlExpressionContext context)
        {
            Contract.Requires(exp != null);

            _CheckCacheSize();

            SqlExpression sqlExp = _cache.GetOrAdd(exp, SqlExpressionAnalyzer.Analyze);
            if (context == null)
                return sqlExp;

            return new SqlExpressionWithContext(sqlExp, context);
        }

        /// <summary>
        /// 将字符串转换为表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static SqlExpression Parse(string exp)
        {
            return Parse(exp, (ISqlExpressionContext)null);
        }

        /// <summary>
        /// 将字符串转换为表达式，使用哈希表提供参数。表达式须为“@a>@b”形式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlExpression Parse(string exp, IDictionary<string, object> parameters)
        {
            if (parameters == null)
                return Parse(exp);

            return Parse(exp, new DictionarySqlExpressionContext(parameters));
        }

        /// <summary>
        /// 将字符串转换为表达式，使用数组提供参数。表达式须为“{0}>{1}”形式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlExpression Parse(string exp, params object[] parameters)
        {
            Contract.Requires(exp != null);

            exp = _regex.Replace(exp, delegate(Match m) {
                return "@" + _ARRAY_PARAMETER_PREFIX + int.Parse(m.Groups[1].Value);
            });

            return Parse(exp, new ArraySqlExpressionContext(parameters));
        }

        private static readonly Regex _regex = new Regex(@"\{(\d+)\}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static void _CheckCacheSize()
        {
            if (_cache.Count > MAX_CACHE_COUNT)
                _cache = new ConcurrentDictionary<string, SqlExpression>();
        }

        private const int MAX_CACHE_COUNT = 1000;
        private static ConcurrentDictionary<string, SqlExpression> _cache = new ConcurrentDictionary<string, SqlExpression>();

        #region Class SqlExpressionWithContext ...

        class SqlExpressionWithContext : SqlExpression
        {
            public SqlExpressionWithContext(SqlExpression sqlExp, ISqlExpressionContext defaultContext)
            {
                _sqlExp = sqlExp;
                _defaultContext = defaultContext;
            }

            private readonly SqlExpression _sqlExp;
            private readonly ISqlExpressionContext _defaultContext;

            public override string ToString(ISqlExpressionContext context)
            {
                if (context == null)
                    return ToString();

                return _sqlExp.ToString(new SqlExpressionContextConnection(new[] { context, _defaultContext }));
            }

            public override string ToString()
            {
                return _sqlExp.ToString(_defaultContext);
            }

            public override IEnumerator<SqlExpression> GetEnumerator()
            {
                return _sqlExp.GetEnumerator();
            }
        }

        #endregion

        #region Class DictionarySqlExpressionContext ...

        class DictionarySqlExpressionContext : ISqlExpressionContext
        {
            public DictionarySqlExpressionContext(IDictionary<string, object> parameter)
            {
                _parameter = parameter;
            }

            private readonly IDictionary<string, object> _parameter;

            public object GetValue(string varName)
            {
                if (varName == null)
                    return null;

                return _parameter.GetOrDefault(varName);
            }

            public string ToStringCallback(SqlExpression sqlExp)
            {
                return null;
            }
        }

        #endregion

        #region Class ArraySqlExpressionContext ...

        private const string _ARRAY_PARAMETER_PREFIX = "__AnonymousParameter_";

        class ArraySqlExpressionContext : ISqlExpressionContext
        {
            public ArraySqlExpressionContext(object[] args)
            {
                _args = args;
            }

            private readonly object[] _args;

            public object GetValue(string varName)
            {
                if (varName == null)
                    return null;

                if (!varName.StartsWith(_ARRAY_PARAMETER_PREFIX))
                    return null;

                int index;
                if (!int.TryParse(varName.Substring(_ARRAY_PARAMETER_PREFIX.Length), out index))
                    return null;

                if (index < 0 || index >= _args.Length)
                    throw new ArgumentOutOfRangeException("参数索引超出了数组界限");

                return _args[index];
            }

            string ISqlExpressionContext.ToStringCallback(SqlExpression sqlExp)
            {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 相等表达式
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static DualitySqlExpression Equals(SqlExpression exp1, SqlExpression exp2)
        {
            return new DualitySqlExpression(SqlExpressionOperator.Equals, exp1, exp2);
        }

        /// <summary>
        /// 相等表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DualitySqlExpression Equals(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return Equals(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// Is Null表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static DualitySqlExpression EqualsNull(string varName)
        {
            return Equals(varName, null, DbColumnType.DBNull);
        }

        /// <summary>
        /// 不相等表达式
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static DualitySqlExpression NotEquals(SqlExpression exp1, SqlExpression exp2)
        {
            return new DualitySqlExpression(SqlExpressionOperator.NotEquals, exp1, exp2);
        }

        /// <summary>
        /// 不相等表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DualitySqlExpression NotEquals(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return NotEquals(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// Is Not Null表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static DualitySqlExpression NotEqualsNull(string varName)
        {
            return NotEquals(varName, null, DbColumnType.DBNull);
        }

        /// <summary>
        /// And逻辑操作
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static DualitySqlExpression And(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.And, exp1, exp2);
        }

        /// <summary>
        /// And逻辑操作
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DualitySqlExpression And(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return And(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// Or逻辑操作
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static DualitySqlExpression Or(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.Or, exp1, exp2);
        }

        /// <summary>
        /// Or逻辑操作
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DualitySqlExpression Or(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return Or(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// Not逻辑操作
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static UnarySqlExpression Not(SqlExpression exp)
        {
            Contract.Requires(exp != null);

            return Unary(SqlExpressionOperator.Not, exp);
        }

        /// <summary>
        /// Not逻辑操作
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UnarySqlExpression Not(object value, DbColumnType type = DbColumnType.Unknown)
        {
            return Not(Value(value, type));
        }

        /// <summary>
        /// Like语句
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static SqlExpression Like(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.Like, exp1, exp2);
        }

        /// <summary>
        /// Like语句
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlExpression Like(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return Like(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// 大于表达式
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static SqlExpression Large(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.Large, exp1, exp2);
        }

        /// <summary>
        /// 大于表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlExpression Large(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return Large(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// 大于等于表达式
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static SqlExpression LargeEquals(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.LargeEquals, exp1, exp2);
        }

        /// <summary>
        /// 大于等于表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlExpression LargeEquals(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return LargeEquals(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// 小于表达式
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static SqlExpression Little(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.Little, exp1, exp2);
        }

        /// <summary>
        /// 小于表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlExpression Little(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return Little(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// 小于等于表达式
        /// </summary>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static SqlExpression LittleEquals(SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            return Duality(SqlExpressionOperator.LittleEquals, exp1, exp2);
        }

        /// <summary>
        /// 小于等于表达式
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlExpression LittleEquals(string varName, object value, DbColumnType type = DbColumnType.Unknown)
        {
            return LittleEquals(Variable(varName), Value(value, type));
        }

        /// <summary>
        /// 数组表达式
        /// </summary>
        /// <param name="exps"></param>
        /// <returns></returns>
        public static ArraySqlExpression Array(SqlExpression[] exps)
        {
            return new ArraySqlExpression(exps);
        }

        public static readonly SqlExpression Empty = new EmptySqlExpression();

        class EmptySqlExpression : SqlExpression
        {
            public override string ToString(ISqlExpressionContext context)
            {
                return "";
            }

            public override IEnumerator<SqlExpression> GetEnumerator()
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// 表达式解析环境
    /// </summary>
    public interface ISqlExpressionContext
    {
        /// <summary>
        /// 获取指定变量的值
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        object GetValue(string parameterName);

        /// <summary>
        /// 转换为字符串时的回调
        /// </summary>
        /// <param name="sqlExp"></param>
        string ToStringCallback(SqlExpression sqlExp);
    }

    /// <summary>
    /// 表达式解析环境的集合
    /// </summary>
    public class SqlExpressionContextConnection : ISqlExpressionContext
    {
        public SqlExpressionContextConnection(ISqlExpressionContext[] contexts)
        {
            _contexts = contexts;
        }

        private readonly ISqlExpressionContext[] _contexts;

        public object GetValue(string parameterName)
        {
            for (int k = 0; k < _contexts.Length; k++)
            {
                object value = _contexts[k].GetValue(parameterName);
                if (value != null)
                    return value;
            }

            return null;
        }

        public string ToStringCallback(SqlExpression sqlExp)
        {
            for (int k = 0; k < _contexts.Length; k++)
            {
                string s = _contexts[k].ToStringCallback(sqlExp);
                if (s != null)
                    return s;
            }

            return null;
        }
    }
}
