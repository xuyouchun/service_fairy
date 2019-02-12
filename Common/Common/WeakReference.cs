using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common
{
    /// <summary>
    /// 强类型的弱引用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeakReference<T> : WeakReference where T : class
    {
        public WeakReference(T obj)
            : base(obj)
        {
            Contract.Requires(obj != null);

            _hashCode = obj.GetHashCode();
        }

        /// <summary>
        /// 对象
        /// </summary>
        public new T Target
        {
            get { return (T)base.Target; }
            set { base.Target = value; }
        }

        private readonly int _hashCode;

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(WeakReference<T>))
                return false;

            T tObj = ((WeakReference<T>)obj).Target;
            return object.Equals(obj, tObj);
        }

        public override string ToString()
        {
            T obj = Target;
            return obj == null ? string.Empty : obj.ToString();
        }
    }
}
