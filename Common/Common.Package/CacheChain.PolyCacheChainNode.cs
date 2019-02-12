using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Utility;

namespace Common.Package
{
    partial class CacheChain<TKey, TValue>
    {
        /// <summary>
        /// 聚合器
        /// </summary>
        class PolyCacheChainNode : ICacheChainNode<TKey, TValue>, IDisposable
        {
            public PolyCacheChainNode(ThreadPriority priority)
            {
                _dispatchThread = ThreadUtility.StartNew(_DispatchFunc, priority);
            }

            private ICacheChainNode<TKey, TValue> _nextNode = Empty;
            private readonly Thread _dispatchThread;
            private readonly ManualResetEvent _waitForExit = new ManualResetEvent(false);
            private readonly AutoResetEvent _waitForTask = new AutoResetEvent(false);
            private readonly object _syncLocker = new object();
            private GetCtx _getCtx = new GetCtx(), _refreshGetCtx = new GetCtx();
            private RemoveCtx _removeCtx = new RemoveCtx();
            private AddCtx _addCtx = new AddCtx();

            #region Class CtxBase ...

            abstract class CtxBase
            {
                public abstract int GetCount();

                public virtual void Wait()
                {
                    if (_error != null)
                        throw _error;
                }

                public virtual void Completed(Exception error)
                {
                    _error = error;
                }

                private Exception _error;
            }

            #endregion

            #region Class GetCtx ...

            // 获取缓存任务的上下文状态
            class GetCtx : CtxBase
            {
                public readonly HashSet<TKey> Keys = new HashSet<TKey>();
                private readonly ManualResetEvent _waitEvent = new ManualResetEvent(false);
                public Dictionary<TKey, TValue> Result;

                public override void Wait()
                {
                    _waitEvent.WaitOne();
                    base.Wait();
                }

                public override void Completed(Exception error)
                {
                    base.Completed(error);
                    _waitEvent.Set();
                }

                public override int GetCount()
                {
                    return Keys.Count;
                }
            }

            #endregion

            #region Class RemoveCtx ...

            // 删除缓存任务的上下文状态
            class RemoveCtx : CtxBase
            {
                public readonly HashSet<TKey> Keys = new HashSet<TKey>();

                public override int GetCount()
                {
                    return Keys.Count;
                }
            }

            #endregion

            #region Class AddCtx ...

            // 添加缓存任务的上下文状态
            class AddCtx : CtxBase
            {
                public readonly List<KeyValuePair<TKey, TValue>> Items = new List<KeyValuePair<TKey, TValue>>();

                public override int GetCount()
                {
                    return Items.Count;
                }
            }

            #endregion

            private static TCtx _Exchange<TCtx>(ref TCtx ctx) where TCtx : CtxBase, new()
            {
                if (ctx.GetCount() == 0)
                    return null;

                TCtx old = ctx;
                ctx = new TCtx();
                return old;
            }

            private void _TryInvoke(CtxBase ctx, Action action)
            {
                try
                {
                    action();
                    ctx.Completed(null);
                }
                catch (Exception ex)
                {
                    ctx.Completed(ex);
                }
            }

            private void _DispatchFunc()
            {
                while (WaitHandle.WaitAny(new WaitHandle[] { _waitForExit, _waitForTask }) != 0)
                {
                    GetCtx getCtx, refreshGetCtx;
                    RemoveCtx removeCtx;
                    AddCtx addCtx;

                    lock (_syncLocker)
                    {
                        getCtx = _Exchange(ref _getCtx);
                        refreshGetCtx = _Exchange(ref _refreshGetCtx);
                        removeCtx = _Exchange(ref _removeCtx);
                        addCtx = _Exchange(ref _addCtx);
                    }

                    // 无刷新的获取
                    if (getCtx != null)
                    {
                        _TryInvoke(getCtx, () => { _Get(getCtx, false); });
                    }

                    // 带刷新的获取
                    if (refreshGetCtx != null)
                    {
                        _TryInvoke(refreshGetCtx, () => { _Get(refreshGetCtx, true); });
                    }

                    // 删除
                    if (removeCtx != null)
                    {
                        _TryInvoke(removeCtx, () => { _nextNode.Remove(removeCtx.Keys.ToArray()); });
                    }

                    // 添加
                    if (addCtx != null)
                    {
                        _TryInvoke(addCtx, () => { _nextNode.Add(addCtx.Items.ToArray()); });
                    }
                }
            }

            private void _Get(GetCtx ctx, bool refresh)
            {
                KeyValuePair<TKey, TValue>[] values = _nextNode.Get(ctx.Keys.ToArray(), refresh);
                ctx.Result = values.ToDictionary(item => item.Key, item => item.Value);
            }

            private void _NotifyRunning()
            {
                _waitForTask.Set();
            }

            public KeyValuePair<TKey, TValue>[] Get(TKey[] keys, bool refresh)
            {
                if (keys.IsNullOrEmpty())
                    return _emptyKeyValueArr;

                GetCtx ctx;
                lock (_syncLocker)
                {
                    ctx = refresh ? _refreshGetCtx : _getCtx;
                    ctx.Keys.AddRange(keys);
                }

                _NotifyRunning();
                ctx.Wait();

                Dictionary<TKey, TValue> dict = ctx.Result;
                List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
                for (int k = 0; k < keys.Length; k++)
                {
                    TKey key = keys[k];
                    TValue value;
                    if (dict.TryGetValue(key, out value))
                        list.Add(new KeyValuePair<TKey, TValue>(key, value));
                }

                return list.ToArray();
            }

            public void Add(KeyValuePair<TKey, TValue>[] items)
            {
                if (items.IsNullOrEmpty())
                    return;

                AddCtx ctx;
                lock (_syncLocker)
                {
                    ctx = _addCtx;
                    ctx.Items.AddRange(items);
                }

                _NotifyRunning();
            }

            public void Remove(TKey[] keys)
            {
                if (keys.IsNullOrEmpty())
                    return;

                RemoveCtx ctx;
                lock (_syncLocker)
                {
                    ctx = _removeCtx;
                    ctx.Keys.AddRange(keys);
                }

                _NotifyRunning();
            }

            public void Dispose()
            {
                _waitForExit.Set();
            }

            public void SetNextNode(ICacheChainNode<TKey, TValue> nextNode)
            {
                _nextNode = nextNode ?? Empty;
            }
        }
    }
}
