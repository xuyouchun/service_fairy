using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Entities;
using Common.Package.Storage;

namespace SnapIsoCountryCodes
{
    class GenerateNationalCodeTable
    {
        private const string _national_code_file = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data\national_code.st";

        public static void Execute()
        {
            checked
            {
                CountryInfo[] countryInfos = CountryInfo.GetAll();
                StreamTableWriter w = new StreamTableWriter("national_code");

                WritableStreamTable configTable = w.CreateTable("config", new StreamTableColumn[] {
                    new StreamTableColumn("national_code_min_length", StreamTableColumnType.Byte),
                    new StreamTableColumn("national_code_max_length", StreamTableColumnType.Byte),
                });

                configTable.AppendRow(new Dictionary<string, object> {
                    { "national_code_min_length",  countryInfos.Min(ci=>ci.NationalCode.Length) },
                    { "national_code_max_length",  countryInfos.Max(ci=>ci.NationalCode.Length) },
                });

                WritableStreamTable t = w.CreateTable("national_code", new StreamTableColumn[] {
                    new StreamTableColumn("national_code", StreamTableColumnType.UShort),
                    new StreamTableColumn("cn_name", StreamTableColumnType.String),
                }, "按national_code升序排序");

                foreach (CountryInfo cInfo in CountryInfo.GetAll().OrderBy(c => int.Parse(c.NationalCode)))
                {
                    int nationCode = int.Parse(cInfo.NationalCode);
                    string countryName = cInfo.ChineseName;

                    t.AppendRow(new Dictionary<string, object> {
                    { "national_code",      nationCode },
                    { "cn_name",            countryName }
                });
                }

                w.WriteToFile(_national_code_file);
            }
        }
    }
}
