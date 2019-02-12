using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Diagnostics.Contracts;

namespace Common.Communication.Wcf.Common
{
    abstract class ChannelFactoryAdapterBase : CommunicationObjectAdapterBase, IChannelFactory
    {
        public ChannelFactoryAdapterBase(IChannelFactory inner)
            : base(inner)
        {
            
        }

        public new IChannelFactory Inner
        {
            get { return (IChannelFactory)base.Inner; }
        }

        public T GetProperty<T>() where T : class
        {
            return Inner.GetProperty<T>();
        }
    }

    abstract class ChannelFactoryAdapterBase<TChannel> : ChannelFactoryAdapterBase, IChannelFactory<TChannel>
    {
        public ChannelFactoryAdapterBase(IChannelFactory<TChannel> inner)
            : base(inner)
        {
            
        }

        public new IChannelFactory<TChannel> Inner
        {
            get { return (IChannelFactory<TChannel>)base.Inner; }
        }

        public virtual TChannel CreateChannel(System.ServiceModel.EndpointAddress to, Uri via)
        {
            return Inner.CreateChannel(to, via);
        }

        public virtual TChannel CreateChannel(System.ServiceModel.EndpointAddress to)
        {
            return Inner.CreateChannel(to);
        }
    }
}
