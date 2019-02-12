using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using System.Runtime.Serialization;
using Common.Package.Serializer;
using Common.Utility;
using ServiceFairy.Entities.Sms;
using Common.Contracts.Service;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private SmsInvoker _sms;

        /// <summary>
        /// Sms Service
        /// </summary>
        public SmsInvoker Sms
        {
            get { return _sms ?? (_sms = new SmsInvoker(this)); }
        }

        public class SmsInvoker : Invoker
        {
            public SmsInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            public ServiceResult SendSr(string[] phoneNumbers, string content, CallingSettings settings = null)
            {
                return SmsService.Send(Sc, new Sms_Send_Request { PhoneNumbers = phoneNumbers, Content = content }, settings);
            }

            public void Send(string[] phoneNumbers, string content, CallingSettings settings = null)
            {
                InvokeWithCheck(SendSr(phoneNumbers, content, settings));
            }

            public ServiceResult SendSr(string phoneNumber, string content, CallingSettings settings = null)
            {
                return SendSr(new[] { phoneNumber }, content, settings);
            }

            public void Send(string phoneNumber, string content, CallingSettings settings = null)
            {
                InvokeWithCheck(SendSr(phoneNumber, content, settings));
            }
        }
    }
}
