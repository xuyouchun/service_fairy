using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfTest.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start ...");
            Console.ReadKey();

            InstanceContext instanceContext = new InstanceContext(new DuplexServiceCallback());
            
            using (DuplexChannelFactory<IDuplexService> factory = new DuplexChannelFactory<IDuplexService>(instanceContext,
                new NetTcpBinding(), new EndpointAddress("net.tcp://127.0.0.1:90/s")))
            {
                IDuplexService client = factory.CreateChannel();
                client.Add(1, 2);
                Console.WriteLine(client.Add2(3, 4));

                Console.ReadKey();
                Console.WriteLine("OK!");
            }

            /*
            string baseAddress = "http://127.0.0.1:8082/TestService";
            ICollection<BindingElement> bindingElements = new List<BindingElement>();
            HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
            JsonMessageEncodingBindingElement jsonBindingElement = new JsonMessageEncodingBindingElement(new BinaryMessageEncodingBindingElement());

            bindingElements.Add(jsonBindingElement);
            bindingElements.Add(httpBindingElement);
            CustomBinding customBinding = new CustomBinding(bindingElements);*/

            /*
            CustomBinding binding = new CustomBinding(new BindingElement[]{
                new JsonMessageEncodingBindingElement(new BinaryMessageEncodingBindingElement()),
                new TcpTransportBindingElement(),
            });

            string address = "net.tcp://127.0.0.1:8083/TestService2";

            ITestService svc = ChannelFactory<ITestService>.CreateChannel(binding, new EndpointAddress(address));
            //Message.CreateMessage(MessageVersion.Soap12, "http://tempuri.org/ITestService/Operate")
            //Message output = svc.Operate(new EntityMessage(new Input(1, 2), "s"));
            Output output = svc.Operate(new Input(1, 2));*/

            Console.ReadLine();
        }
    }
}
