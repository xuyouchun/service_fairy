using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 表连接
    /// </summary>
    public interface IUtConnection
    {
        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="routeKeys">路由键</param>
        /// <param name="columns">字段</param>
        /// <param name="param">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <remarks>
        /// 1、如果routeKeys为空引用，则到各分表中按条件查找数据，否则限定routeKeys所在的分表
        /// 2、如果查询条件param为空引用，则查询routeKeys的全部数据
        /// 3、如果routeKeys或param都为空，则返回空数据集
        /// </remarks>
        /// <returns></returns>
        DataList Select(object[] routeKeys, string[] columns, DbSearchParam param, UtInvokeSettings settings = null);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data">需要插入的数据</param>
        /// <param name="autoUpdate">当数据已经存在时，是否将其更新</param>
        /// <param name="settings">调用设置</param>
        /// <remarks>
        /// 1、如果路由键与主键不同，则必须指定路由键
        /// 2、主键不指定时，则自动按规则填入主键，有可能是自增整型或guid
        /// </remarks>
        /// <returns>插入的行数</returns>
        int Insert(DataList data, bool autoUpdate, UtInvokeSettings settings = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">需要更新的数据</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <remarks>
        /// 1、如果data中不含有路由键，则只能包含一行，此时将按条件搜索所有的分表进行更新，如果查询条件为空，则所有的记录都被更新
        /// 2、如果data中含有路由键，则可以包含多行
        /// 3、如果routeKeys不为空，则data中只能包含一行，并且不允许包含路由键，此时将更新指定路由键中的行
        /// 4、主键与路由键不可更新，如果包含，则将其作为过滤条件
        /// </remarks>
        /// <returns>更新的行数</returns>
        int Update(DataList data, object[] routeKeys, string where, UtInvokeSettings settings = null);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data">需要删除的数据</param>
        /// <param name="where">查询条件</param>
        /// <param name="settings">调用设置</param>
        /// <remarks>
        /// 1、如果data为空引用，则在各个分表中搜索符合查询条件的记录，如果查询条件为空，则所有的记录都将被删除
        /// 2、如果data不为空，则所有记录都需要包含路由键，此时将按路由键在各分表中执行删除
        /// 3、data中的所有字段都将作为删除的过滤条件
        /// </remarks>
        /// <returns>删除的行数</returns>
        int Delete(DataList data, string where, UtInvokeSettings settings = null);

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="routeKey">路由键</param>
        /// <param name="data">需要合并的数据</param>
        /// <param name="compareColumns">用于比较的字段</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选面</param>
        /// <param name="settings">调用设置</param>
        /// <remarks>
        /// 1、路由键必须由routeKey指定，参数data中的路由键可以省略，如果没有省略，则必须与routeKey相同
        /// 2、compareColumns指定用于比较的列，如果省略，则默认为主键比较，如果主键未指定，则出错
        /// 3、mergeColumns不可以为空，并且所有的列都必须在data中包含
        /// 4、where可以不指定，如果指定，则与routeKey共同选出需要合并的数据
        /// </remarks>
        /// <returns></returns>
        int Merge(object routeKey, DataList data, string[] compareColumns, string where,
            UtConnectionMergeOption option, UtInvokeSettings settings = null);
    }

    /// <summary>
    /// 合并选项
    /// </summary>
    [Flags]
    public enum UtConnectionMergeOption
    {
        None,

        /// <summary>
        /// 插入不存在的数据
        /// </summary>
        Insert = 0x01,

        /// <summary>
        /// 更新已经存在的数据
        /// </summary>
        Update = 0x02,

        /// <summary>
        /// 删除源表中没有的数据
        /// </summary>
        Delete = 0x04,

        /// <summary>
        /// 插入并更新
        /// </summary>
        InsertUpdate = Insert | Update,

        /// <summary>
        /// 插入更新删除
        /// </summary>
        All = Insert | Update | Delete,
    }
}
