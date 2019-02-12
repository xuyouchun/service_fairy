using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Common.Package.Storage;
using Common.Utility;
using System.IO;
using Common.Contracts.Entities;

namespace SnapIsoCountryCodes
{
    class ConvertXmlToStreamTable
    {
        private const string _xmlFile = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data\cn.xml";
        //private const string _xmlFile = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data_xml\美国.xml";
        private const string _dstDir = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data\";

        public static void Execute()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_xmlFile);

            // basic_info
            BasicInfo basicInfo = new BasicInfo() {
                CnName = xDoc.ReadInnerText("/root/basic_info/cn_name"),
                EnName = xDoc.ReadInnerText("/root/basic_info/en_name"),
                NationalCode = xDoc.ReadInnerText("/root/basic_info/national_code"),
                Mcc = xDoc.ReadInnerText("/root/basic_info/mcc"),
            };

            // 运营商
            List<Operation> operations = new List<Operation>();
            foreach (XmlElement operationNode in xDoc.SelectNodes("/root/operations/operation"))
            {
                Operation op = new Operation() {
                    OpCode = operationNode.GetAttribute("op_code"), InternalPrefix = operationNode.GetAttribute("internal_prefix"),
                    NationalPrefix = operationNode.GetAttribute("national_prefix"), Desc = operationNode.GetAttribute("desc"), Mncs = operationNode.GetAttribute("mncs").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                };
                operations.Add(op);
            }

            // 归属地数据
            List<OperationAreaData> operationAreaDatas = new List<OperationAreaData>();
            foreach (XmlElement operationAreaData in xDoc.SelectNodes("/root/area_datas/area_data"))
            {
                List<OperationAreaDataPrefix> prefixes = new List<OperationAreaDataPrefix>();
                foreach (XmlElement prefixData in operationAreaData.SelectNodes("prefixes/prefix"))
                {
                    prefixes.Add(new OperationAreaDataPrefix() { OpCode = prefixData.GetAttribute("op_code"), OpPrefix = prefixData.GetAttribute("op_prefix") });
                }

                OperationAreaData areaData = new OperationAreaData() { Prefixes = prefixes.ToArray(), StorageModel = operationAreaData.GetAttribute("storage_model") };
                foreach (XmlElement cityNode in operationAreaData.SelectNodes("cities/city"))
                {
                    string provinceName = cityNode.GetAttribute("province"), cityName = cityNode.GetAttribute("city"), code = cityNode.GetAttribute("area_code");
                    City city = Province.GetProvince(provinceName).GetCity(cityName);
                    if (areaData.CodeDict.ContainsKey(code))
                    {
                        City city0 = areaData.CodeDict[code];
                        string[] cityNames = city0.CityName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        if (!string.IsNullOrEmpty(city.CityName))
                        {
                            areaData.CodeDict[code] = Province.GetProvince(provinceName).GetCity(string.Join("/", cityNames.Union(new[] { city.CityName })));
                        }
                    }
                    else
                    {
                        areaData.CodeDict.Add(code, city);
                    }
                    areaData.CityDict.GetOrSet(city).Add(code);
                }
                operationAreaDatas.Add(areaData);
            }

            _GenerateFiles(operationAreaDatas, operations, basicInfo);

        }

        // 生成主文件
        private static void _GenerateFiles(List<OperationAreaData> areaDatas, List<Operation> operations, BasicInfo basicInfo)
        {
            StreamTableWriter mainWriter = new StreamTableWriter(basicInfo.NationalCode);

            // 配置表
            WritableStreamTable configTable = mainWriter.CreateTable("config", new StreamTableColumn[] {
                new StreamTableColumn("cn_name",        StreamTableColumnType.String),      // 中文名称，例如：中国
                new StreamTableColumn("en_name",        StreamTableColumnType.String),      // 英文名称，例如：China
                new StreamTableColumn("national_code",  StreamTableColumnType.String),      // 国际代码，例如：86
                new StreamTableColumn("mcc",            StreamTableColumnType.String),      // 移动国家码，例如：460
                new StreamTableColumn("op_prefix_min_length", StreamTableColumnType.Byte),  // 运营商前缀的最小长度
                new StreamTableColumn("op_prefix_max_length", StreamTableColumnType.Byte),  // 运营商前缀的最大长度
            });

            configTable.AppendRow(new Dictionary<string, object> {
                { "cn_name",                basicInfo.CnName },
                { "en_name",                basicInfo.EnName },
                { "national_code",          basicInfo.NationalCode },
                { "mcc",                    basicInfo.Mcc },
                { "op_prefix_min_length",   areaDatas.SelectMany(ad=>ad.Prefixes).MinOrDefault(pfx => pfx.OpPrefix.Length) },
                { "op_prefix_max_length",   areaDatas.SelectMany(ad=>ad.Prefixes).MaxOrDefault(pfx => pfx.OpPrefix.Length) },
            });

            // 运营商表，按internal_prefix长度逆序排序
            WritableStreamTable operationTable = mainWriter.CreateTable("operation", new StreamTableColumn[] {
                new StreamTableColumn("op_code",              StreamTableColumnType.String),  // 英文代号
                new StreamTableColumn("desc",                 StreamTableColumnType.String),  // 中文名称
                new StreamTableColumn("national_prefix",      StreamTableColumnType.String),  // 国际长途前缀，以逗号分隔形式存储：例如00
                new StreamTableColumn("internal_prefix",      StreamTableColumnType.String),  // 国内长途前缀，以逗号分隔形式存储：例如0
            }, "按internal_prefix、op_code排序");

            Dictionary<string, int> opCodeIndexDict = new Dictionary<string, int>();
            int opIndex = 0;
            foreach (Operation op in operations.OrderByDescending(op => op.InternalPrefix.Length).ThenBy(op => op.OpCode))
            {
                operationTable.AppendRow(new Dictionary<string, object> {
                    { "op_code",              op.OpCode },
                    { "desc",                 op.Desc },
                    { "national_prefix",      op.NationalPrefix },
                    { "internal_prefix",      op.InternalPrefix },
                });

                opCodeIndexDict.Add(op.OpCode, opIndex++);
            }

            // MNC表，按mnc排序
            WritableStreamTable mncTable = mainWriter.CreateTable("mnc", new StreamTableColumn[] {
                new StreamTableColumn("mnc",        StreamTableColumnType.UShort),           // mnc，例如00, 01
                new StreamTableColumn("op_index",   StreamTableColumnType.UShort),           // 运营商索引号（对应于运营商表中的索引）
            }, "按mnc排序");

            foreach (var item in operations.SelectMany(op => op.Mncs.Select(mnc => new { Operation = op, Mnc = ushort.Parse(mnc) })).OrderBy(item => item.Mnc))
            {
                mncTable.AppendRow(new Dictionary<string, object> {
                    { "mnc",        item.Mnc },
                    { "op_index",   opCodeIndexDict[item.Operation.OpCode] },
                });
            }

            // 运营商前缀表，按prefix顺序排序
            WritableStreamTable opPrefixTable = mainWriter.CreateTable("op_prefix", new StreamTableColumn[] {
                new StreamTableColumn("op_prefix",     StreamTableColumnType.String),           // 前缀，例如137、158
                new StreamTableColumn("op_index",   StreamTableColumnType.UShort),           // 运营商索引号（对应于运营商表中的索引）
            }, "按op_prefix排序");

            foreach (var item in areaDatas.SelectMany(ad=>ad.Prefixes).Select(pfx => new { Prefix = pfx.OpPrefix, OpCode = pfx.OpCode }).OrderBy(item => item.Prefix))
            {
                opPrefixTable.AppendRow(new Dictionary<string, object> {
                    { "op_prefix",  item.Prefix },
                    { "op_index",   opCodeIndexDict[item.OpCode] },
                });
            }

            _SaveStreamTable(mainWriter);

            // 区域数据表
            foreach (OperationAreaData areaData in areaDatas)
            {
                StreamTableWriter operationWriter = new StreamTableWriter(basicInfo.NationalCode + "_" + string.Join(",", areaData.Prefixes.Select(pfx => pfx.OpPrefix)));
                WritableStreamTable areaConfigTable = operationWriter.CreateTable("config", new StreamTableColumn[] {
                    new StreamTableColumn("storage_model", StreamTableColumnType.String),
                    new StreamTableColumn("area_code_min_length", StreamTableColumnType.Byte),
                    new StreamTableColumn("area_code_max_length", StreamTableColumnType.Byte),
                });

                var area_codes = areaData.CodeDict.SelectMany(item =>item.Key.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
                areaConfigTable.AppendRow(new Dictionary<string, object> {
                    { "storage_model",           areaData.StorageModel },
                    { "area_code_min_length",    area_codes.Min(v=>v.Length) },
                    { "area_code_max_length",    area_codes.Max(v=>v.Length) },
                });

                WritableStreamTable areaCodeTable = operationWriter.CreateTable("area_code", new StreamTableColumn[] {
                    new StreamTableColumn("area_code",      StreamTableColumnType.UShort),
                    new StreamTableColumn("province_index", StreamTableColumnType.Byte),
                    new StreamTableColumn("city_index",     StreamTableColumnType.Byte),
                    //new StreamTableColumn("repeat_count",   StreamTableColumnType.Byte)
                }, "按area_code排序，其中递增的area_code未被存储");

                foreach (OperationCodeItem codeItem in _GetOperationCodeItems(areaData))
                {
                    areaCodeTable.AppendRow(new Dictionary<string, object> {
                        { "area_code",          codeItem.AreaCode },
                        { "province_index",     codeItem.City.Province.ProvinceIndex },
                        { "city_index",         codeItem.City.CityIndex },
                        //{ "repeat_count",       codeItem.RepeatCount },
                    });
                }

                _SaveStreamTable(operationWriter);
            }

            // 字符串表
            StreamTableWriter stringTableWriter = new StreamTableWriter(basicInfo.NationalCode + "_string");
            WritableStreamTable cityTable = stringTableWriter.CreateTable("city", new StreamTableColumn[] {
                new StreamTableColumn("province_name", StreamTableColumnType.String),
                new StreamTableColumn("city_names", StreamTableColumnType.String,  StreamTableColumnStorageModel.DynamicArray),
            });

            foreach (Province province in Province.GetAllProvinces())
            {
                string[] cities = province.GetAllCities().SelectFromArray(c=>c.CityName);
                cityTable.AppendRow(new Dictionary<string, object> {
                    { "province_name",      province.ProvinceName },
                    { "city_names",         cities },
                });
            }

            _SaveStreamTable(stringTableWriter);
        }

        private static void _SaveStreamTable(StreamTableWriter tableWriter)
        {
            string file = Path.Combine(_dstDir, tableWriter.Name + ".st");
            tableWriter.WriteToFile(file);
        }

        class Operation
        {
            public string OpCode, NationalPrefix, InternalPrefix, Desc;
            public string[] Mncs;
        }

        class OperationAreaData
        {
            public string StorageModel;
            public Dictionary<City, HashSet<string>> CityDict = new Dictionary<City, HashSet<string>>();
            public Dictionary<string, City> CodeDict = new Dictionary<string, City>();
            public OperationAreaDataPrefix[] Prefixes;
        }

        class OperationAreaDataPrefix
        {
            public string OpCode, OpPrefix;
        }

        class BasicInfo
        {
            public string CnName, EnName, NationalCode, Mcc;
        }

        #region Class Province ...

        class Province
        {
            private Province() { }
            public string ProvinceName;
            public int ProvinceIndex;
            public Dictionary<string, City> CityDict = new Dictionary<string, City>();

            public City GetCity(string cityName)
            {
                return CityDict.GetOrSet(cityName, key => new City() { CityIndex = CityDict.Count, Province = this, CityName = cityName });
            }

            public City[] GetAllCities()
            {
                return CityDict.Values.OrderBy(c => c.CityIndex).ToArray();
            }

            public override string ToString()
            {
                return ProvinceName;
            }

            private static readonly Dictionary<string, Province> _provinceDict = new Dictionary<string, Province>();
            public static Province GetProvince(string provinceName)
            {
                return _provinceDict.GetOrSet(provinceName,
                    key => new Province() { ProvinceIndex = _provinceDict.Count, ProvinceName = provinceName });
            }

            public static Province[] GetAllProvinces()
            {
                return _provinceDict.Values.OrderBy(p => p.ProvinceIndex).ToArray();
            }
        }

        class City
        {
            public Province Province;
            public int CityIndex;
            public string CityName;

            public override string ToString()
            {
                return CityName;
            }
        }

        #endregion

        class OperationCodeItem
        {
            public ushort AreaCode;
            public City City;
            public int RepeatCount;
        }

        private static IEnumerable<OperationCodeItem> _GetOperationCodeItems(OperationAreaData areaData)
        {
            var list = areaData.CodeDict.SelectMany(item =>
                    item.Key.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(area_code => new { AreaCode = area_code, City = item.Value })).OrderBy(item => item.AreaCode).ToArray();

            if (areaData.StorageModel == "full")
            {
                foreach (var item in list)
                {
                    yield return new OperationCodeItem() {
                        AreaCode = ushort.Parse(item.AreaCode), City = item.City
                    };
                }
            }
            else if (areaData.StorageModel == "ellipsis")
            {
                OperationCodeItem last = null;
                int repeatCount = 0, lastAreaCode = 0;

                foreach (var item in list)
                {
                    string area_code = item.AreaCode;
                    City city = item.City;

                    OperationCodeItem opCodeItem = new OperationCodeItem() {
                        AreaCode = ushort.Parse(area_code), City = city,
                    };

                    if (last == null || last.City == opCodeItem.City)
                    {
                        last = last ?? opCodeItem;

                        lastAreaCode = opCodeItem.AreaCode;
                        repeatCount++;
                    }
                    else
                    {
                        last.RepeatCount = repeatCount;
                        yield return last;

                        last = opCodeItem;
                        repeatCount = 1;
                    }
                }

                if (last != null)
                {
                    last.RepeatCount = repeatCount;
                    yield return last;
                }
            }
        }
    }
}
