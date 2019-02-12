using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// ActionUIObject执行器
    /// </summary>
    public interface IUIObjectExecutor
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceObject"></param>
        void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject);

        /// <summary>
        /// 是否是有效状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceObject"></param>
        /// <returns></returns>
        UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject);
    }

    /// <summary>
    /// ActionUIObject执行器的状态
    /// </summary>
    [Serializable]
    public class UIObjectExecutorState
    {
        public UIObjectExecutorState(bool enable = true, bool visiable = true, bool @checked = false)
        {
            Enable = enable;
            Visiable = visiable;
            Checked = @checked;
        }

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        public bool Enable { get; private set; }

        /// <summary>
        /// 是否为选中状态
        /// </summary>
        public bool Checked { get; private set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visiable { get; private set; }

        public override int GetHashCode()
        {
            return Enable.GetHashCode() ^ Checked.GetHashCode() ^ Visiable.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(UIObjectExecutorState))
                return false;

            UIObjectExecutorState state = (UIObjectExecutorState)obj;
            return state.Enable == Enable && state.Checked == Checked && state.Visiable == Visiable;
        }

        public static bool operator ==(UIObjectExecutorState state1, UIObjectExecutorState state2)
        {
            return object.Equals(state1, state2);
        }

        public static bool operator !=(UIObjectExecutorState state1, UIObjectExecutorState state2)
        {
            return !object.Equals(state1, state2);
        }

        public static readonly UIObjectExecutorState Default = new UIObjectExecutorState();

        public static UIObjectExecutorState OfVisiable(bool visiable)
        {
            return new UIObjectExecutorState(visiable: visiable);
        }

        public static UIObjectExecutorState OfVisiable(UIObjectExecutorState prototype, bool visiable)
        {
            if (prototype == null)
                return OfVisiable(visiable);

            return new UIObjectExecutorState(prototype.Enable, visiable, prototype.Checked);
        }

        public static UIObjectExecutorState OfEnable(bool enable)
        {
            return new UIObjectExecutorState(enable: enable);
        }

        public static UIObjectExecutorState OfEnable(UIObjectExecutorState prototype, bool enable)
        {
            if (prototype == null)
                return OfEnable(enable);

            return new UIObjectExecutorState(enable, prototype.Visiable, prototype.Checked);
        }

        public static UIObjectExecutorState OfChecked(bool @checked)
        {
            return new UIObjectExecutorState(@checked: @checked);
        }

        public static UIObjectExecutorState OfChecked(UIObjectExecutorState prototype, bool @checked)
        {
            if (prototype == null)
                return OfChecked(@checked);

            return new UIObjectExecutorState(prototype.Enable, prototype.Visiable, prototype.Checked);
        }
    }
}
