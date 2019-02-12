using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Entities;
using System.Diagnostics.Contracts;
using Common.Package.Storage;
using System.IO;
using Common.Package;
using Common.Utility;
using Common.Algorithms;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 基于StreamTable的手机归属地数据
    /// </summary>
    public class StreamTablePhoneAreaDataProvider : IPhoneAreaDataProvider
    {
        public StreamTablePhoneAreaDataProvider(string basePath)
        {
            Contract.Requires(basePath != null);

            _basePath = basePath;
        }

        private readonly string _basePath;
        private static readonly TimeSpan _cacheTime = TimeSpan.FromSeconds(30);

        private readonly StreamTableFileManager _stFileMgr = new StreamTableFileManager();
        private readonly Cache<object, object> _cache = new Cache<object, object>();

        private string _GetFilePath(string stName)
        {
            return Path.Combine(_basePath, stName + ".st");
        }

        private StreamTable _GetStreamTable(string tablePath)
        {
            int index = tablePath.LastIndexOf('/');
            if (index < 0)
                throw new FormatException("未指定表名");

            string realPath = tablePath.Substring(0, index);
            string file = _GetFilePath(realPath), tableName = tablePath.Substring(index + 1);
            if (File.Exists(file))
            {
                StreamTableReader r = _stFileMgr.Load(file);
                StreamTable st = r.GetTable(tableName);
                if (st != null)
                    return st;
            }

            index = tableName.IndexOf(':');
            if (index >= 0)
            {
                string prefix = tableName.Substring(0, index), name = tableName.Substring(index + 1);
                file = _GetFilePath(realPath + "_" + prefix);
                if (File.Exists(file))
                {
                    StreamTableReader r = _stFileMgr.Load(file);
                    StreamTable st = r.GetTable(name);
                    if (st != null)
                        return st;
                }
            }

            throw new PhoneAreaQueryException(string.Format("未从文件“{0}”中找到表“{1}”", file, tableName));
        }

        private Dictionary<object, object> _GetCountryDict(string nationalCode)
        {
            return (Dictionary<object, object>)_cache.GetOrAddOfDynamic(nationalCode, _cacheTime, (key) => new Dictionary<object, object>());
        }

        // 根据mnc查询运营商
        public OperationInfo GetOperationInfo(string nationalCode, string mnc)
        {
            OperationRelation[] relations = _GetCountryDict(nationalCode).GetOrSet("mnc", (key) => _LoadMncDict(nationalCode)) as OperationRelation[];
            int index = Algorithm.BinarySearch<OperationRelation, ushort>(relations, ushort.Parse(mnc), (item) => item.Mnc, SortType.Asc);
            if (index < 0)
                throw new PhoneAreaQueryException(string.Format("未找到MNC={0}的运营商", mnc));

            int opIndex = relations[index].OpIndex;
            return GetOperationInfos(nationalCode)[opIndex];
        }

        struct OperationRelation
        {
            public ushort Mnc, OpIndex;
        }

        private OperationRelation[] _LoadMncDict(string nationalCode)
        {
            OperationInfo[] opInfos = GetOperationInfos(nationalCode);

            StreamTable st = _GetStreamTable(nationalCode + "/mnc");
            OperationRelation[] relations = new OperationRelation[st.RowCount];
            for (int k = 0; k < relations.Length; k++)
            {
                StreamTableRow row = st[k];
                relations[k] = new OperationRelation { Mnc = row.Get<ushort>("mnc"), OpIndex = row.Get<ushort>("op_index") };
            }

            return relations;
        }

        // 获取所有运营商信息，按InternalPrefix的长度逆序排列
        public OperationInfo[] GetOperationInfos(string nationalCode)
        {
            return _GetCountryDict(nationalCode).GetOrSet("operation_info", (key) => _LoadOperationInfos(nationalCode)) as OperationInfo[];
        }

        private OperationInfo[] _LoadOperationInfos(string nationalCode)
        {
            StreamTable st = _GetStreamTable(nationalCode + "/operation");
            OperationInfo[] infos = new OperationInfo[st.RowCount];
            for (int k = 0, length = st.RowCount; k < length; k++)
            {
                StreamTableRow row = st.GetRow(k);
                infos[k] = new OperationInfo((string)row["national_prefix"], (string)row["internal_prefix"], (string)row["desc"]);
            }

            return infos;
        }

        // 获取所有国际长途前缀，按升序排序
        public NationalCodes GetNationalCodes()
        {
            return _cache.GetOrAddOfDynamic("national_code", _cacheTime, (key) => _LoadNationalCodes()) as NationalCodes;
        }

        private NationalCodes _LoadNationalCodes()
        {
            StreamTable cfgTable = _GetStreamTable("national_code/config");
            StreamTableRow cfgRow = cfgTable[0];
            int minLength = cfgRow.Get<int>("national_code_min_length");
            int maxLength = cfgRow.Get<int>("national_code_max_length");

            StreamTable nationalCodeTable = _GetStreamTable("national_code/national_code");
            return new NationalCodes(minLength, maxLength, new NationalCodeList(nationalCodeTable));
        }

        #region Class NationalCodeList ...

        class NationalCodeList : ListBase<ushort>
        {
            public NationalCodeList(StreamTable st)
            {
                _st = st;
            }

            private readonly StreamTable _st;

            protected override ushort Get(int index)
            {
                return _st[index].Get<ushort>("national_code");
            }

            protected override int GetCount()
            {
                return _st.RowCount;
            }
        }

        #endregion

        public OperationPrefixes GetOperationPrefixes(string nationalCode)
        {
            return (OperationPrefixes)_GetCountryDict(nationalCode).GetOrSet("operation_prefixes", (key) => _LoadOperationPrefixes(nationalCode));
        }

        // 读取运营商前缀表，按prefix升序排序
        private OperationPrefixes _LoadOperationPrefixes(string nationalCode)
        {
            StreamTable configTable = _GetStreamTable(nationalCode + "/config");
            StreamTableRow configRow = configTable[0];
            int minLength = configRow.Get<int>("op_prefix_min_length"), maxLength = configRow.Get<int>("op_prefix_max_length");

            StreamTable opPrefixTable = _GetStreamTable(nationalCode + "/op_prefix");
            return new OperationPrefixes(minLength, maxLength, new OperationPrefixList(opPrefixTable, GetOperationInfos(nationalCode)));
        }

        #region Class OperationPrefixList ...

        class OperationPrefixList : ListBase<OperationPrefix>
        {
            public OperationPrefixList(StreamTable st, OperationInfo[] opInfos)
            {
                _st = st;
                _opInfos = opInfos;
                _prefixes = new OperationPrefix[st.RowCount];
            }

            private readonly StreamTable _st;
            private readonly OperationInfo[] _opInfos;
            private readonly OperationPrefix[] _prefixes;

            protected override OperationPrefix Get(int index)
            {
                return _prefixes[index] ?? (_prefixes[index] = _Get(index));
            }

            private OperationPrefix _Get(int index)
            {
                StreamTableRow row = _st[index];
                return new OperationPrefix((string)row["op_prefix"], _opInfos[row.Get<ushort>("op_index")]);
            }

            protected override int GetCount()
            {
                return _st.RowCount;
            }
        }

        #endregion

        public AreaDatas GetAreaDatas(string nationalCode, OperationInfo opInfo, string opPrefix)
        {
            string file = nationalCode + "/" + opPrefix;

            StreamTable configTable = _GetStreamTable(file + ":config");
            StreamTableRow configRow = configTable[0];
            int minLength = configRow.Get<int>("area_code_min_length");
            int maxLength = configRow.Get<int>("area_code_max_length");
            AreaDataModel areaDataModel = (AreaDataModel)Enum.Parse(typeof(AreaDataModel), configRow.Get<string>("storage_model"), true);

            StreamTable areaCodeTable = _GetStreamTable(file + ":area_code");
            return new AreaDatas(areaDataModel, minLength, maxLength, new AreaDataList(areaCodeTable));
        }

        #region Class AreaDataList ...

        class AreaDataList : ListBase<AreaData>
        {
            public AreaDataList(StreamTable st)
            {
                _st = st;
                _areas = new AreaData[st.RowCount];
            }

            private readonly StreamTable _st;
            private readonly AreaData[] _areas;

            protected override AreaData Get(int index)
            {
                return _areas[index] ?? (_areas[index] = _Get(index));
            }

            private AreaData _Get(int index)
            {
                StreamTableRow row = _st[index];
                return new AreaData(row.Get<ushort>("area_code"), row.Get<byte>("province_index"), row.Get<byte>("city_index"));
            }

            protected override int GetCount()
            {
                return _st.RowCount;
            }
        }

        #endregion

        /// <summary>
        /// 城市字符串表
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        public CityStringTable GetCityStringTable(string nationalCode)
        {
            return (CityStringTable)_cache.GetOrAddOfDynamic("city_string_table", _cacheTime, (key) => _LoadCityStringTable(nationalCode));
        }

        private CityStringTable _LoadCityStringTable(string nationalCode)
        {
            string file = nationalCode + "/string";
            StreamTable stringTable = _GetStreamTable(file + ":city");

            return new CityStringTable(new CityStringList(stringTable));
        }

        #region Class CityStringList ...

        class CityStringList : ListBase<ProvinceStringTable>
        {
            public CityStringList(StreamTable st)
            {
                _st = st;
                _tables = new ProvinceStringTable[st.RowCount];
            }

            private readonly StreamTable _st;
            private readonly ProvinceStringTable[] _tables;

            protected override ProvinceStringTable Get(int index)
            {
                return _tables[index] ?? (_tables[index] = _Get(index));
            }

            private ProvinceStringTable _Get(int index)
            {
                StreamTableRow row = _st[index];
                return new ProvinceStringTable((string)row["province_name"], (string[])row["city_names"]);
            }

            protected override int GetCount()
            {
                return _st.RowCount;
            }
        }

        #endregion

        public void Dispose()
        {
            _stFileMgr.Dispose();
            _cache.Dispose();
        }
    }
}
