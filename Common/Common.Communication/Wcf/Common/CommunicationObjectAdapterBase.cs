using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics.Contracts;
using System.Collections;

namespace Common.Communication.Wcf.Common
{
    abstract class CommunicationObjectAdapterBase : ICommunicationObject, ICommunicationObjectEx
    {
        public CommunicationObjectAdapterBase(ICommunicationObject inner)
        {
            Contract.Requires(inner != null);
            _inner = inner;
        }

        private readonly ICommunicationObject _inner;

        public ICommunicationObject Inner
        {
            get { return _inner; }
        }

        public virtual void Abort()
        {
            _inner.Abort();
        }

        public virtual IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _inner.BeginClose(timeout, callback, state);
        }

        public virtual IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            return _inner.BeginClose(callback, state);
        }

        public virtual IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _inner.BeginOpen(timeout, callback, state);
        }

        public virtual IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            return _inner.BeginOpen(callback, state);
        }

        public virtual void Close(TimeSpan timeout)
        {
            _inner.Close(timeout);
        }

        public virtual void Close()
        {
            _inner.Close();
        }

        public virtual event EventHandler Closed
        {
            add { _inner.Closed += value; }
            remove { _inner.Closed -= value; }
        }

        public virtual event EventHandler Closing
        {
            add { _inner.Closing += value; }
            remove { _inner.Closing -= value; }
        }

        public virtual void EndClose(IAsyncResult result)
        {
            _inner.EndClose(result);
        }

        public virtual void EndOpen(IAsyncResult result)
        {
            _inner.EndOpen(result);
        }

        public virtual event EventHandler Faulted
        {
            add { _inner.Faulted += value; }
            remove { _inner.Faulted -= value; }
        }

        public virtual void Open(TimeSpan timeout)
        {
            _inner.Open(timeout);
        }

        public virtual void Open()
        {
            _inner.Open();
        }

        public virtual event EventHandler Opened
        {
            add { _inner.Opened += value; }
            remove { _inner.Opened -= value; }
        }

        public virtual event EventHandler Opening
        {
            add { _inner.Opening += value; }
            remove { _inner.Opening -= value; }
        }

        public virtual CommunicationState State
        {
            get { return _inner.State; }
        }

        private readonly Hashtable _items = new Hashtable();

        public System.Collections.IDictionary Items
        {
            get { return _items; }
        }
    }
}
