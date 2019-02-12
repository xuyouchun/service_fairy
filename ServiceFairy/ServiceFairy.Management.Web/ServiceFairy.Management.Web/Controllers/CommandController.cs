using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ServiceFairy.Management.Web.Models;
using ServiceFairy.Management.Web.Core;
using Common.Contracts.Service;
using ServiceFairy.Entities.Sys;
using Common.Contracts;

namespace ServiceFairy.Management.Web.Controllers
{
    public class CommandController : Controller
    {
        [HttpGet]
        public ActionResult Doc()
        {
            JCoreInvoker.Instance.GetAppServices();

            return View();
        }

        /// <summary>
        /// 获取AppService
        /// </summary>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetAppServiceDocs()
        {
            JAppServiceDoc[] docs = JCoreInvoker.Instance.GetAppServices();
            Array.Sort(docs);

            return Json(docs);
        }

        /// <summary>
        /// 获取AppCommand
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetAppCommands(string serviceName, string serviceVersion)
        {
            JAppCommand[] cmds = JCoreInvoker.Instance.GetAppCommands(new ServiceDesc(serviceName, serviceVersion));
            Array.Sort(cmds);

            return Json(cmds);
        }

        /// <summary>
        /// 获取接口文档
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <param name="messageName"></param>
        /// <param name="messageVersion"></param>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetAppCommandDocs(string serviceName, string serviceVersion, string commandName, string commandVersion)
        {
            try
            {
                ServiceDesc sd = new ServiceDesc(serviceName, serviceVersion);
                CommandDesc cd = new CommandDesc(commandName, commandVersion);

                AppCommandDoc doc = JCoreInvoker.Instance.GetAppCommandDoc(sd, cd);
                return Json(doc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取AppMessage
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetAppMessages(string serviceName, string serviceVersion)
        {
            JAppMessage[] msgs = JCoreInvoker.Instance.GetAppMessages(new ServiceDesc(serviceName, serviceVersion));
            Array.Sort(msgs);
            return Json(msgs);
        }

        /// <summary>
        /// 获取消息文档
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <param name="commandName"></param>
        /// <param name="commandVersion"></param>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetAppMessageDocs(string serviceName, string serviceVersion, string messageName, string messageVersion)
        {
            ServiceDesc sd = new ServiceDesc(serviceName, serviceVersion);
            MessageDesc md = new MessageDesc(messageName, messageVersion);

            AppMessageDoc doc = JCoreInvoker.Instance.GetAppMessageDoc(sd, md);
            return Json(doc);
        }

        /// <summary>
        /// 获取状态码文档
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetAppStatusCodes(string serviceName, string serviceVersion)
        {
            ServiceDesc sd = new ServiceDesc(serviceName, serviceVersion);

            JAppStatusCodeDoc[] doc = JCoreInvoker.Instance.GetAppStatusCodeDocs(sd);
            return Json(doc);
        }

        /// <summary>
        /// 获取用于生成代码的所有数据
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <returns></returns>
        [HttpPost, JsonExceptionFilter]
        public ActionResult GetCodeGenerationData(string serviceName, string serviceVersion)
        {
            ServiceDesc sd = new ServiceDesc(serviceName, serviceVersion);

            return Json(new {
                types = JCoreInvoker.Instance.GetEntityDocs(sd),
                commands = JCoreInvoker.Instance.GetAppCommands(sd),
                messages = JCoreInvoker.Instance.GetAppMessages(sd),
                statusCodes = JCoreInvoker.Instance.GetAppStatusCodeDocs(sd),
            });
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new MyJsonResult { Data = data, ContentEncoding = contentEncoding, ContentType = contentType, JsonRequestBehavior = behavior };
        }
    }
}
