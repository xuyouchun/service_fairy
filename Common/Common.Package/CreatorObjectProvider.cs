using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Diagnostics.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 基于对象创建器的服务提供策略
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CreatorObjectProvider<T> : IObjectProvider<T>
    {
        public CreatorObjectProvider(Type type)
        {
            Contract.Requires(type != null);

            _creator = new Func<T>(() => (T)ObjectFactory.CreateObject(type));
        }

        public CreatorObjectProvider(Func<T> creator)
        {
            Contract.Requires(creator != null);

            _creator = creator;
        }

        private readonly Func<T> _creator;

        public T Get()
        {
            return _creator();
        }
    }
}
