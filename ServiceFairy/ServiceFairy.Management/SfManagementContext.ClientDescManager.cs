using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using ServiceFairy.SystemInvoke;
using Common.Package;
using Common;
using Common.Utility;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management
{
	partial class SfManagementContext
	{
        public class ClientDescManager : ManagementBase<ClientDescManager.Wrapper>
        {
            public ClientDescManager(SfManagementContext ctx)
                : base(ctx)
            {

            }

            public class Wrapper
            {
                public Dictionary<Guid, ClientDesc> Dict;
                public ClientDesc[] ClientDescs;
            }

            protected override ClientDescManager.Wrapper OnLoad()
            {
                ClientDesc[] clientDescs = MgrCtx.Invoker.Master.GetAllClientDesc() ?? Array<ClientDesc>.Empty;
                Array.Sort(clientDescs, (x, y) => x.ToString().CompareTo(y.ToString()));
                return new Wrapper() {
                    Dict = clientDescs.ToDictionary(cd => cd.ClientID),
                    ClientDescs = clientDescs,
                };
            }

            /// <summary>
            /// 获取所有终端描述
            /// </summary>
            /// <returns></returns>
            public ClientDesc[] GetAll()
            {
                return Value.ClientDescs;
            }

            /// <summary>
            /// 获取指定终端的描述
            /// </summary>
            /// <param name="clientId"></param>
            /// <returns></returns>
            public ClientDesc Get(Guid clientId)
            {
                return Value.Dict.GetOrDefault(clientId);
            }

            /// <summary>
            /// 是否包含指定的终端
            /// </summary>
            /// <param name="clientId"></param>
            /// <returns></returns>
            public bool Exist(Guid clientId)
            {
                return Get(clientId) != null;
            }

            /// <summary>
            /// 获取指定终端的标题
            /// </summary>
            /// <param name="clientId"></param>
            /// <returns></returns>
            public string GetClientTitle(Guid clientId)
            {
                return Get(clientId).ToStringIgnoreNull(clientId.ToString());
            }
        }
	}
}
