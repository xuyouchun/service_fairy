using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable.Metadata;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 元数据管理器
    /// </summary>
    public interface IMetadataManager
    {
        /// <summary>
        /// 加载元数据
        /// </summary>
        MtDatabase LoadMetaData();

        /// <summary>
        /// 添加一个连接点
        /// </summary>
        /// <param name="conPoint"></param>
        /// <param name="throwError"></param>
        bool AddConnectionPoint(MtConnectionPoint conPoint, bool throwError = false);

        /// <summary>
        /// 添加一个物理连接
        /// </summary>
        /// <param name="con"></param>
        /// <param name="throwError"></param>
        bool AddConnection(MtConnection con, bool throwError = false);

        /// <summary>
        /// 删除一个物理连接
        /// </summary>
        /// <param name="name"></param>
        /// <param name="throwError"></param>
        bool DropConnection(string name, bool throwError = false);

        /// <summary>
        /// 添加一个组
        /// </summary>
        /// <param name="group"></param>
        /// <param name="throwError"></param>
        bool AddTableGroup(MtTableGroup group, bool throwError = false);

        /// <summary>
        /// 添加表
        /// </summary>
        /// <param name="tableGroupName">表组名</param>
        /// <param name="table">表</param>
        /// <param name="throwError"></param>
        void AddTable(string tableGroupName, MtTable table, bool throwError = false);
    }
}
