using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class VbConstValues
    {
        [VbConstValue("MsgBox函数中的参数，表示对话框上只显示一个“确定”按钮")]
        public const int vbOKOnly = 0;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示“确定”与“取消”按钮")]
        public const int vbOKCancel = 1;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示“中止”、“重试”、“忽略”按钮")]
        public const int vbAbortRetryIgnore = 2;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示“是”、“否”、“取消”按钮")]
        public const int vbYesNoCancel = 3;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示“是”、“否”按钮")]
        public const int vbYesNo = 4;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示“重试”、“取消”按钮")]
        public const int vbRetryCancel = 5;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示一个紧急图标")]
        public const int vbCritical = 16;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示一个询问图标")]
        public const int vbQuestion = 32;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示一个警告图标")]
        public const int vbExclamation = 48;

        [VbConstValue("MsgBox函数中的参数，表示对话框上显示一个信息图标")]
        public const int vbInformation = 64;

        [VbConstValue("MsgBox函数中的参数，表示对话框上第一个按钮默认具有焦点")]
        public const int vbDefaultButton1 = 0;

        [VbConstValue("MsgBox函数中的参数，表示对话框上第二个按钮默认具有焦点")]
        public const int vbDefaultButton2 = 256;

        [VbConstValue("MsgBox函数中的参数，表示对话框上第三个按钮默认具有焦点")]
        public const int vbDefaultButton3 = 512;

        [VbConstValue("MsgBox函数中的参数，表示对话框上第四个按钮默认具有焦点")]
        public const int vbDefaultButton4 = 768;

        [VbConstValue("MsgBox函数中的参数，表示对话框以模式对话框方式显示")]
        public const int vbApplicationModal = 0;

        [VbConstValue("MsgBox函数中的参数，表示对话框以非模式对话框方式显示")]
        public const int vbSystemModal = 4096;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“确定”按钮")]
        public const int vbOK = 1;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“取消”按钮")]
        public const int vbCancel = 2;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“中止”按钮")]
        public const int vbAbort = 3;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“重试”按钮")]
        public const int vbRetry = 4;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“忽略”按钮")]
        public const int vbIgnore = 5;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“是”按钮")]
        public const int vbYes = 6;

        [VbConstValue("MsgBox函数的返回值，表示用户点击了“否”按钮")]
        public const int vbNo = 7;

        [VbConstValue("颜色的RGB数值：表示黑色")]
        public const int vbBlack  = 0x00;

        [VbConstValue("颜色的RGB数值：表示红色")]
        public const int vbRed =0xFF;

        [VbConstValue("颜色的RGB数值：表示绿色")]
        public const int vbGreen = 0xFF00;

        [VbConstValue("颜色的RGB数值：表示黄色")]
        public const int vbYellow = 0xFFFF;

        [VbConstValue("颜色的RGB数值：表示蓝色")]
        public const int vbBlue = 0xFF0000;

        [VbConstValue("颜色的RGB数值：表示洋红色")]
        public const int vbMagenta = 0xFF00FF;

        [VbConstValue("颜色的RGB数值：表示青色")]
        public const int vbCyan = 0xFFFF00;

        [VbConstValue("颜色的RGB数值：表示白色")]
        public const int vbWhite = 0xFFFFFF;

        [VbConstValue("函数StrComp的参数，表示按二进制方式比较，不忽略大小写")]
        public const int vbBinaryCompare = 0;

        [VbConstValue("函数StrComp的参数，表示按文本方式比较，忽略大小写")]
        public const int vbTextCompare = 1;

        [VbConstValue("星期日")]
        public const int vbSunday = 1;

        [VbConstValue("星期一")]
        public const int vbMonday = 2;

        [VbConstValue("星期二")]
        public const int vbTuesday = 3;

        [VbConstValue("星期三")]
        public const int vbWednesday = 4;

        [VbConstValue("星期四")]
        public const int vbThursday = 5;

        [VbConstValue("星期五")]
        public const int vbFriday = 6;

        [VbConstValue("星期六")]
        public const int vbSaturday = 7;

        [VbConstValue("使用系统中星期设置")]
        public const int vbUseSystemDayOfWeek = 0;

        public const int vbFirstJan1 = 1;

        public const int vbFirstFourDays = 2;

        public const int vbFirstFullWeek = 3;

        public const int vbGeneralDate = 0;

        [VbConstValue("Now函数中的参数，长日期格式")]
        public const int vbLongDate = 1;

        [VbConstValue("Now函数中的参数，短日期格式")]
        public const int vbShortDate = 2;

        [VbConstValue("Now函数中的参数，长时间格式")]
        public const int vbLongTime = 3;

        [VbConstValue("Now函数中的参数，短时间格式")]
        public const int vbShortTime = 4;

        public const int vbObjectError = -2147221504;

        public const string vbCr = "\r";

        public const string vbCrLf = "\r\n";

        //public const string vbFormFeed = "\12";

        public const string vbLf = "\n";

        public const string vbNewLine = "\r\n";

        public const string vbNullChar = "\0";

        public const string vbNullString = "";

        public const string vbTab = "\t";

        public const string vbVerticalTab = "\v";

        public const int vbUseDefault = -2;

        public const int vbTrue = -1;

        public const int vbFalse = 0;

        [VbConstValue("函数CType返回值，表示数值尚未初始化")]
        public const int vbEmpty = 0;

        [VbConstValue("函数CType返回值，表示数值为无效数据")]
        public const int vbNull = 1;

        [VbConstValue("函数CType返回值，表示数值类型为整型")]
        public const int vbInteger = 2;

        [VbConstValue("函数CType返回值，表示数值类型为长整型")]
        public const int vbLong = 3;

        [VbConstValue("函数CType返回值，表示数值类型为单精度浮点型")]
        public const int vbSingle = 4;

        [VbConstValue("函数CType返回值，表示数值类型为双精度浮点型")]
        public const int vbDouble = 5;

        [VbConstValue("函数CType返回值，表示数值类型为Currency型")]
        public const int vbCurrency = 6;

        [VbConstValue("函数CType返回值，表示数值类型为时间型")]
        public const int vbDate = 7;

        [VbConstValue("函数CType返回值，表示数值类型为字符串型")]
        public const int vbString = 8;

        [VbConstValue("函数CType返回值，表示数值类型为对象型")]
        public const int vbObject = 9;

        [VbConstValue("函数CType返回值，表示数值类型为异常信息")]
        public const int vbError = 10;

        [VbConstValue("函数CType返回值，表示数值类型为布尔型")]
        public const int vbBoolean = 11;

        [VbConstValue("函数CType返回值，表示数值类型尚未确定")]
        public const int vbVariant = 12;

        [VbConstValue("函数CType返回值，表示数值类型为数据库访问对象")]
        public const int vbDataObject = 13;

        [VbConstValue("函数CType返回值，表示数值类型为Decimal型")]
        public const int vbDecimal = 14;

        [VbConstValue("函数CType返回值，表示数值类型为Byte型")]
        public const int vbByte = 17;

        [VbConstValue("函数CType返回值，表示数值类型为数组型")]
        public const int vbArray = 8192;

        private static Dictionary<string, VbConstValueInfo> _Dict;

        private static Dictionary<string, VbConstValueInfo> _GetDict()
        {
            var dict = _Dict;
            if (dict != null)
                return dict;

            dict = new Dictionary<string, VbConstValueInfo>();
            foreach (FieldInfo fInfo in typeof(VbConstValues).GetFields())
            {
                VbConstValueAttribute[] attrs = (VbConstValueAttribute[])fInfo.GetCustomAttributes(typeof(VbConstValueAttribute), false);
                dict[fInfo.Name.ToUpper()] = new VbConstValueInfo(fInfo.Name, new Value(fInfo.GetValue(null)), attrs.Length == 0 ? string.Empty : attrs[0].Description);
            }

            return _Dict = dict;
        }

        /// <summary>
        /// 获取所有的常量信息
        /// </summary>
        /// <returns></returns>
        public static VbConstValueInfo[] GetAllConstValueInfos()
        {
            return new List<VbConstValueInfo>(_GetDict().Values).ToArray();
        }

        /// <summary>
        /// 获取常量信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VbConstValueInfo GetConstValueInfo(string name)
        {
            if (name == null)
                return null;

            VbConstValueInfo info;
            if (_GetDict().TryGetValue(name.ToUpper(), out info))
                return info;

            return info;
        }

        /// <summary>
        /// 获取常量值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Value GetConstValue(string name)
        {
            if (name == null)
                return null;

            VbConstValueInfo info = GetConstValueInfo(name);
            return info == null ? null : info.Value;
        }
    }
}
