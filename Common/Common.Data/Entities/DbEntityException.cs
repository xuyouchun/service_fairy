using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.Entities
{
    /// <summary>
    /// 表实体类的异常
    /// </summary>
    [Serializable]
    public class DbEntityException : DbException
    {
        public DbEntityException() { }
        public DbEntityException(string message) : base(message) { }
        public DbEntityException(string message, Exception inner) : base(message, inner) { }
        protected DbEntityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
