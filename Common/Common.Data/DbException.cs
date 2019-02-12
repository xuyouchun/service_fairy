using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    /// <summary>
    /// 数据异常
    /// </summary>
    [Serializable]
    public class DbException : ApplicationException
    {
        public DbException() { }
        public DbException(string message) : base(message) { }
        public DbException(string message, Exception inner) : base(message, inner) { }
        protected DbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// 元数据异常
    /// </summary>
    [Serializable]
    public class DbMetadataException : DbException
    {
        public DbMetadataException() { }
        public DbMetadataException(string message) : base(message) { }
        public DbMetadataException(string message, Exception inner) : base(message, inner) { }
        protected DbMetadataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
