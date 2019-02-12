using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;

namespace ClassLibrary1
{
    public class Class1 : MarshalByRefObject, IMyService
    {
        public string Send(string aaa)
        {
            Helper.Send(aaa);

            return "YEAH!";
        }

        public IHelper Helper
        {
            get { return _helper; }
        }

        private readonly Helper _helper = new Helper();
    }

    /// <summary>
    /// 
    /// </summary>
    class Helper : MarshalByRefObject, IHelper
    {
        public void Send(string a)
        {
            var eh = Received;
            if (eh != null)
                eh(this, new MyEventArgs() { Data = "Hello World" });
        }

        public event EventHandler<MyEventArgs> Received;
    }
}
