using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;

namespace ServiceFairy.Cluster
{
    [DataContract]
    class ClusterConfig
    {
        [DataMember]
        public string Title { get; set; }


        public static ClusterConfig FromXml(string xml)
        {
            return XmlUtility.Deserialize<ClusterConfig>(xml);
        }

        public static string ToXml(ClusterConfig cfg)
        {
            return XmlUtility.SerializeToString(cfg);
        }
    }
}
