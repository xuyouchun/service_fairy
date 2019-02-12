using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using ServiceFairy.Entities.Sys;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    class InvokeControlContext
    {
        public InvokeControlContext(SystemInvoker invoker,
            ClientDesc clientDesc, ServiceDesc serviceDesc, AppCommandInfo cmdInfo)
        {
            SystemInvoker = invoker;
            ClientDesc = clientDesc;
            ServiceDesc = serviceDesc;
            AppCommandInfo = cmdInfo;

            _wrapper = new Lazy<Wrapper>(_Load);
        }

        public SystemInvoker SystemInvoker { get; private set; }

        public ClientDesc ClientDesc { get; private set; }

        public ServiceDesc ServiceDesc { get; private set; }

        public AppCommandInfo AppCommandInfo { get; private set; }

        class Wrapper
        {
            public AppCommandDoc ArgumentInfo { get; set; }
        }

        private readonly Lazy<Wrapper> _wrapper;

        private Wrapper _Load()
        {
            AppCommandDoc info = SystemInvoker.Sys.GetAppCommandDoc(ClientDesc.ClientID,
                ServiceDesc, AppCommandInfo.CommandDesc, (DataFormat[])null);

            return new Wrapper { ArgumentInfo = info };
        }

        private string _GetSample(AppArgumentData data, DataFormat format, string def = "<no sample>")
        {
            AppArgumentDataSample sample = (data.Samples == null) ? null
                : data.Samples.FirstOrDefault(s => s.Format == format);

            return (sample == null ? null : sample.Sample) ?? def;
        }

        public string GetSample(string direct, DataFormat format)
        {
            if (direct == "input")
            {
                switch (format)
                {
                    case DataFormat.Json:
                        return _GetSample(_wrapper.Value.ArgumentInfo.Input, DataFormat.Json);

                    case DataFormat.Xml:
                        return _GetSample(_wrapper.Value.ArgumentInfo.Input, DataFormat.Xml);
                }
            }
            else if (direct == "output")
            {
                switch (format)
                {
                    case DataFormat.Json:
                        return _GetSample(_wrapper.Value.ArgumentInfo.Output, DataFormat.Json);

                    case DataFormat.Xml:
                        return _GetSample(_wrapper.Value.ArgumentInfo.Output, DataFormat.Xml);
                }
            }

            return "";
        }

        /// <summary>
        /// 获取参数文档
        /// </summary>
        /// <param name="direct"></param>
        /// <returns></returns>
        public TypeDocTree GetDocTree(string direct)
        {
            switch (direct)
            {
                case "input":
                    return _wrapper.Value.ArgumentInfo.Input.DocTree;

                case "output":
                    return _wrapper.Value.ArgumentInfo.Output.DocTree;

                default:
                    return null;
            }
        }
    }
}
