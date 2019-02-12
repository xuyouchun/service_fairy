using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;
using System.Threading;

namespace WcfTest.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(DuplexService), new Uri("http://127.0.0.1:91/s")))
            {
                serviceHost.AddServiceEndpoint(typeof(IDuplexService), new NetTcpBinding(), "net.tcp://127.0.0.1:90/s");

                ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
                metadataBehavior.HttpGetEnabled = true;
                serviceHost.Description.Behaviors.Add(metadataBehavior);
    
                serviceHost.Open();

                Console.WriteLine("Running ...");
                Console.Read();
            }

            /*
            ServiceHost serviceHost = new ServiceHost(typeof(TestService));
            ICollection<BindingElement> bindingElements = new List<BindingElement>();
            HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
            JsonMessageEncodingBindingElement jsonMessageEncodingBindingElement = new JsonMessageEncodingBindingElement(new BinaryMessageEncodingBindingElement());
            bindingElements.Add(jsonMessageEncodingBindingElement);
            bindingElements.Add(httpBindingElement);
            CustomBinding customBinding = new CustomBinding(bindingElements);
            serviceHost.AddServiceEndpoint(typeof(ITestService), customBinding, "http://127.0.0.1:8082/TestService");

            //serviceHost.AddServiceEndpoint(typeof(ITestService), new BasicHttpBinding(), "http://127.0.0.1:8082/TestService");

            /*
            // metadata
            ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(metadataBehavior);
            //serviceHost.Description.Behaviors.Add(new MyBehavior());*/

            // json serialize
            /*
            foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new OutputMessageEndpointBehavior());

                foreach (OperationDescription op in endpoint.Contract.Operations)
                {
                    op.Behaviors.Add(new JsonOperationBehavior());
                }
            }*/

            Console.WriteLine("Running ...");

            Console.ReadLine();
        }

        class MyBehavior : IServiceBehavior
        {

            public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
            {
                return;
            }

            public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
            {
                return;
            }

            public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
            {
                return;
            }
        }

    }
}
