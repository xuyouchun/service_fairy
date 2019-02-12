using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Utility;

namespace Common.WinForm
{
    public partial class DropdownSelectDialog : XDialog
    {
        public DropdownSelectDialog()
        {
            InitializeComponent();
        }

        private static DropdownSelectDialog _CreateDialog(object[] list, string initText, string prompt, string title, ComboBoxStyle style)
        {
            return _CreateDialog(() => list, initText, prompt, title, style);
        }

        private static DropdownSelectDialog _CreateDialog(Func<object[]> listFunc, string initText, string prompt, string title, ComboBoxStyle style)
        {
            DropdownSelectDialog dlg = new DropdownSelectDialog();

            dlg._ctlPrompt.Text = prompt;
            dlg._ctlList.Text = initText;
            dlg.Text = title;
            dlg._ctlList.DropDownStyle = style;

            bool dropDownListInitialized = false;
            dlg._ctlList.DropDown += delegate(object sender, EventArgs e) {
                if (!dropDownListInitialized)
                {
                    try
                    {
                        object[] list;
                        if (listFunc != null && (list = listFunc()) != null)
                            dlg._ctlList.Items.AddRange(list);

                        dropDownListInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorDialog.Show(dlg, ex);
                    }
                }
            };

            return dlg;
        }

        /// <summary>
        /// 输入文本
        /// </summary>
        /// <param name="window"></param>
        /// <param name="list"></param>
        /// <param name="initText"></param>
        /// <param name="prompt"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static string Show(IWin32Window window, string[] list, string initText, string prompt = "", string title = "", bool editable = true)
        {
            return Show(window, () => list, initText, prompt, title, editable);
        }

        /// <summary>
        /// 输入文本
        /// </summary>
        /// <param name="window"></param>
        /// <param name="listCreator"></param>
        /// <param name="initText"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static string Show(IWin32Window window, Func<string[]> listCreator, string initText, string prompt = "", string title = "", bool editable = true)
        {
            DropdownSelectDialog dlg = _CreateDialog(listCreator, initText, prompt, title,
                editable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList);

            if (dlg.ShowDialog(window) == DialogResult.OK)
                return dlg._ctlList.Text;

            return null;
        }

        /// <summary>
        /// 从下拉列表中选择对象
        /// </summary>
        /// <param name="window"></param>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static object Show(IWin32Window window, object[] list, object current, string prompt = "", string title = "", bool editable = false)
        {
            return Show(window, () => list, current, prompt, title, editable);
        }

        /// <summary>
        /// 从下拉列表中选择对象
        /// </summary>
        /// <param name="window"></param>
        /// <param name="listCreator"></param>
        /// <param name="current"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static object Show(IWin32Window window, Func<object[]> listCreator, object current, string prompt = "", string title = "", bool editable = false)
        {
            DropdownSelectDialog dlg = _CreateDialog(listCreator, "", prompt, title, editable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList);
            dlg._ctlList.SelectedItem = current;

            if (dlg.ShowDialog(window) == DialogResult.OK)
                return dlg._ctlList.SelectedItem;

            return null;
        }

        /// <summary>
        /// 从下拉列表中选择对象
        /// </summary>
        /// <param name="window"></param>
        /// <param name="listCreator"></param>
        /// <param name="current"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static T Show<T>(IWin32Window window, Func<T[]> listCreator, T current, string prompt = "", string title = "", bool editable = false)
            where T : class
        {
            return (T)Show(window, new Func<object[]>(() => listCreator == null ? null : listCreator().CastAsObject()), (object)current, prompt, title, editable);
        }

        /// <summary>
        /// 从下拉列表中选择对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="window"></param>
        /// <param name="listCreator"></param>
        /// <param name="current"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static T Show<T>(IWin32Window window, T[] list, T current, string prompt = "", string title = "", bool editable = false)
            where T : class
        {
            return Show<T>(window, () => list, current, prompt, title, editable);
        }

        /// <summary>
        /// 从下拉列表框中选择对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="window"></param>
        /// <param name="listCreator"></param>
        /// <param name="current"></param>
        /// <param name="result"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool Show<T>(IWin32Window window, Func<T[]> listCreator, T current, out T result, string prompt = "", string title = "", bool editable = false)
        {
            object obj = Show(window, () => listCreator == null ? null : listCreator().CastAsObject(), (object)current, prompt, title, editable);
            if (obj == null)
            {
                result = default(T);
                return false;
            }

            result = (T)obj;
            return true;
        }


        /// <summary>
        /// 从下拉列表中选择对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="window"></param>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <param name="result"></param>
        /// <param name="prompt"></param>
        /// <param name="title"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static bool Show<T>(IWin32Window window, T[] list, T current, out T result, string prompt = "", string title = "", bool editable = false)
        {
            return Show<T>(window, () => list, current, out result, prompt, title, editable);
        }
    }
}
