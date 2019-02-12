using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values.Objects
{
    /// <summary>
    /// 内部预定义的类
    /// </summary>
    class InnerClass : Class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="objType"></param>
        public InnerClass(string name, Type objType)
            : base(name)
        {
            ObjectType = objType;
        }

        public Type ObjectType { get; private set; }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Object CreateObject(RunningContext context)
        {
            return new Object(context, this, Activator.CreateInstance(ObjectType));
        }
    }
}
