using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Common
{
    #region Class ChannelAdapterBase ...

    abstract class ChannelAdapterBase : CommunicationObjectAdapterBase, IChannel
    {
        public ChannelAdapterBase(IChannel inner)
            : base(inner)
        {

        }

        public new IChannel Inner
        {
            get { return (IChannel)base.Inner; }
        }

        public virtual T GetProperty<T>() where T : class
        {
            return Inner.GetProperty<T>();
        }
    }

    #endregion

    #region Class InputChannelAdapterBase ...

    abstract class InputChannelAdapterBase : ChannelAdapterBase, IInputChannel
    {
        public InputChannelAdapterBase(IInputChannel inner)
            : base(inner)
        {

        }

        public new IInputChannel Inner
        {
            get { return (IInputChannel)base.Inner; }
        }

        public virtual IAsyncResult BeginReceive(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginReceive(timeout, callback, state);
        }

        public virtual IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            return Inner.BeginReceive(callback, state);
        }

        public virtual IAsyncResult BeginTryReceive(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginTryReceive(timeout, callback, state);
        }

        public virtual IAsyncResult BeginWaitForMessage(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginWaitForMessage(timeout, callback, state);
        }

        public virtual Message EndReceive(IAsyncResult result)
        {
            return Inner.EndReceive(result);
        }

        public virtual bool EndTryReceive(IAsyncResult result, out Message message)
        {
            return Inner.EndTryReceive(result, out message);
        }

        public virtual bool EndWaitForMessage(IAsyncResult result)
        {
            return Inner.EndWaitForMessage(result);
        }

        public virtual System.ServiceModel.EndpointAddress LocalAddress
        {
            get { return Inner.LocalAddress; }
        }

        public virtual Message Receive(TimeSpan timeout)
        {
            return Inner.Receive(timeout);
        }

        public virtual Message Receive()
        {
            return Inner.Receive();
        }

        public virtual bool TryReceive(TimeSpan timeout, out Message message)
        {
            return Inner.TryReceive(timeout, out message);
        }

        public virtual bool WaitForMessage(TimeSpan timeout)
        {
            return Inner.WaitForMessage(timeout);
        }
    }

    #endregion

    #region Class OutputChannelAdapterBase ...

    abstract class OutputChannelAdapterBase : ChannelAdapterBase, IOutputChannel
    {
        public OutputChannelAdapterBase(IOutputChannel inner)
            : base(inner)
        {

        }

        public new IOutputChannel Inner
        {
            get { return (IOutputChannel)base.Inner; }
        }

        public virtual IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginSend(message, timeout, callback, state);
        }

        public virtual IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            return Inner.BeginSend(message, callback, state);
        }

        public virtual void EndSend(IAsyncResult result)
        {
            Inner.EndSend(result);
        }

        public virtual System.ServiceModel.EndpointAddress RemoteAddress
        {
            get { return Inner.RemoteAddress; }
        }

        public virtual void Send(Message message, TimeSpan timeout)
        {
            Inner.Send(message, timeout);
        }

        public virtual void Send(Message message)
        {
            Inner.Send(message);
        }

        public virtual Uri Via
        {
            get { return Inner.Via; }
        }
    }

    #endregion

    #region Class InputSessionChannelAdapterBase ...

    abstract class InputSessionChannelAdapterBase : InputChannelAdapterBase, IInputSessionChannel
    {
        public InputSessionChannelAdapterBase(IInputSessionChannel inner)
            : base(inner)
        {

        }

        public new IInputSessionChannel Inner
        {
            get { return (IInputSessionChannel)base.Inner; }
        }

        public virtual IInputSession Session
        {
            get { return Inner.Session; }
        }
    }

    #endregion

    #region Class OutputSessionChannelAdapterBase ...

    abstract class OutputSessionChannelAdapterBase : OutputChannelAdapterBase, IOutputSessionChannel
    {
        public OutputSessionChannelAdapterBase(IOutputSessionChannel inner)
            : base(inner)
        {

        }

        public new IOutputSessionChannel Inner
        {
            get { return (IOutputSessionChannel)base.Inner; }
        }

        public virtual IOutputSession Session
        {
            get { return Inner.Session; }
        }
    }

    #endregion

    #region Class RequestChannelAdapterBase ...

    abstract class RequestChannelAdapterBase : ChannelAdapterBase, IRequestChannel
    {
        public RequestChannelAdapterBase(IRequestChannel inner)
            : base(inner)
        {

        }

        public new IRequestChannel Inner
        {
            get { return (IRequestChannel)base.Inner; }
        }

        public virtual IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginRequest(message, timeout, callback, state);
        }

        public virtual IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            return Inner.BeginRequest(message, callback, state);
        }

        public virtual Message EndRequest(IAsyncResult result)
        {
            return Inner.EndRequest(result);
        }

        public virtual System.ServiceModel.EndpointAddress RemoteAddress
        {
            get { return Inner.RemoteAddress; }
        }

        public virtual Message Request(Message message, TimeSpan timeout)
        {
            return Inner.Request(message, timeout);
        }

        public virtual Message Request(Message message)
        {
            return Inner.Request(message);
        }

        public virtual Uri Via
        {
            get { return Inner.Via; }
        }
    }

    #endregion

    #region Class ReplyChannelAdapterBase ...

    abstract class ReplyChannelAdapterBase : ChannelAdapterBase, IReplyChannel
    {
        public ReplyChannelAdapterBase(IReplyChannel inner)
            : base(inner)
        {

        }

        public new IReplyChannel Inner
        {
            get { return (IReplyChannel)base.Inner; }
        }

        public virtual IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginReceiveRequest(timeout, callback, state);
        }

        public virtual IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state)
        {
            return Inner.BeginReceiveRequest(callback, state);
        }

        public virtual IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginTryReceiveRequest(timeout, callback, state);
        }

        public virtual IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginWaitForRequest(timeout, callback, state);
        }

        public virtual RequestContext EndReceiveRequest(IAsyncResult result)
        {
            return Inner.EndReceiveRequest(result);
        }

        public virtual bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context)
        {
            return Inner.EndTryReceiveRequest(result, out context);
        }

        public virtual bool EndWaitForRequest(IAsyncResult result)
        {
            return Inner.EndWaitForRequest(result);
        }

        public virtual System.ServiceModel.EndpointAddress LocalAddress
        {
            get { return Inner.LocalAddress; }
        }

        public virtual RequestContext ReceiveRequest(TimeSpan timeout)
        {
            return Inner.ReceiveRequest(timeout);
        }

        public virtual RequestContext ReceiveRequest()
        {
            return Inner.ReceiveRequest();
        }

        public virtual bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
        {
            return Inner.TryReceiveRequest(timeout, out context);
        }

        public virtual bool WaitForRequest(TimeSpan timeout)
        {
            return Inner.WaitForRequest(timeout);
        }
    }

    #endregion

    #region Class RequestSessionChannelAdapterBase ...

    class RequestSessionChannelAdapterBase : RequestChannelAdapterBase, IRequestSessionChannel
    {
        public RequestSessionChannelAdapterBase(IRequestSessionChannel inner)
            : base(inner)
        {

        }

        public new IRequestSessionChannel Inner
        {
            get { return (IRequestSessionChannel)base.Inner; }
        }

        public IOutputSession Session
        {
            get { return Inner.Session; }
        }
    }

    #endregion

    #region Class ReplySessionChannelAdapterBase ...

    abstract class ReplySessionChannelAdapterBase : ReplyChannelAdapterBase, IReplySessionChannel
    {
        public ReplySessionChannelAdapterBase(IReplySessionChannel inner)
            : base(inner)
        {

        }

        public new IReplySessionChannel Inner
        {
            get { return (IReplySessionChannel)base.Inner; }
        }

        public virtual IInputSession Session
        {
            get { return Inner.Session; }
        }
    }

    #endregion

    #region Class DuplexChannelAdapterBase ...

    class DuplexChannelAdapterBase : InputChannelAdapterBase, IInputChannel, IOutputChannel, IDuplexChannel
    {
        public DuplexChannelAdapterBase(IDuplexChannel inner)
            : base(inner)
        {
            
        }

        public new IDuplexChannel Inner
        {
            get { return (IDuplexChannel)base.Inner; }
        }

        public virtual IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return Inner.BeginSend(message, timeout, callback, state);
        }

        public virtual IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            return Inner.BeginSend(message, callback, state);
        }

        public virtual void EndSend(IAsyncResult result)
        {
            Inner.EndSend(result);
        }

        public virtual System.ServiceModel.EndpointAddress RemoteAddress
        {
            get { return Inner.RemoteAddress; }
        }

        public virtual void Send(Message message, TimeSpan timeout)
        {
            Inner.Send(message, timeout);
        }

        public virtual void Send(Message message)
        {
            Inner.Send(message);
        }

        public virtual Uri Via
        {
            get { return Inner.Via; }
        }
    }

    #endregion

    #region Class DuplexSessionChannelAdapterBase ...

    class DuplexSessionChannelAdapterBase : DuplexChannelAdapterBase, IDuplexSessionChannel
    {
        public DuplexSessionChannelAdapterBase(IDuplexSessionChannel inner)
            : base(inner)
        {

        }

        public new IDuplexSessionChannel Inner
        {
            get { return (IDuplexSessionChannel)base.Inner; }
        }

        public virtual IDuplexSession Session
        {
            get { return Inner.Session; }
        }
    }

    #endregion

}
