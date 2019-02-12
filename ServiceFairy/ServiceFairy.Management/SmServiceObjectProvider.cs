using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Management
{
    public class SmServiceObjectProvider : IServiceObjectProvider
    {
        public SmServiceObjectProvider(IServiceObjectTree tree)
        {
            Contract.Requires(tree != null);

            _tree = tree;
        }

        private readonly IServiceObjectTree _tree;

        public IServiceObjectTree GetTree()
        {
            return _tree;
        }

        public static IServiceObjectProvider Load(SfManagementContext mgrCtx)
        {
            return new SmServiceObjectProvider(SmServiceObjectTree.Load(mgrCtx));
        }
    }
}
