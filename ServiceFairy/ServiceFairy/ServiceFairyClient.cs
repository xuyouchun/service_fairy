using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf;
using Common.Contracts;
using Common.Package.Service;

namespace ServiceFairy
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceFairyClient : ServiceClient, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="dataFormat"></param>
        /// <param name="disposeIt"></param>
        public ServiceFairyClient(ICommunicate communicate, DataFormat dataFormat = DataFormat.Unknown, bool disposeIt = false)
            : base(_Revise(communicate, disposeIt), dataFormat, true)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dataFormat"></param>
        /// <param name="disposeIt"></param>
        public ServiceFairyClient(WcfConnection connection, DataFormat dataFormat = DataFormat.Unknown, bool disposeIt = false)
            : this((ICommunicate)connection, dataFormat, disposeIt)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dataFormat"></param>
        /// <param name="disposeIt"></param>
        public ServiceFairyClient(IConnection connection, DataFormat dataFormat = DataFormat.Unknown, bool disposeIt = false)
            : this((ICommunicate)connection, dataFormat, disposeIt)
        {

        }

        private static ICommunicate _Revise(ICommunicate communicate, bool disposeIt)
        {
            return _CreateAdapter(communicate, disposeIt);
        }

        #region Class CommunicateAdapter ...

        [System.Diagnostics.DebuggerStepThrough]
        class CommunicateAdapter : ICommunicate, IDisposable
        {
            public CommunicateAdapter(ICommunicate communicate, bool disposeIt)
            {
                _disposeIt = disposeIt;
                _communicate = communicate;
            }

            private volatile bool _initialized = false;
            private bool _disposeIt;
            private readonly ICommunicate _communicate;

            private void _EnsureInit()
            {
                if (_initialized)
                    return;

                lock (this)
                {
                    if (!_initialized)
                    {
                        IConnection con = _communicate as IConnection;
                        if (con != null)
                        {
                            if (con.State != ConnectionState.Opened)
                            {
                                con.Open();
                                _disposeIt = true;
                            }
                        }

                        _initialized = true;
                    }
                }
            }

            public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
            {
                _EnsureInit();
                return _communicate.Call(context, method, data, settings);
            }

            public void Dispose()
            {
                if (_disposeIt && _initialized)
                    _communicate.Dispose();
            }
        }

        #endregion

        #region Class ConnectionAdapter ...

        class ConnectionAdapter : CommunicateAdapter, IConnection
        {
            public ConnectionAdapter(IConnection connection, bool disposeIt)
                : base(connection, disposeIt)
            {
                _con = connection;
            }

            private readonly IConnection _con;

            public event ConnectionDataReceivedEventHandler Received
            {
                add { _con.Received += value; }
                remove { _con.Received -= value; }
            }

            public bool Duplex
            {
                get { return _con.Duplex; }
            }

            public ConnectionState State
            {
                get { return _con.State; }
            }

            public void Open()
            {
                _con.Open();
            }
        }

        #endregion

        private static ICommunicate _CreateAdapter(ICommunicate communicate, bool disposeIt)
        {
            IConnection connection = communicate as IConnection;
            if (connection != null)
            {
                return new ConnectionAdapter(connection, disposeIt);
            }
            else
            {
                return new CommunicateAdapter(communicate, disposeIt);
            }
        }
    }
}
