using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common.Script.VbScript.Values.Objects
{
    static class ClassManager
    {
        static ClassManager()
        {
            foreach (Type type in typeof(ClassManager).Assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(ClassAttribute), false);
                if (attrs.Length > 0)
                {
                    ClassAttribute attr = (ClassAttribute)attrs[0];
                    string name = attr.Name ?? type.Name;

                    _ClassDict.Add(name.ToUpper(), _CreateClass(name, type));
                }
            }
        }

        static readonly Dictionary<string, Class> _ClassDict = new Dictionary<string, Class>();

        private static Class _CreateClass(string name, Type type)
        {
            InnerClass classInfo = new InnerClass(name, type);

            foreach (MethodInfo mInfo in type.GetMethods())
            {
                object[] attrs = mInfo.GetCustomAttributes(typeof(MethodAttribute), false);
                if (attrs.Length > 0)
                {
                    MethodAttribute methodAttr = (MethodAttribute)attrs[0];
                    string methodName = methodAttr.Name ?? mInfo.Name;

                    classInfo.AddMember(new InnerObjectFunctionMember(classInfo, methodName, mInfo));
                }
            }

            return classInfo;
        }

        /// <summary>
        /// 根据名字获取类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Class GetClass(string name)
        {
            Class cls;
            _ClassDict.TryGetValue(name.ToUpper(), out cls);
            return cls;
        }
    }
}
