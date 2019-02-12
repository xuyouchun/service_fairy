using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Storage
{
    abstract class AppCommandBase<TRequest, TReply> : AppCommandBaseEx<Service, TRequest, TReply>
        where TRequest : RequestEntity
        where TReply : ReplyEntity
    {

    }
}
