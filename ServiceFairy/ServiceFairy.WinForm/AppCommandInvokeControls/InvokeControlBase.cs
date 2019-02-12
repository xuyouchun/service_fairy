using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;
using ServiceFairy.Entities.Sys;
using ServiceFairy.SystemInvoke;
using Common.Utility;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    abstract class InvokeControlContextBase
    {
        public abstract Control GetControl();

        public static InvokeControlContextBase Create(string tag, InvokeControlContext ctx)
        {
            switch (tag)
            {
                case "INPUT_JSON":
                    return new ArgumentSampleInvokeControlContext(ctx, "input", DataFormat.Json);

                case "INPUT_XML":
                    return new ArgumentSampleInvokeControlContext(ctx, "input", DataFormat.Xml);

                case "INPUT_DOC":
                    return new DocInvokeControlContext(ctx, "input");

                case "OUTPUT_JSON":
                    return new ArgumentSampleInvokeControlContext(ctx, "output", DataFormat.Json);

                case "OUTPUT_XML":
                    return new ArgumentSampleInvokeControlContext(ctx, "output", DataFormat.Xml);

                case "OUTPUT_DOC":
                    return new DocInvokeControlContext(ctx, "output");

                default:
                    return null;
            }
        }
    }
}
