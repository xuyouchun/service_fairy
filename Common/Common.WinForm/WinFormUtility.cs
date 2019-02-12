using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using Common.Utility;
using Common.Contracts;
using System.Threading;

namespace Common.WinForm
{
    /// <summary>
    /// 
    /// </summary>
    public static class WinFormUtility
    {
        /// <summary>
        /// 寻找所有指定类型的控件
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<TControl> FindControls<TControl>(this Control parent)
            where TControl : Control
        {
            Contract.Requires(parent != null);

            foreach (Control control in parent.Controls)
            {
                TControl c = control as TControl;
                if (c != null)
                    yield return c;

                foreach (TControl c0 in FindControls<TControl>(control))
                {
                    yield return c0;
                }
            }
        }

        /// <summary>
        /// 获取指定控件的值
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static object GetControlValue(Control control)
        {
            if (control is CheckBox)
            {
                CheckBox c = ((CheckBox)control);
                return c.ThreeState ? (object)c.CheckState : c.Checked;
            }

            if (control is ListBox)
            {
                ListBox c = (ListBox)control;
                return c.SelectedValue;
            }

            return control.Text;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public static void ShowMessage(this IWin32Window owner, string message, string caption = "")
        {
            MessageBox.Show(owner, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public static void ShowError(this IWin32Window owner, string message, string caption = "")
        {
            MessageBox.Show(owner, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="error"></param>
        /// <param name="caption"></param>
        public static void ShowError(this IWin32Window owner, Exception error, string caption = "")
        {
            Contract.Requires(error != null);
            ErrorDialog.Show(owner, error, caption);
        }

        /// <summary>
        /// 显示警告
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        public static void ShowWarning(this IWin32Window owner, string message, string caption = "")
        {
            MessageBox.Show(owner, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// 显示选择“是”或“否”的问题
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ShowQuestion(this IWin32Window owner, string message, string caption = "", bool defaultValue = false)
        {
            return MessageBox.Show(owner, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                defaultValue ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>
        /// 显示选择“是”或“否”的问题
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ShowQuestion(this IWin32Window owner, string message, bool defaultValue)
        {
            return ShowQuestion(owner, message, "", defaultValue);
        }

        /// <summary>
        /// 显示选择“是”、“否”、“取消”的问题
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool? ShowQuestion(this IWin32Window owner, string message, string caption, bool? defaultValue)
        {
            DialogResult r = MessageBox.Show(owner, message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                defaultValue == true ? MessageBoxDefaultButton.Button1 : defaultValue == false ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button3);

            return r == DialogResult.Yes ? (bool?)true : r == DialogResult.No ? (bool?)false : null;
        }

        /// <summary>
        /// 显示选择“是”、“否”、“取消”的问题
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool? ShowQuestion(this IWin32Window owner, string message, bool? defaultValue)
        {
            return ShowQuestion(owner, message, "", defaultValue);
        }

        /// <summary>
        /// 在UI线程上执行代码
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void Invoke(this Control control, Action action)
        {
            Contract.Requires(control != null && action != null);

            control.Invoke((Delegate)action);
        }

        /// <summary>
        /// 在UI线程上执行代码
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void BeginInvoke(this Control control, Action action)
        {
            Contract.Requires(control != null && action != null);

            control.BeginInvoke((Delegate)action);
        }

        /// <summary>
        /// 获取所有的子菜单项
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <returns></returns>
        public static IEnumerable<ToolStripMenuItem> GetAllMenuItems(MenuStrip menuStrip)
        {
            Contract.Requires(menuStrip != null);

            foreach (ToolStripMenuItem menuItem in menuStrip.Items.OfType<ToolStripMenuItem>())
            {
                yield return menuItem;

                foreach (ToolStripMenuItem menuItem0 in GetAllMenuItems(menuItem))
                {
                    yield return menuItem0;
                }
            }
        }

        /// <summary>
        /// 获取所有的子菜单项
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        private static IEnumerable<ToolStripMenuItem> GetAllMenuItems(ToolStripMenuItem menuItem)
        {
            Contract.Requires(menuItem != null);

            foreach (ToolStripMenuItem subMenuItem in menuItem.DropDownItems.OfType<ToolStripMenuItem>())
            {
                yield return subMenuItem;

                foreach (ToolStripMenuItem subMenuItem0 in GetAllMenuItems(subMenuItem))
                {
                    yield return subMenuItem0;
                }
            }
        }

        /// <summary>
        /// 选择该节点
        /// </summary>
        /// <param name="node"></param>
        public static void Select(this TreeNode node)
        {
            Contract.Requires(node != null);

            TreeView tree = node.TreeView;
            if (tree != null)
                tree.SelectedNode = node;
        }

        /// <summary>
        /// 展开指定层次的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="deep"></param>
        public static void Expand(this TreeNode node, int deep)
        {
            Contract.Requires(node != null && deep >= 0);

            if (deep == 0)
                return;

            node.Expand();
            _Expand(node.Nodes, deep - 1);
        }

        private static void _Expand(TreeNodeCollection nodes, int deep)
        {
            if (deep <= 0)
                return;

            foreach (TreeNode node in nodes)
            {
                node.Expand();
                _Expand(node.Nodes, deep - 1);
            }
        }

        /// <summary>
        /// 展开指定层次的节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="deep"></param>
        public static void Expand(this TreeView tree, int deep)
        {
            Contract.Requires(tree != null && deep >= 0);

            _Expand(tree.Nodes, deep);
        }

        /// <summary>
        /// 执行指定的动作，如果遇到错误，则显示错误对话框
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="actionUIObject"></param>
        /// <param name="executeContext"></param>
        public static void ExecuteActionWithErrorDialog(this IServiceObject serviceObject, IActionUIObject actionUIObject, IUIObjectExecuteContext executeContext)
        {
            Contract.Requires(serviceObject != null && actionUIObject != null && executeContext != null);

            try
            {
                actionUIObject.Execute(executeContext, serviceObject);
            }
            catch (UserCancelException) { }
            catch (Exception ex)
            {
                _ShowErrorDialogForActionUIObject(executeContext, ex);
            }
        }

        private static void _ShowErrorDialogForActionUIObject(IUIObjectExecuteContext executeContext, Exception error)
        {
            IUIOperation op = executeContext.ServiceProvider.GetService<IUIOperation>();
            if (op != null)
            {
                ServiceException ex = error as ServiceException;
                if (ex != null)
                {
                    if ((int)ex.StatusCode >= 501 && (int)ex.StatusCode < 600)
                        op.ShowError(ex.Message);
                    else
                        op.ShowError(ex);
                }
                else
                {
                    op.ShowError(error);
                }
            }
        }

        /// <summary>
        /// 执行默认动作，如果遇到错误，则显示错误对话框
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="executeContext"></param>
        public static void ExecuteDefaultActionWithErrorDialog(this IServiceObject serviceObject, IUIObjectExecuteContext executeContext)
        {
            ExecuteActionWithErrorDialog(serviceObject, executeContext, ServiceObjectActionType.Default);
        }

        /// <summary>
        /// 执行指定类型的动作，如果遇到错误，则显示错误对话框
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="executeContext"></param>
        /// <param name="actionType"></param>
        public static void ExecuteActionWithErrorDialog(this IServiceObject serviceObject, IUIObjectExecuteContext executeContext, ServiceObjectActionType actionType)
        {
            Contract.Requires(serviceObject != null && executeContext != null);

            try
            {
                serviceObject.ExecuteAction(executeContext, actionType);
            }
            catch (UserCancelException) { }
            catch (Exception ex)
            {
                _ShowErrorDialogForActionUIObject(executeContext, ex);
            }
        }

        private class InvokeContext<T> where T : class
        {
            public Func<T> Func;
            public ManualResetEvent WaitHandle;
            public Exception Error;
            public T Result;
        }

        /// <summary>
        /// 带有进度对话框的调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiOperation"></param>
        /// <param name="func"></param>
        /// <param name="title"></param>
        /// <param name="showDelay"></param>
        /// <param name="showCancelButton"></param>
        /// <param name="throwError">是否抛出异常</param>
        /// <returns></returns>
        public static T InvokeWithProgressWindow<T>(this IUIOperation uiOperation, Func<T> func, string title, TimeSpan showDelay = default(TimeSpan), bool showCancelButton = true, bool throwError = true, bool topMost = false)
            where T : class
        {
            Contract.Requires(uiOperation != null && func != null);

            ManualResetEvent e = new ManualResetEvent(false);
            InvokeContext<T> ctx = new InvokeContext<T>() { WaitHandle = e, Func = func };
            Thread thread = ThreadUtility.StartNew(_Invoke, ctx);
            if (!uiOperation.ShowProgressWindow(title, e, showDeply: showDelay, showCancelButton: showCancelButton, topMost: topMost))
            {
                thread.Abort();

                if (throwError)
                    throw new UserCancelException("操作已取消");

                return null;
            }

            if (ctx.Error != null)
            {
                if (throwError)
                    throw ctx.Error;

                uiOperation.ShowError(ctx.Error);
                return null;
            }

            return ctx.Result;
        }

        private static void _Invoke<T>(InvokeContext<T> ctx) where T : class
        {
            try
            {
                ctx.Result = ctx.Func();
            }
            catch (Exception ex)
            {
                ctx.Error = ex;
            }
            finally
            {
                ctx.WaitHandle.Set();
            }
        }

        /// <summary>
        /// 带有进度对话框的调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="window"></param>
        /// <param name="func"></param>
        /// <param name="title"></param>
        /// <param name="showDelay"></param>
        /// <param name="showCancelButton"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static T InvokeWithProgressWindow<T>(this IWin32Window window, Func<T> func, string title, TimeSpan showDelay = default(TimeSpan), bool showCancelButton = true, bool throwError = true, bool topMost = false)
            where T : class
        {
            Contract.Requires(func != null);

            ManualResetEvent e = new ManualResetEvent(false);
            InvokeContext<T> ctx = new InvokeContext<T>() { WaitHandle = e, Func = func };
            Thread thread = ThreadUtility.StartNew(_Invoke, ctx);
            if(!ProgressWindow.Show(window, title, e, showDelay: showDelay, showCancelButton: showCancelButton, topMost: topMost))
            {
                thread.Abort();

                if (throwError)
                    throw new UserCancelException("操作已取消");

                return null;
            }

            if (ctx.Error != null)
            {
                if (throwError)
                    throw ctx.Error;
            }

            return ctx.Result;
        }
    }
}
