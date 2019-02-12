using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Framework.Management.DockingWindow;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Framework.Management;

namespace ServiceFairy.Management.WinForm
{
    class NavigationItemCollection : INavigation
    {
        public NavigationItemCollection(INavigation[] navigationItems)
        {
            Contract.Requires(navigationItems != null);
            _navigationItems = navigationItems;
        }

        private readonly INavigation[] _navigationItems;

        public void Show(IUIObjectExecuteContext executeContext, IServiceObjectProvider provider)
        {
            foreach (INavigation navigationItem in _navigationItems)
            {
                navigationItem.Show(executeContext, provider);
            }
        }

        public void SetCurrent(IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            
        }
    }

    class NavigationItemAdapter : INavigation
    {
        public NavigationItemAdapter(ITreeViewWindow treeViewWindow)
        {
            _treeViewWindow = treeViewWindow;
        }

        private readonly ITreeViewWindow _treeViewWindow;

        public void Show(IUIObjectExecuteContext executeContext, IServiceObjectProvider provider)
        {
            _treeViewWindow.Show(executeContext, provider.GetTree().Root, 2);
        }

        public void SetCurrent(IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            _treeViewWindow.SetCurrent(serviceObject);
        }
    }
}
