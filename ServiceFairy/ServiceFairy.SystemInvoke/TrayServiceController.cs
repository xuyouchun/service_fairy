using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 服务控制器
    /// </summary>
    public class TrayServiceController
    {
        private TrayServiceController(ServiceEndPoint endpoint, Sid sid = default(Sid))
        {
            Contract.Requires(endpoint != null);

            EndPoint = endpoint;
            Sid = sid;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="endpoint">服务终端</param>
        /// <param name="sid">安全码</param>
        public TrayServiceController(CoreInvoker invoker, ServiceEndPoint endpoint, Sid sid = default(Sid))
            : this(endpoint, sid)
        {
            Contract.Requires(invoker != null);

            _invoker = invoker;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation">导航地址</param>
        /// <param name="endpoint">服务终端</param>
        /// <param name="sid">安全码</param>
        public TrayServiceController(CommunicationOption navigation, ServiceEndPoint endpoint, Sid sid = default(Sid))
            : this(endpoint, sid)
        {
            Contract.Requires(navigation != null);

            _invoker = CoreInvoker.FromNavigation(navigation);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigation">导航地址</param>
        /// <param name="endpoint">服务终端</param>
        /// <param name="sid">安全码</param>
        public TrayServiceController(string navigation, ServiceEndPoint endpoint, Sid sid = default(Sid))
            : this(endpoint, sid)
        {
            Contract.Requires(navigation != null);

            _invoker = CoreInvoker.FromNavigation(navigation);
        }

        private readonly CoreInvoker _invoker;

        /// <summary>
        /// 服务终端
        /// </summary>
        public ServiceEndPoint EndPoint { get; private set; }

        /// <summary>
        /// 安全码
        /// </summary>
        public Sid Sid { get; private set; }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            _invoker.Master.StartService(EndPoint);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _invoker.Master.StopService(EndPoint);
        }

        /// <summary>
        /// 等待服务变为指定的状态
        /// </summary>
        /// <param name="running">状态，是否运行</param>
        /// <returns>如果未超时，则返回true</returns>
        public bool WaitForStatus(bool running)
        {
            return WaitForStatus(running, TimeSpan.Zero);
        }

        /// <summary>
        /// 等待服务变为指定的状态
        /// </summary>
        /// <param name="running">状态，是否运行</param>
        /// <param name="timeout">超时时间，设置为负值或0则无限期等待</param>
        /// <returns>如果未超时，则返回true</returns>
        public bool WaitForStatus(bool running, TimeSpan timeout)
        {
            ManualResetEvent r = new ManualResetEvent(false);
            bool cancel = false;
            Exception error = null;

            ThreadPool.QueueUserWorkItem(delegate {
                while (!cancel)
                {
                    try
                    {
                        if (Running == running)
                            break;
                    }
                    catch(Exception ex)
                    {
                        error = ex;
                        break;
                    }

                    Thread.Sleep(100);
                }

                r.Set();
            });

            bool result = true;
            if (timeout <= TimeSpan.Zero)
                r.WaitOne();
            else
                result = r.WaitOne(timeout);

            if (error != null)
                throw error;

            cancel = true;
            return result;
        }

        private bool? _running;

        /// <summary>
        /// 运行状态
        /// </summary>
        public bool Running
        {
            get
            {
                if (_running == null || _lastRefresh < DateTime.Now - TimeSpan.FromSeconds(1))
                    Refresh();

                return (bool)_running;
            }
        }

        private DateTime _lastRefresh;

        /// <summary>
        /// 刷新状态
        /// </summary>
        public bool Refresh()
        {
            bool running = _invoker.Tray.ExistsService(EndPoint, Sid);
            _running = running;

            _lastRefresh = DateTime.Now;

            return running;
        }
    }
}
