using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.IO
{
    [Function("MsgBox", false, typeof(object))]
    [FunctionInfo("显示对话框", "text [,title [,model]]]")]
    class MsgBoxFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            switch (values.Length)
            {
                case 1:
                    return context.ShowMsgBox(values[0], string.Empty, VbConstValues.vbOKOnly);

                case 2:
                    return context.ShowMsgBox(values[0], values[1], VbConstValues.vbOKOnly);

                default:
                    return context.ShowMsgBox(values[0], values[1], values[2]);
            }
        }
    }
}
