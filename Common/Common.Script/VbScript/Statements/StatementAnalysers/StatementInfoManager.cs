using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 语句块信息管理器
    /// </summary>
    static class StatementInfoManager
    {
        static StatementInfoManager()
        {
            foreach (Type type in typeof(StatementInfoManager).Assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(StatementAttribute), false);
                if (attrs.Length > 0)
                {
                    StatementAttribute attr = (StatementAttribute)attrs[0];
                    if (attr.CreatorType == null)
                        continue;

                    StatementTypeAttribute typeAttr = _GetTypeAttr(attr.StatementType);
                    if (typeAttr == null)
                        continue;

                    StatementInfo statementInfo = new StatementInfo(typeAttr.Name, type,
                        (IStatementCreator)Activator.CreateInstance(attr.CreatorType));

                    _Dict.Add(attr.StatementType, statementInfo);
                }
            }
        }

        private static StatementTypeAttribute _GetTypeAttr(StatementType statementType)
        {
            object[] attrs = typeof(StatementType).GetField(statementType.ToString()).GetCustomAttributes(typeof(StatementTypeAttribute), false);
            if (attrs.Length == 0)
                return null;

            return (StatementTypeAttribute)attrs[0];
        }

        static readonly Dictionary<StatementType, StatementInfo> _Dict = new Dictionary<StatementType, StatementInfo>();

        /// <summary>
        /// 根据语句块名称获取语句块的信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static StatementInfo GetInfo(StatementType statementType)
        {
            StatementInfo info;
            _Dict.TryGetValue(statementType, out info);

            return info;
        }
    }
}
