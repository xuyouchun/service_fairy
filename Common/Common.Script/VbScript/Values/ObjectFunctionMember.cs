using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    abstract class ObjectFunctionMember : ObjectMember
    {
        public ObjectFunctionMember(Class owner, string name, ObjectMemberAccessRight accessRight)
            : base(owner, name, accessRight)
        {

        }
    }
}
