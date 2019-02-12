using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Ats.Client.Script.Values.Objects
{
    static class ObjectManager
    {
        static ObjectManager()
        {
            foreach (Type type in typeof(ObjectManager).Assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(ObjectAttribute), false);
                if (attrs.Length > 0)
                {
                    ObjectAttribute attr = (ObjectAttribute)attrs[0];
                    string name = attr.Name ?? type.Name;

                    _ClassDict.Add(name.ToUpper(), _CreateClass(type, name));
                }
            }
        }

        static readonly Dictionary<string, Class> _ClassDict = new Dictionary<string, Class>();

        private static Class _CreateClass(Type type, string name)
        {
            Class classInfo = new Class(name);

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
    }
}
