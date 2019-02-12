using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Collection;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package
{
    /// <summary>
    /// 自动释放的弱引用哈希表
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class AutoReleaseWeakReferenceDictionary<TKey, TValue> : WeakReferenceDictionary<TKey, TValue>, IDisposable, IAutoReleaseWeakReferenceDictionary
        where TValue : class
    {
        public AutoReleaseWeakReferenceDictionary(object syncLocker)
        {
            _syncLocker = syncLocker ?? this;
            this.Register();
        }

        public AutoReleaseWeakReferenceDictionary(RwLocker locker)
            : this((object)locker)
        {

        }

        public AutoReleaseWeakReferenceDictionary()
            : this(null)
        {
            
        }

        private object _syncLocker;

        public object SyncLocker
        {
            get { return _syncLocker; }
            set { _syncLocker = value; }
        }

        void IAutoReleaseWeakReferenceDictionary.CheckExpire()
        {
            RwLocker locker = _syncLocker as RwLocker;
            if (locker != null)
            {
                using (locker.Write())
                {
                    this.RemoveCollected();
                }
            }
            else
            {
                lock (_syncLocker)
                {
                    this.RemoveCollected();
                }
            }
        }

        ~AutoReleaseWeakReferenceDictionary()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Unregister();
        }
    }

    interface IAutoReleaseWeakReferenceDictionary
    {
        void CheckExpire();
    }

    static class AutoReleaseWearReferenceDictionaryManager
    {
        private static readonly ObjectManager<IAutoReleaseWeakReferenceDictionary> _objMgr
            = new ObjectManager<IAutoReleaseWeakReferenceDictionary>(TimeSpan.FromSeconds(10), _CheckFunc);

        public static void Register(this IAutoReleaseWeakReferenceDictionary dict)
        {
            _objMgr.Register(dict);
        }

        public static void Unregister(this IAutoReleaseWeakReferenceDictionary dict)
        {
            _objMgr.Unregister(dict);
        }

        private static void _CheckFunc(IAutoReleaseWeakReferenceDictionary obj)
        {
            obj.CheckExpire();
        }
    }
}
