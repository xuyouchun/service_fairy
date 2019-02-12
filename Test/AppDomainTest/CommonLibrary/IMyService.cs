using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public interface IMyService
    {
        string Send(string aaa);

        IHelper Helper { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    //[Serializable]
    public class MyEventArgs : EventArgs
    {
        public string Data;
    }
}
