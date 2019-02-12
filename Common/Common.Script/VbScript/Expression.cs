using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;
using Common.Script.VbScript.Expressions.ExpressionAnalysers;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 表达式的基类
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Expression()
        {
            
        }

        /// <summary>
        /// 执行表达式
        /// </summary>
        /// <param name="context">上下文执行环境</param>
        /// <returns></returns>
        protected abstract Value OnExecute(IExpressionContext context);

        /// <summary>
        /// 执行表达式
        /// </summary>
        /// <param name="context">上下文执行环境</param>
        public Value Execute(IExpressionContext context)
        {
            if (context == null)
                context = new DefaultExpressionContext();

            return OnExecute(context);
        }

        /// <summary>
        /// 执行表达式
        /// </summary>
        /// <returns></returns>
        public Value Execute()
        {
            return Execute(new DefaultExpressionContext());
        }

        /// <summary>
        /// 用于保存一些与该表达式相关的信息
        /// </summary>
        public object Tag { get; private set; }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// 根据字符串加载表达式
        /// </summary>
        /// <param name="expStr"></param>
        /// <param name="compileContext"></param>
        /// <returns></returns>
        public static Expression Parse(string expStr, IExpressionCompileContext compileContext)
        {
            if (expStr == null)
                throw new ArgumentNullException("expStr");

            Expression[] exps = ParseExpressions(expStr, compileContext ?? new DefaultExpressionCompileContext());
            if (exps.Length != 1)
                throw new ScriptException("表达式语法错误");

            return exps[0];
        }

        /// <summary>
        /// 根据字符串加载表达式
        /// </summary>
        /// <param name="expStr"></param>
        /// <returns></returns>
        public static Expression Parse(string expStr)
        {
            return Parse(expStr, new DefaultExpressionCompileContext());
        }

        /// <summary>
        /// 执行指定的字符串形式的表达式
        /// </summary>
        /// <param name="expStr"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Value Execute(string expStr, IExpressionContext context)
        {
            Expression exp = Expression.Parse(expStr);
            return exp.Execute(context);
        }

        /// <summary>
        /// 执行指定的字符串形式的表达式
        /// </summary>
        /// <param name="expStr"></param>
        /// <returns></returns>
        public static Value Execute(string expStr)
        {
            return Execute(expStr, null);
        }

        /// <summary>
        /// 将指定的字符串解析成一个或多个表达式
        /// </summary>
        /// <param name="expStr"></param>
        /// <param name="compileContext"></param>
        /// <returns></returns>
        internal static Expression[] ParseExpressions(string expStr, IExpressionCompileContext compileContext)
        {
            return new ExpressionAnalyser().Parse(expStr, compileContext);
        }

        /// <summary>
        /// 空的表达式
        /// </summary>
        public static readonly Expression Empty = new EmptyExpression();

        /// <summary>
        /// 获取所有的子表达式
        /// </summary>
        /// <returns></returns>
        public virtual Expression[] GetAllSubExpressions()
        {
            return new Expression[0];
        }

        /// <summary>
        /// 获取所有的变量名字
        /// </summary>
        /// <returns></returns>
        public string[] GetAllVariableNames()
        {
            Dictionary<string, string> variables = new Dictionary<string, string>();
            _GetAllVariableNames(variables);

            return new List<string>(variables.Values).ToArray();
        }

        private void _GetAllVariableNames(Dictionary<string, string> dict)
        {
            VariableExpression varExp = this as VariableExpression;
            if (varExp != null)
            {
                dict[varExp.Name.ToUpper()] = varExp.Name;
            }
            else
            {
                foreach (Expression exp in GetAllSubExpressions())
                {
                    exp._GetAllVariableNames(dict);
                }
            }
        }

        /// <summary>
        /// 获取表达式的值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        /// <param name="result"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Value GetValue(IExpressionContext context, string expression, out Value result, out ExpressionType type)
        {
            if (expression == null)
            {
                result = null;
                type = ExpressionType.Unknown;
                return false;
            }

            context = context ?? new DefaultExpressionContext();

            result = context.GetValue(expression);  // 作为变量
            if ((object)result != null)
            {
                type = ExpressionType.Variable;
                return true;
            }

            // 作为VB常量来解析
            result = VbConstValues.GetConstValue(expression);
            if ((object)result != null)
            {
                type = ExpressionType.VbConst;
                return true;
            }

            // 作为普通常量来解析
            if (expression.Length >= 2 && expression.StartsWith("\"") && expression.EndsWith("\""))  // 是否为字符串
            {
                result = expression.Substring(1, expression.Length - 2);
                type = ExpressionType.Const;
                return true;
            }

            int v;
            if (int.TryParse(expression, out v))  // 是否为整型
            {
                result = v;
                type = ExpressionType.Const;
                return true;
            }

            double v2;
            if (double.TryParse(expression, out v2))  // 是否为浮点型
            {
                result = v2;
                type = ExpressionType.Const;
                return true;
            }

            try  // 作为表达式来解析
            {
                result = Expression.Parse(expression).Execute(context);
                type = ExpressionType.Expression;
                return true;
            }
            catch
            {

            }

            result = null;
            type = ExpressionType.Unknown;
            return false;
        }

    }
}
