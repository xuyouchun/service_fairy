using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common.Contracts;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Contracts.Entities;
using System.Runtime.Serialization;
using Common.Utility;
using System.Diagnostics;

namespace TestDev
{
    class Program
    {
        static readonly WcfService _wcfService = new WcfService();

        static void Main(string[] args)
        {
            using (WcfConnection con = _wcfService.Connect(ServiceAddress.Parse("127.0.0.1:8090"), CommunicationType.Tcp))
            {
                con.Open();
                SFClient sf = new SFClient(con, DataFormat.Binary);

                SystemInvoker invoker = new SystemInvoker(sf);
                Guid[] guids = CollectionUtility.GenerateArray(10000, k => Guid.NewGuid());

                Stopwatch sw = Stopwatch.StartNew();
                for (int k = 0; k < guids.Length; k++)
                {
                    string routePath = guids[k].ToString();
                    invoker.Cache.SetRange("System.Share/MyLocation_" + routePath, new Dictionary<string, MyClass> {
                        { "[100,200]/12345", new MyClass("1111", 11111) },
                        { "[100,200]/12346", new MyClass("1112", 11112) },
                    }, TimeSpan.FromMinutes(10));
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);

                /*
                sw.Restart();
                for (int k = 0; k < 10; k++)
                {
                    string routePath = guids[k].ToString();
                    string key = string.Format("System.Share/MyLocation_{0}:[100,200]/*", routePath);
                    invoker.Cache.Remove(key);
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);*/

                sw.Restart();
                for (int k = 0; k < guids.Length; k++)
                {
                    string routePath = guids[k].ToString();
                    string key = string.Format("System.Share/MyLocation_{0}:[100,200]/*", routePath);
                    string[] keys = invoker.Cache.GetKey(key);
                    //Console.WriteLine("==:" + string.Join(",", (object[])keys));
                    //MyClass[] mcs = invoker.Cache.GetRange<MyClass>(key);

                    //Console.WriteLine("==: " + string.Join(", ", (object[])mcs));
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
        }
    }

    [Serializable, DataContract]
    class MyClass
    {
        public MyClass(string stringValue, int intValue)
        {
            StringValue = stringValue.PadLeft(1024, '*');
            IntValue = intValue;
        }

        [DataMember]
        public string StringValue { get; set; }

        [DataMember]
        public int IntValue { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", StringValue, IntValue);
        }
    }
}
