using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using Common.Contracts.Service;
using System.Collections;
using System.ServiceModel;

namespace Common.Communication.Wcf.Common
{
    /// <summary>
    /// 
    /// </summary>
    interface ICommunicationObjectEx : ICommunicationObject
    {
        /// <summary>
        /// 所有与该通道相关的信息
        /// </summary>
        IDictionary Items { get; }
    }
}
