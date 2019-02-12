using System;
using System.Data;
namespace Common.Data
{
    /// <summary>
    /// 数据查询器接口
    /// </summary>
    public interface IDbQuery : IDisposable
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        ITranscation BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted);

        /// <summary>
        /// 执行SQL并返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null);

        /// <summary>
        /// 执行SQL并返回第一个DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null);

        /// <summary>
        /// 执行SQL并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null);

        /// <summary>
        /// 执行SQL并返回DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="behavior"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, ITranscation transcation = null);

        /// <summary>
        /// 执行SQL并返回第一行第一列的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transcation"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, DbQueryParam[] parameters = null, CommandType commandType = CommandType.Text, ITranscation transcation = null);
    }
}
