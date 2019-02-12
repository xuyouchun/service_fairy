using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Algorithms;

namespace CommonTest
{
    class ConsistentHashing
    {
        public static void Execute()
        {
            ConsistentHashing<ConsistentNode> ch = new ConsistentHashing<ConsistentNode>();

            Guid id = Guid.NewGuid();
            ConsistentNode node = new ConsistentNode(id);
            ch.AddNodes(new[] { node, new ConsistentNode(Guid.NewGuid()) });
            ch.RemoveNode(node);

            for (int k = 0; k < 10; k++)
            {

            }
        }
    }

    public class ConsistentNode : IConsistentNode
    {
        public ConsistentNode(Guid clientId)
        {
            _clientId = clientId;
        }

        private readonly Guid _clientId;

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
            return _clientId.CompareTo(((ConsistentNode)other)._clientId);
        }

        public override string ToString()
        {
            return _clientId.ToString();
        }
    }
}
