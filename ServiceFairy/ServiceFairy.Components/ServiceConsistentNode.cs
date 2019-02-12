using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Algorithms;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 用于一致性哈希算法的服务节点
    /// </summary>
    public class ServiceConsistentNode : IConsistentNode
    {
        public ServiceConsistentNode(Guid clientId)
        {
            _clientId = clientId;
        }

        private readonly Guid _clientId;

        /// <summary>
        /// 服务
        /// </summary>
        public Guid ClientID { get { return _clientId; } }

        private const string _ss = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";

        public int[] GetVirtualNodeHashCodes()
        {
            int[] hashcodes = new int[17];
            for (int k = 0, len = hashcodes.Length; k < len; k++)
            {
                hashcodes[k] = _clientId.GetHashCode() ^ (_ss.Substring(k + 1, (k + 1) * 2)).ToString().GetHashCode();
            }

            return hashcodes;
        }

        public int CompareTo(IConsistentNode other)
        {
            return _clientId.CompareTo(((ServiceConsistentNode)other)._clientId);
        }

        public override string ToString()
        {
            return _clientId.ToString();
        }
    }
}
