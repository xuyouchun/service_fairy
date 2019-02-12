using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Diagnostics.Contracts;

namespace Common.Communication.Wcf.Common
{
    abstract class ChannelListenerAdapterBase : CommunicationObjectAdapterBase, IChannelListener
    {
        public ChannelListenerAdapterBase(IChannelListener inner)
            : base(inner)
        {

        }

        public new IChannelListener Inner
        {
            get { return (IChannelListener)base.Inner; }
        }

        public virtual IAsyncResult BeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginWaitForChannel(timeout, callback, state);
        }

        public virtual bool EndWaitForChannel(IAsyncResult result)
        {
            return Inner.EndWaitForChannel(result);
        }

        public virtual T GetProperty<T>() where T : class
        {
            return Inner.GetProperty<T>();
        }

        public virtual Uri Uri
        {
            get { return Inner.Uri; }
        }

        public virtual bool WaitForChannel(TimeSpan timeout)
        {
            return Inner.WaitForChannel(timeout);
        }
    }

    abstract class ChannelListenerAdapterBase<TChannel> : ChannelListenerAdapterBase, IChannelListener<TChannel> where TChannel : class, IChannel
    {
        public ChannelListenerAdapterBase(IChannelListener<TChannel> inner)
            : base(inner)
        {
            
        }

        public new IChannelListener<TChannel> Inner
        {
            get { return (IChannelListener<TChannel>)base.Inner; }
        }

        public virtual TChannel AcceptChannel(TimeSpan timeout)
        {
            return Inner.AcceptChannel(timeout);
        }

        public virtual TChannel AcceptChannel()
        {
            return Inner.AcceptChannel();
        }

        public virtual IAsyncResult BeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginAcceptChannel(timeout, callback, state);
        }

        public virtual IAsyncResult BeginAcceptChannel(AsyncCallback callback, object state)
        {
            return Inner.BeginAcceptChannel(callback, state);
        }

        public virtual TChannel EndAcceptChannel(IAsyncResult result)
        {
            return Inner.EndAcceptChannel(result);
        }
    }
}
