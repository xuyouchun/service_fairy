using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Entities;
using System.Diagnostics.Contracts;
using System.Xml;

namespace Common.Mobile
{
    /// <summary>
    /// 运营商信息
    /// </summary>
    public class OperationDetail
    {
        public OperationDetail(string mnc, string brand, string opName, string state, string remarks)
        {
            Mnc = mnc;
            Brand = brand;
            OpName = opName;
            State = state;
            Remarks = remarks;
        }

        /// <summary>
        /// MNC
        /// </summary>
        public string Mnc { get; private set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; private set; }

        /// <summary>
        /// 运营商名称
        /// </summary>
        public string OpName { get; private set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        /// 参考和注释
        /// </summary>
        public string Remarks { get; private set; }

        public override string ToString()
        {
            string s = "[" + Mnc + "] ";
            s += OpName;
            if (string.IsNullOrEmpty(Brand) && Brand != OpName)
                s += " " + Brand;

            return s;
        }
    }
}
