using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;
using Common.Script.VbScript.Common;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 类的成员
    /// </summary>
    abstract class ObjectMember
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name"></param>
        /// <param name="accessRight"></param>
        public ObjectMember(Class ownerClass, string name, ObjectMemberAccessRight accessRight)
        {
            OwnerClass = ownerClass;
            Name = name;
            AccessRight = accessRight;
        }

        public Class OwnerClass { get; private set; }

        /// <summary>
        /// 成员名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 访问权限
        /// </summary>
        public ObjectMemberAccessRight AccessRight { get; private set; }

        /// <summary>
        /// 调用该成员
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Value Execute(RunningContext context, Object obj, Expression[] parameters)
        {
            return OnExecute(context, obj, parameters);
        }

        /// <summary>
        /// 验证访问权限
        /// </summary>
        /// <param name="context"></param>
        protected void ValidateAccessRight(RunningContext context)
        {
            if (AccessRight == ObjectMemberAccessRight.Private)
            {
                if (!IsInnerCall(context))
                    throw new ScriptRuntimeException(string.Format("不具有对成员{0}的访问权限", Name));
            }
        }

        protected bool IsInnerCall(RunningContext context)
        {
            FunctionCallStackItem caller = context.CallStack.Caller;
            return caller != null && string.Compare(caller.Function.OwnerClassName, OwnerClass.Name, true) == 0;
        }

        /// <summary>
        /// 执行成员的调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected abstract Value OnExecute(RunningContext context, Object obj, Expression[] parameters);
    }
}
