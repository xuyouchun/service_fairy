using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.Functions.DateTimes
{
    /// <summary>
    /// 给定的时间加上给定的时间段（天数，小时数，秒数）
    /// 有三个参数，分别是
    /// 开始时间：字符串类型的时间
    /// 要添加的时间单位：ss，秒；hh，小时；mm，分钟
    /// 添加的数量：字符串类型，必须是可以转化成Double类型的字符串
    /// </summary>
    [Function("DateAdd",typeof(string),typeof(string),typeof(string))]
    class DateAddFunction:FunctionBase
    {
        protected override Value OnExecute(RunningContext context, Value[] values)
        {
            DateTime begionTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            double TimeAddCount = 0;
            int DateAddCount = 0;
            try
            {
                begionTime = DateTime.Parse(values[0].ToString());
                TimeAddCount = double.Parse(values[2].ToString());
                DateAddCount = int.Parse(values[2].ToString());

                switch (values[1].ToString())
                {
                    case "ss": endTime = begionTime.AddSeconds(TimeAddCount); break;
                    case "hh": endTime = begionTime.AddHours(TimeAddCount); break;
                    case "mm": endTime = begionTime.AddMinutes(TimeAddCount); break;
                    case "m": endTime = begionTime.AddMonths(DateAddCount); break;
                    case "d": endTime = begionTime.AddDays(DateAddCount); break;
                    case "y": endTime = begionTime.AddYears(DateAddCount); break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return endTime.ToString("yyyy-MM-dd");
        }
    }
}
