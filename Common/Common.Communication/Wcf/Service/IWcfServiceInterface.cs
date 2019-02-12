using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Security;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using System.IO;

namespace Common.Communication.Wcf.Service
{
    /// <summary>
    /// WCF服务接口
    /// </summary>
    [ServiceContract(Name = "srv")]
    interface IWcfServiceInterface
    {
        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Action = WcfRequestActions.RequestReply, ReplyAction = "*")]
        Message Request(Message input);

        /// <summary>
        /// 单向通道
        /// </summary>
        /// <param name="input"></param>
        [OperationContract(Action = WcfRequestActions.OneWay, IsOneWay = true)]
        void OneWay(Message input);

        /// <summary>
        /// 输入输出流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Action = WcfRequestActions.OpenStream, ReplyAction = "*")]
        Stream OpenStream(Stream input);
    }
}
