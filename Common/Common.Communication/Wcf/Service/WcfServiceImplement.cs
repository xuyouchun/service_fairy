using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.IO;
using Common.Utility;
using Common.Package;
using Common.Contracts.Service;

namespace Common.Communication.Wcf.Service
{
    /// <summary>
    /// WCF服务的实现
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple
    )]
    class WcfServiceImplement : IWcfServiceInterface
    {
        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Message Request(Message input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 单向通信
        /// </summary>
        /// <param name="input"></param>
        public void OneWay(Message input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 打开流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Stream OpenStream(Stream input)
        {
            DuplexStream duplexStream = new DuplexStream(true);

            duplexStream.Write(Encoding.UTF8.GetBytes("ABCDEFGHIJKLMN"));
            duplexStream.Flush();

            GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(5), new TaskFuncAdapter(delegate {
                duplexStream.Write(Encoding.UTF8.GetBytes("ABCDEFGHIJKLMN"));
                duplexStream.Flush();
            }), false);

            WcfStream stream = new WcfStream(input, null);
            return duplexStream;
        }
    }

    /// <summary>
    /// WCF服务的实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class DuplexWcfServiceImplement : WcfServiceImplement, IDuplexWcfServiceInterface
    {
        
    }
}
