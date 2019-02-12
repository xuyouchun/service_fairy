using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar;
using Common.WinForm;
using System.ComponentModel;
using Common.Utility;

namespace ServiceFairy.Management.WinForm
{
	partial class MainForm
	{
        class XUIOperation : UIOperation
        {
            public XUIOperation(MainForm mainform)
                : base(mainform)
            {
                _mainform = mainform;

                _componentDict = new Dictionary<string, IComponent> {
                    { "connection", _mainform._tsConnectionStatus },
                };
            }

            private readonly MainForm _mainform;
            private readonly SuperTooltip _tooltip = new SuperTooltip();

            private readonly Dictionary<string, IComponent> _componentDict;

            public override void ShowPopup(string category, string message, string title, UIOperationPopupType type, TimeSpan autoCloseTime)
            {
                SuperTooltipInfo info = new SuperTooltipInfo() {
                    BodyText = message ?? string.Empty,
                    HeaderText = title ?? string.Empty,
                    HeaderVisible = !string.IsNullOrEmpty(title),
                };

                IComponent component = _componentDict.GetOrDefault(category) ?? _mainform._tsConnectionStatus;
                if (component != null)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        _tooltip.SetSuperTooltip(component, info);
                        _tooltip.ShowTooltip(null);
                    }
                    else
                    {
                        _tooltip.SetSuperTooltip(component, null);
                    }
                }
            }
        }
	}
}
