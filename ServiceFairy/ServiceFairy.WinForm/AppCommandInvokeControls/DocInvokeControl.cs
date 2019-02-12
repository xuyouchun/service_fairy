using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    /// <summary>
    /// 显示参数的文档
    /// </summary>
    public partial class DocInvokeControl : UserControl
    {
        public DocInvokeControl()
        {
            InitializeComponent();
        }

        private static readonly Dictionary<string, TypeDoc> _emptyDict = new Dictionary<string, TypeDoc>();

        public void SetDoc(TypeDocTree typeDocTree)
        {
            if (typeDocTree == null)
                return;

            Dictionary<string, TypeDoc> subTypeDocDict = typeDocTree.SubTypeDocs == null ? _emptyDict :
                typeDocTree.SubTypeDocs.ToDictionary(doc => doc.TypeFullName, true);

            _AppendNode(_tree.Nodes, typeDocTree.Root, subTypeDocDict);
            _tree.ExpandAll();
        }

        private void _AppendNode(TreeNodeCollection nodes, TypeDoc typeDoc, Dictionary<string, TypeDoc> subTypeDocDict)
        {
            TreeNode node = new TreeNode() {
                Name = typeDoc.Name,
                Text = typeDoc.Name + "<" + typeDoc.TypeShortName + ">"
            };

            nodes.Add(node);

            foreach (FieldDoc fieldDoc in typeDoc.FieldDocs)
            {
                string fullName = fieldDoc.TypeFullName;
                TypeDoc td;
                if (!subTypeDocDict.TryGetValue(fullName, out td))
                    continue;

                _AppendNode(node.Nodes, td, subTypeDocDict);
            }
        }
    }
}
