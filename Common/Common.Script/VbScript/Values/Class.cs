using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 类
    /// </summary>
    class Class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">类名</param>
        public Class(string name)
        {
            Name = name;
        }

        private readonly Dictionary<string, ObjectMember> _Dict = new Dictionary<string, ObjectMember>();

        /// <summary>
        /// 类名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 添加一个成员
        /// </summary>
        /// <param name="function"></param>
        public void AddMember(ObjectMember function)
        {
            _Dict.Add(function.Name.ToUpper(), function);
        }

        /// <summary>
        /// 是否包含指定的成员
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsMember(string name)
        {
            return _Dict.ContainsKey(name.ToUpper());
        }

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectMember GetMember(string name)
        {
            ObjectMember v;
            _Dict.TryGetValue(name.ToUpper(), out v);
            return v;
        }

        /// <summary>
        /// 获取所有成员
        /// </summary>
        /// <returns></returns>
        public ObjectMember[] GetMembers()
        {
            return new List<ObjectMember>(_Dict.Values).ToArray();
        }

        /// <summary>
        /// 从Class语句块中创建类
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static Class BuildFromClassStatemet(ClassStatement statement)
        {
            Class cls = new Class(statement.ClassName);

            foreach (FunctionClassMember function in statement.Functions)
            {
                cls.AddMember(new ObjectFunctionStatementMember(cls, function.Function.Name, function.AccessRight, function.Function));
            }

            foreach (VariableClassMember variable in statement.Variables)
            {
                cls.AddMember(new ObjectVariableMember(cls, variable.Expression.Name, variable.AccessRight, variable.Expression));
            }

            return cls;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        public virtual Object CreateObject(RunningContext context)
        {
            return new Object(context, this);
        }
    }
}
