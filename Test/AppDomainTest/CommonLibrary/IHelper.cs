using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public interface IHelper
    {
        void Send(string a);

        event EventHandler<MyEventArgs> Received;
    }
}
