using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm.Docking;
using Common.Package.Storage;

namespace StreamTableViewer
{
    public partial class TableViewerDockingWindow : DockContentEx
    {
        public TableViewerDockingWindow(ViewerContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private readonly ViewerContext _context;

        public void SetData(StreamTable streamTable)
        {
            DataTable dt = streamTable.ToDataTable();
            _dg.DataSource = dt;

            _dg.SuspendLayout();
            for (int k = 0; k < _dg.Columns.Count; k++)
            {
                DataGridViewColumn column = _dg.Columns[k];
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                column.HeaderCell.ToolTipText = streamTable.Columns[k].ToString();
            }

            if (_dg.Columns.Count > 0)
                _dg.Columns[_dg.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            _dg.ResumeLayout();
        }

        private void _dg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
