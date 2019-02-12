using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace ServiceFairy.Management.Database
{
    /// <summary>
    /// 数据库树
    /// </summary>
    public class DatabaseServiceObjectTree : IServiceObjectTree
    {
        public IServiceObjectTreeNode Root
        {
            get { throw new NotImplementedException(); }
        }
    }
}
