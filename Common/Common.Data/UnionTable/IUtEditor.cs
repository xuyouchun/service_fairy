using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics.Contracts;
using Common.Data.Entities;
using Common.Collection;
using Common.Utility;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 编辑器
    /// </summary>
    public interface IUtEditor
    {
        /// <summary>
        /// 执行元数据表的初始化
        /// </summary>
        /// <param name="conPoint">连接点</param>
        /// <returns>是否有改动</returns>
        bool InitMetaDataSchema();

        /// <summary>
        /// 修正表结构
        /// </summary>
        /// <param name="conPoint">连接点</param>
        /// <param name="tables">表的修正信息</param>
        /// <returns>是否有改动</returns>
        bool ReviseTableSchema(UtTableReviseInfo[] tables);
    }

    /// <summary>
    /// 表的修正信息
    /// </summary>
    [Serializable, DataContract]
    public class UtTableReviseInfo
    {
        public UtTableReviseInfo()
        {

        }

        /// <summary>
        /// 表名
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        /// 默认组名
        /// </summary>
        [DataMember]
        public string DefaultGroup { get; set; }

        /// <summary>
        /// 主键列信息
        /// </summary>
        [DataMember]
        public ColumnInfo PrimaryKeyColumnInfo { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        [DataMember]
        public MtColumn[] Columns { get; set; }

        /// <summary>
        /// 表的初始数量
        /// </summary>
        [DataMember]
        public int InitTableCount { get; set; }

        public override int GetHashCode()
        {
            return CommonUtility.GetHashCode(new object[] { TableName, DefaultGroup, PrimaryKeyColumnInfo, Columns, InitTableCount });
        }
    }

    /// <summary>
    /// 表组的修正信息
    /// </summary>
    [Serializable, DataContract]
    public class UtTableGroupReviseInfo
    {
        /// <summary>
        /// 表组信息
        /// </summary>
        [DataMember]
        public string TableGroupName { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        [DataMember]
        public ColumnInfo RouteKeyColumnInfo { get; set; }

        /// <summary>
        /// 路由类型
        /// </summary>
        [DataMember]
        public DbRouteType RouteType { get; set; }

        /// <summary>
        /// 路由参数
        /// </summary>
        [DataMember]
        public string RouteArgs { get; set; }

        /// <summary>
        /// 表修正信息
        /// </summary>
        [DataMember]
        public UtTableReviseInfo[] ReviseInfos { get; set; }

        public override int GetHashCode()
        {
            return CommonUtility.GetHashCode(new object[] { TableGroupName, RouteKeyColumnInfo, RouteType, RouteArgs, ReviseInfos });
        }

        /// <summary>
        /// 从程序集中加载修正信息
        /// </summary>
        /// <param name="assemblies">程序集</param>
        /// <returns></returns>
        public static UtTableGroupReviseInfo[] LoadFromAssembly(Assembly[] assemblies)
        {
            Contract.Requires(assemblies != null);

            List<UtTableGroupReviseInfo> infos = new List<UtTableGroupReviseInfo>();
            foreach (Assembly assembly in assemblies)
            {
                infos.AddRange(LoadFromAssembly(assembly));
            }

            return Combine(infos);
        }

        /// <summary>
        /// 从程序集中加载修正信息
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static UtTableGroupReviseInfo[] LoadFromAssembly(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            List<UtTableGroupReviseInfo> infos = new List<UtTableGroupReviseInfo>();
            foreach (Type entityType in DbEntity.LoadEntityTypesFrom(assembly))
            {
                DbEntity entity = Activator.CreateInstance(entityType) as DbEntity;
                if (entity != null)
                {
                    DbEntityTableInfo tableInfo = entity.GetTableInfo();
                    UtTableGroupReviseInfo info = new UtTableGroupReviseInfo {
                        TableGroupName = tableInfo.TableGroupName,
                        RouteType = tableInfo.RouteType,
                        RouteArgs = tableInfo.RouteArgs,
                        RouteKeyColumnInfo = tableInfo.RouteKeyColumnInfo,
                        ReviseInfos = new [] {
                            new UtTableReviseInfo() {
                                TableName = tableInfo.TableName,
                                DefaultGroup = tableInfo.DefaultGroup,
                                Columns = entity.GetColumns().ToArray(),
                                InitTableCount = tableInfo.InitTableCount,
                                PrimaryKeyColumnInfo = tableInfo.PrimaryKeyColumnInfo,
                            },
                        },
                    };

                    infos.Add(info);
                }
            }

            return Combine(infos.ToArray());
        }

        public static UtTableGroupReviseInfo[] Combine(IList<UtTableGroupReviseInfo> groups)
        {
            Contract.Requires(groups != null);

            var list = from g in groups.GroupBy(item => item.TableGroupName, IgnoreCaseEqualityComparer.Instance)
                       let tableGroupName = g.Key
                       let ri = g.First()
                       let ris = g.SelectMany(v => v.ReviseInfos)
                       select new UtTableGroupReviseInfo { TableGroupName = ri.TableGroupName, ReviseInfos = ris.ToArray(), 
                           RouteType = ri.RouteType, RouteKeyColumnInfo = ri.RouteKeyColumnInfo, RouteArgs = ri.RouteArgs
                       };

            return list.ToArray();
        }
    }
}
