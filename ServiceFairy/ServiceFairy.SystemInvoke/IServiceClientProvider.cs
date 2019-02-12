using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package.Service;
using Common.Contracts.Service;

namespace ServiceFairy.SystemInvoke
{
    public interface IServiceClientProvider : IObjectProvider<IServiceClient>
    {

    }

    class ServiceClientProvider : IServiceClientProvider
    {
        public ServiceClientProvider(IServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        private readonly IServiceClient _serviceClient;

        public IServiceClient Get()
        {
            return _serviceClient;
        }
    }
}
