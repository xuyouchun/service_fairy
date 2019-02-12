using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy;
using ServiceFairy.Entities;

namespace BhFairy.Service.Test
{
    /// <summary>
    /// 写入日志
    /// </summary>
    [AppCommand("WriteLog")]
    class WriteLogAppCommand : ACS<Service>.Func<Test_WriteLog_Request, Test_WriteLog_Reply>
    {
        protected override Test_WriteLog_Reply OnExecute(AppCommandExecuteContext<Service> context, Test_WriteLog_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.TestAppComponent.WriteLog(req.Log);

            return new Test_WriteLog_Reply();
        }
    }

    /// <summary>
    /// 记录日志－请求
    /// </summary>
    [Serializable, DataContract]
    class Test_WriteLog_Request : RequestEntity
    {
        /// <summary>
        /// 日志内容
        /// </summary>
        [DataMember]
        public string Log { get; set; }
    }

    /// <summary>
    /// 记录日志－应答
    /// </summary>
    [Serializable, DataContract]
    class Test_WriteLog_Reply : ReplyEntity
    {

    }
}
