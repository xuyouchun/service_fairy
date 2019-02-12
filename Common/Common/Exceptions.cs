using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 用户取消操作
    /// </summary>
    [Serializable]
    public class UserCancelException : ApplicationException
    {
        public UserCancelException()
            : base("操作已取消")
        {

        }

        public UserCancelException(string message)
            : base(message)
        {

        }

        public UserCancelException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
