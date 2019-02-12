using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Package.Cache
{
    /// <summary>
    /// 缓存过期策略
    /// </summary>
    public interface ICacheExpireDependency
    {
        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        /// <summary>
        /// 访问通知
        /// </summary>
        void AccessNotify();

        /// <summary>
        /// 是否已经过期
        /// </summary>
        /// <returns></returns>
        bool HasExpired();
    }

    /// <summary>
    /// 缓存过期策略集合
    /// </summary>
    public static class CacheExpireDependency
    {
        public static ICacheExpireDependency And(params ICacheExpireDependency[] dependencies)
        {
            return new AndDependency(dependencies);
        }

        public static ICacheExpireDependency Or(params ICacheExpireDependency[] dependencies)
        {
            return new OrDependency(dependencies);
        }

        public static ICacheExpireDependency Proxy(Func<ICacheExpireDependency> creator)
        {
            Contract.Requires(creator != null);

            return new ProxyDependency(creator);
        }

        abstract class DependencyBase : ICacheExpireDependency
        {
            public DependencyBase(ICacheExpireDependency[] dependencies)
            {
                Dependencies = dependencies;
            }

            protected readonly ICacheExpireDependency[] Dependencies;

            public void Reset()
            {
                for (int k = 0; k < Dependencies.Length; k++)
                {
                    Dependencies[k].Reset();
                }
            }

            public void AccessNotify()
            {
                for (int k = 0; k < Dependencies.Length; k++)
                {
                    Dependencies[k].AccessNotify();
                }
            }

            public abstract bool HasExpired();
        }

        class AndDependency : DependencyBase
        {
            public AndDependency(ICacheExpireDependency[] dependencies)
                : base(dependencies)
            {
                
            }

            public override bool HasExpired()
            {
                for (int k = 0; k < Dependencies.Length; k++)
                {
                    if (!Dependencies[k].HasExpired())
                        return false;
                }

                return true;
            }
        }

        class OrDependency : DependencyBase
        {
            public OrDependency(ICacheExpireDependency[] dependencies)
                : base(dependencies)
            {
                
            }

            public override bool HasExpired()
            {
                for (int k = 0; k < Dependencies.Length; k++)
                {
                    if (Dependencies[k].HasExpired())
                        return true;
                }

                return false;
            }
        }

        class ProxyDependency : ICacheExpireDependency
        {
            public ProxyDependency(Func<ICacheExpireDependency> creator)
            {
                _dependency = new Lazy<ICacheExpireDependency>(creator, true);
            }

            private readonly Lazy<ICacheExpireDependency> _dependency;

            public void Reset()
            {
                _dependency.Value.Reset();
            }

            public void AccessNotify()
            {
                _dependency.Value.AccessNotify();
            }

            public bool HasExpired()
            {
                return _dependency.Value.HasExpired();
            }
        }
    }
}
