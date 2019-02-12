using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Utility;
using Common.Contracts.Service;

namespace Common.WinForm
{
    public class MenuStripCommandManager : MarshalByRefObjectEx, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <param name="commandAssembly"></param>
        /// <param name="context"></param>
        /// <param name="commandNameGetter"></param>
        public MenuStripCommandManager(MenuStrip menuStrip, Assembly commandAssembly, object context, Func<ToolStripMenuItem, string> commandNameGetter = null, Action<Exception> showError = null)
        {
            Contract.Requires(menuStrip != null && commandAssembly != null);

            MenuStrip = menuStrip;
            CommandManager = CommandManager.LoadFromAssembly(commandAssembly);
            _commandNameGetter = _commandNameGetter ?? (mi => mi.Tag as string);
            _showError = showError ?? (ex => ErrorDialog.Show(menuStrip.FindForm(), ex));
            _context = context;

            WinFormUtility.GetAllMenuItems(menuStrip).ForEach(menuItem => {
                menuItem.Click += new EventHandler(menuItem_Click);
                menuItem.DropDownOpening += new EventHandler(menuItem_DropDownOpening);
            });
        }

        private readonly Func<ToolStripMenuItem, string> _commandNameGetter;
        private readonly object _context;
        private readonly Action<Exception> _showError;

        private ICommand _GetCommand(ToolStripMenuItem menuItem)
        {
            string commandName = _commandNameGetter(menuItem);
            return commandName == null ? null : CommandManager.GetCommand(commandName);
        }

        void menuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem mItem in (((ToolStripMenuItem)sender).DropDownItems).OfType<ToolStripMenuItem>())
            {
                ICommand cmd = _GetCommand(mItem);
                mItem.Enabled = (cmd == null) ? false : cmd.IsAvaliable(_context);
            }
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = _GetCommand((ToolStripMenuItem)sender);
            if (cmd != null)
                cmd.Execute(_context);
        }

        /// <summary>
        /// 获取指定名称的命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public ICommand GetCommand(string commandName)
        {
            Contract.Requires(commandName != null);

            return CommandManager.GetCommand(commandName);
        }

        /// <summary>
        /// 执行指定的命令
        /// </summary>
        /// <param name="commandName"></param>
        public void Execute(string commandName)
        {
            Contract.Requires(commandName != null);

            try
            {
                ICommand cmd = GetCommand(commandName);
                if (cmd == null)
                    throw new NotSupportedException("不支持该命令:" + commandName);

                cmd.Execute(_context);
            }
            catch (Exception ex)
            {
                _showError(ex);
            }
        }

        /// <summary>
        /// 是否支持指定的命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public bool IsSupportCommand(string commandName)
        {
            if (commandName == null)
                return false;

            return GetCommand(commandName) != null;
        }

        /// <summary>
        /// 菜单
        /// </summary>
        public MenuStrip MenuStrip { get; private set; }

        /// <summary>
        /// 命令管理器
        /// </summary>
        public CommandManager CommandManager { get; private set; }

        public void Dispose()
        {
            WinFormUtility.GetAllMenuItems(MenuStrip).ForEach(menuItem => {
                menuItem.Click -= new EventHandler(menuItem_Click);
                menuItem.DropDownOpening -= new EventHandler(menuItem_DropDownOpening);
            });
        }
    }
}
