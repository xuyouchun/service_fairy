namespace TestListView
{
    partial class Form1
    {
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        ///   <summary>  
        ///   Required   designer   variable.  
        ///   </summary>  
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region   Windows   Form   Designer   generated   code

        ///   <summary>  
        ///   Required   method   for   Designer   support   -   do   not   modify  
        ///   the   contents   of   this   method   with   the   code   editor.  
        ///   </summary>  
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            //    
            //   listView1  
            //    
            this.listView1.AllowDrop = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]   {  
  this.columnHeader1,  
  this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.FullRowSelect = true;
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(232, 176);
            this.listView1.TabIndex = 0;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            //    
            //   columnHeader1  
            //    
            this.columnHeader1.Text = "COL1";
            this.columnHeader1.Width = 100;
            //    
            //   columnHeader3  
            //    
            this.columnHeader3.Text = "COL2";
            this.columnHeader3.Width = 100;
            //    
            //   splitter1
            //    
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 176);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(232, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            //    
            //   listView2  
            //    
            this.listView2.AllowDrop = true;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]   {  
  this.columnHeader2,  
  this.columnHeader5});
            this.listView2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.FullRowSelect = true;
            this.listView2.Location = new System.Drawing.Point(0, 179);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(232, 226);
            this.listView2.TabIndex = 2;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView2_MouseDown);
            this.listView2.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView2_DragDrop);
            this.listView2.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView2_DragEnter);
            this.listView2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView2_MouseMove);
            //    
            //   columnHeader2  
            //    
            this.columnHeader2.Text = "COL1";
            this.columnHeader2.Width = 100;
            //    
            //   columnHeader5  
            //    
            this.columnHeader5.Text = "COL2";
            this.columnHeader5.Width = 100;
            //    
            //   Form1  
            //    
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(232, 405);
            this.Controls.AddRange(new System.Windows.Forms.Control[]   {  
      this.listView2,  
      this.splitter1,  
      this.listView1});
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }
        #endregion
    }
}

