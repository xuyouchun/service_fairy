using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace WcfTest
{
    [ServiceContract(CallbackContract = typeof(IDuplexServiceCallback))]
    public interface IDuplexService
    {
        [OperationContract(IsOneWay = true)]
        void Add(int x, int y);

        [OperationContract]
        int Add2(int x, int y);
    }


    public interface IDuplexServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Display(int result);
    }


    public class DuplexService : IDuplexService
    {
        public void Add(int x, int y)
        {
            int result = x + y;
            OperationContext ctx = OperationContext.Current;

            ThreadPool.QueueUserWorkItem(delegate {

                for (int k = 0; k < 10000; k++)
                {
                    Thread.Sleep(10 * 1000);

                    IDuplexServiceCallback callback = ctx.GetCallbackChannel<IDuplexServiceCallback>();
                    callback.Display(k);
                }
            });
        }

        public int Add2(int x, int y)
        {
            return x + y;
        }
    }

    public class DuplexServiceCallback : IDuplexServiceCallback
    {
        public void Display(int result)
        {
            Console.WriteLine("Result: " + result);
        }
    }

}
