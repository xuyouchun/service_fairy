using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.Management.Web.Core
{
    public class JAppStatusCodeDoc
    {
        public string Category { get; set; }

        public string Service { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public string DefName { get; set; }

        public string Title { get; set; }

        public int ServiceCode { get; set; }

        public int InnerCode { get; set; }

        public string Desc { get; set; }

        public static JAppStatusCodeDoc From( AppStatusCodeInfo info, string serviceName)
        {
            return new JAppStatusCodeDoc {
                Category = ContractUtility.GetServiceStatusCodeCategory(info.Code).GetDesc(),
                Title = info.Title, Desc = info.Desc, Code = info.Code, Name = info.Name,
                DefName = JAppStatusCodeUtility.ToDefName(info.Name, serviceName), Service = serviceName,
                ServiceCode = ContractUtility.GetServiceCode(info.Code), InnerCode = ContractUtility.GetInnerStatusCode(info.Code)
            };
        }
    }
}