using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using Common.Utility;

namespace Common.Data.Entities
{
    partial class DbEntity<TEntity>
	{
        /// <summary>
        /// 转换为实体类数组
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static TEntity[] FromDataList(DataList dataList)
        {
            Contract.Requires(dataList != null);

            TEntity[] entities = new TEntity[dataList.Rows.Length];
            for (int k = 0; k < entities.Length; k++)
            {
                entities[k] = FromDataList(dataList, k);
            }

            return entities;
        }

        /// <summary>
        /// 转换为实体类
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static TEntity FromDataList(DataList dataList, int index)
        {
            Contract.Requires(dataList != null);

            DataListRow r = dataList.Rows[index];
            TEntity entity = new TEntity();

            ColumnInfo[] colInfos = dataList.Columns;
            for (int k = 0, len = colInfos.Length; k < len; k++)
            {
                entity.SetValue(colInfos[k].Name, r.Cells[k], false);
            }

            return entity;
        }

        /// <summary>
        /// 转换为数据集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static DataList ToDataList(IList<TEntity> entities)
        {
            Contract.Requires(entities != null);

            DataListBuilder db = new DataListBuilder(TableName, PrimaryKeyColumnInfo.Name);
            db.AddColumns(AllColumnInfos);

            for (int k = 0, count = entities.Count; k < count; k++)
            {
                TEntity entity = entities[k];
                db.AddRow(_ToValueArray(entity));
            }

            return db.ToDataList();
        }

        /// <summary>
        /// 将指定的列转换为数据集合
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DataList ToDataList(IList<TEntity> entities, string[] columns)
        {
            if (columns == null)
                return ToDataList(entities);

            Contract.Requires(entities != null);

            DataListBuilder db = new DataListBuilder(FullTableName, PrimaryKeyColumnInfo.Name);
            db.AddColumns(GetColumnInfos(columns));

            for (int k = 0, count = entities.Count; k < count; k++)
			{
                TEntity entity = entities[k];
                db.AddRow(_ToValueArray(entity, columns));
			}

            return db.ToDataList();
        }

        /// <summary>
        /// 转换为数据集合
        /// </summary>
        /// <returns></returns>
        public DataList ToDataList()
        {
            return ToDataList(new TEntity[] { (TEntity)this });
        }

        // 将值转换为object数组
        private static object[] _ToValueArray(TEntity entity)
        {
            ReadOnlyCollection<ColumnInfo> cInfos = AllColumnInfos;
            object[] values = new object[cInfos.Count];

            for (int k = 0; k < values.Length; k++)
            {
                ColumnInfo cInfo = cInfos[k];
                values[k] = entity.GetValue(cInfo.Name);
            }

            return values;
        }

        // 将值转换为object数组
        private static object[] _ToValueArray(TEntity entity, string[] columns)
        {
            object[] values = new object[columns.Length];
            for (int k = 0; k < columns.Length; k++)
            {
                values[k] = entity.GetValue(columns[k]);
            }

            return values;
        }

        // 将单列数据转换为数据集
        private static DataList _ToDataList(string columnName, object[] values)
        {
            DataListBuilder db = new DataListBuilder(FullTableName, PrimaryKeyColumnInfo.Name);

            ColumnInfo ci = GetColumnInfo(columnName);
            db.AddColumn(ci);

            db.AddRowsForSingleColumn(values);
            return db.ToDataList();
        }

        // 将单列数据转换为数据集，带有路由键
        private static DataList _ToDataListWithRouteKey(string columnName, object[] values, object routeKey)
        {
            DataListBuilder db = new DataListBuilder(FullTableName, PrimaryKeyColumnInfo.Name);

            ColumnInfo ci = GetColumnInfo(columnName);
            db.AddColumn(ci);
            db.AddColumn(RouteKeyColumnInfo);

            db.AddRows(values.ToArray(v => new object[] { v, routeKey }));
            return db.ToDataList();
        }

        /// <summary>
        /// 将泛型数组转换为object数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static object[] _ToObjectArray<T>(T[] arr)
        {
            if (arr == null)
                return null;

            return arr.CastAsObject();
        }

        /// <summary>
        /// 如果指定的对象为空引用，则返回空引用，否则返回包含该对象的数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object[] _ToArray(object obj)
        {
            if (obj == null)
                return null;

            return new[] { obj };
        }

        /// <summary>
        /// 判断指定的值是否为空引用或DBNull
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool _IsEmpty(object value)
        {
            return value == null || value == DBNull.Value;
        }

        /// <summary>
        /// 转换为指定的类型，如果为空，则返回默认值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private static T _ConvertTo<T>(object value, T defaultValue = default(T))
        {
            if (_IsEmpty(value))
                return defaultValue;

            return value.ToTypeWithError<T>();
        }
	}
}
