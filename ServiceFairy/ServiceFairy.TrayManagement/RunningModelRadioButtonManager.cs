using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceFairy.Install;

namespace ServiceFairy.TrayManagement
{
    class RunningModelRadioButtonManager : RadioButtonManager
    {
        public RunningModelRadioButtonManager(RadioButton[] radioButtons)
            : base(radioButtons)
        {

        }

        public RunningModelRadioButtonManager(Control parent)
            : base(parent)
        {

        }

        /// <summary>
        /// 当前值
        /// </summary>
        public new RunningModel Value
        {
            get
            {
                object value = base.Value;
                if (value == null)
                    return RunningModel.None;

                return (RunningModel)Enum.Parse(typeof(RunningModel), value.ToString());
            }
            set
            {
                base.Value = value.ToString();
            }
        }
    }
}
