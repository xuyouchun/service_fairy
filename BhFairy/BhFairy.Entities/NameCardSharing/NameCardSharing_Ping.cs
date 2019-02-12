using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using System.Runtime.Serialization;

namespace BhFairy.Entities
{
    [Serializable, DataContract]
    public class NameCardSharing_Ping_Request : RequestEntity
    {
    }

    [Serializable, DataContract]
    public class NameCardSharing_Ping_Reply : ReplyEntity
    {
    }
}
