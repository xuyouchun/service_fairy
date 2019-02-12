using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceFairy.SystemInvoke;
using Common.Communication.Wcf;
using Common.Package;
using ServiceFairy.Entities.Master;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities.Sys;
using Common.Contracts;
using Common;

namespace ServiceFairy.Management.Web.Core
{
    public class JCoreInvoker : SystemInvoker
    {
        private JCoreInvoker(IServiceClientProvider scp)
            : base(scp)
        {
            _clientDict = new AutoLoad<Dictionary<Guid, JAppClient>>(_LoadAppClients, TimeSpan.FromSeconds(30));
            _serviceDict = new AutoLoad<Dictionary<ServiceDesc, List<Guid>>>(_LoadAppServices, TimeSpan.FromSeconds(30));
        }

        private readonly AutoLoad<Dictionary<Guid, JAppClient>> _clientDict;
        private readonly AutoLoad<Dictionary<ServiceDesc, List<Guid>>> _serviceDict;

        private Dictionary<Guid, JAppClient> _LoadAppClients()
        {
            ClientDesc[] clientDescs = Master.GetAllClientDesc();

            return clientDescs.ToDictionary(cd => cd.ClientID, cd => new JAppClient(this, cd));
        }

        public Dictionary<ServiceDesc, List<Guid>> _LoadAppServices()
        {
            var dict = new Dictionary<ServiceDesc, List<Guid>>();
            foreach (JAppClient client in _clientDict.Value.Values)
            {
                foreach (ServiceDesc sd in client.GetServices())
                {
                    dict.GetOrSet(sd).Add(client.ClientDesc.ClientID);
                }
            }

            return dict;
        }

        public JAppServiceDoc[] GetAppServices()
        {
            List<JAppServiceDoc> docs = new List<JAppServiceDoc>();
            foreach (ServiceDesc serviceDesc in _serviceDict.Value.Keys)
            {
                docs.Add(new JAppServiceDoc { Name = serviceDesc.Name, Version = serviceDesc.Version.ToString() });
            }

            return docs.ToArray();
        }

        public JAppCommand[] GetAppCommands(ServiceDesc serviceDesc)
        {
            AppCommandInfo[] cmdInfos = Sys.GetAppCommandInfos(Guid.Empty, serviceDesc);
            JAppCommand[] cmds = cmdInfos.ToArray(ci => new JAppCommand(ci.CommandDesc.Name,
                ci.CommandDesc.Version.ToString(), ci.Title, ci.Desc, ci.Category == AppCommandCategory.System,
                _GetTypeName(ci.InputParameter), _GetTypeName(ci.OutputParameter), (int)ci.SecurityLevel, ci.SecurityLevel.GetDesc(),
                ci.Usable, ci.UsableDesc));

            Array.Sort(cmds);
            return cmds;
        }

        private static string _GetTypeName(AppParameter p)
        {
            return p == null ? null : p.Name;
        }

        public JAppMessage[] GetAppMessages(ServiceDesc serviceDesc)
        {
            AppMessageInfo[] msgInfos = Sys.GetAppMessageInfos(Guid.Empty, serviceDesc);
            JAppMessage[] msgs = msgInfos.ToArray(mi => new JAppMessage(mi.MessageDesc.Name, mi.MessageDesc.Version, mi.Title, mi.Desc, _GetTypeName(mi.Data)));
            Array.Sort(msgs);
            return msgs;
        }

        public AppCommandDoc GetAppCommandDoc(ServiceDesc sd, CommandDesc cd)
        {
            return Sys.GetAppCommandDoc(Guid.Empty, sd, cd, (DataFormat[])null);
        }

        public AppMessageDoc GetAppMessageDoc(ServiceDesc sd, MessageDesc md)
        {
            return Sys.GetAppMessageDoc(md, (DataFormat[])null, Guid.Empty, sd);
        }

        public JAppStatusCodeDoc[] GetAppStatusCodeDocs(ServiceDesc sd)
        {
            AppStatusCodeInfo[] infos = Sys.GetAppStatusCodeInfos(Guid.Empty, sd);
            return infos.ToArray(info => { var doc = JAppStatusCodeDoc.From(info, sd.Name); return doc; });
        }

        public TypeDoc[] GetEntityDocs(ServiceDesc sd)
        {
            return _GetEntityDocs(sd).Distinct(v => v.Name).OrderBy(v => v.Name, TypeNameComparer.Instance).ToArray();
        }

        private IEnumerable<TypeDoc> _GetEntityDocs(ServiceDesc sd)
        {
            AppCommandInfo[] cmdInfos = Sys.GetAppCommandInfos(Guid.Empty, sd);
            foreach (AppCommandInfo info in cmdInfos)
            {
                if (info.Category == AppCommandCategory.System)
                    continue;

                AppCommandDoc cmdDoc = Sys.GetAppCommandDoc(Guid.Empty, sd, info.CommandDesc, Array<DataFormat>.Empty);
                foreach (AppArgumentData argData in new[] { cmdDoc.Input, cmdDoc.Output })
                {
                    if (argData != null && argData.DocTree != null && argData.DocTree.SubTypeDocs != null)
                    {
                        foreach (TypeDoc typeDoc in argData.DocTree.SubTypeDocs)
                        {
                            yield return typeDoc;
                        }
                    }
                }
            }

            foreach (AppMessageInfo msg in Sys.GetAppMessageInfos(Guid.Empty, sd))
            {
                AppMessageDoc msgDoc = Sys.GetAppMessageDoc(msg.MessageDesc, Array<DataFormat>.Empty, Guid.Empty, sd);
                if (msgDoc.Data != null && msgDoc.Data.DocTree != null && msgDoc.Data.DocTree.SubTypeDocs != null)
                {
                    foreach (TypeDoc typeDoc in msgDoc.Data.DocTree.SubTypeDocs)
                    {
                        yield return typeDoc;
                    }
                }
            }
        }

        public static JCoreInvoker Instance
        {
            get { return _instance.Value; }
        }

        private static JCoreInvoker _Create()
        {
            SystemConnection sysCon = new SystemConnection(CommunicationOption.Parse(AppSettings.NavigationUrl));
            JCoreInvoker invoker = new JCoreInvoker(ServiceClientProviderDecorate.ToSecurityDecorate(sysCon));
            invoker.Security.Login(AppSettings.UserName, AppSettings.Password);

            return invoker;
        }

        private static readonly Lazy<JCoreInvoker> _instance = new Lazy<JCoreInvoker>(_Create, true);

        #region Class TypeNameComparer ...

        class TypeNameComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == null || y == null)
                    return string.Compare(x, y);

                string xName, xPostfix, yName, yPostfix;
                if (_Split(x, out xName, out xPostfix) && _Split(y, out yName, out yPostfix) && xName == yName)
                {
                    return string.Compare(yPostfix, xPostfix);
                }

                return string.Compare(x, y);
            }

            private static bool _Split(string s, out string name, out string postfix)
            {
                int index = s.LastIndexOf('_');
                if (index < 0)
                    goto _false;

                name = s.Substring(0, index);
                postfix = s.Substring(index + 1);
                return postfix == "Request" || postfix == "Reply";

            _false:
                name = postfix = null;
                return false;
            }

            public static readonly TypeNameComparer Instance = new TypeNameComparer();
        }

        #endregion

    }
}