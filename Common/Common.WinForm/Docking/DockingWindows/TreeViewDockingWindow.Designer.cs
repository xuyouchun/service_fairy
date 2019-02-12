namespace Common.WinForm.Docking.DockingWindows
{
    partial class TreeViewDockingWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeViewDockingWindow));
            this._tree = new System.Windows.Forms.TreeView();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.FullRowSelect = true;
            this._tree.HideSelection = false;
            this._tree.ImageIndex = 0;
            this._tree.ImageList = this._imageList;
            this._tree.ItemHeight = 20;
            this._tree.Location = new System.Drawing.Point(0, 0);
            this._tree.Name = "_tree";
            this._tree.SelectedImageIndex = 0;
            this._tree.ShowNodeToolTips = true;
            this._tree.ShowRootLines = false;
            this._tree.Size = new System.Drawing.Size(246, 510);
            this._tree.TabIndex = 0;
            this._tree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this._tree_AfterExpand);
            this._tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tree_AfterSelect);
            this._tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._tree_NodeMouseDoubleClick);
            this._tree.DoubleClick += new System.EventHandler(this._tree_DoubleClick);
            // 
            // _imageList
            // 
            this._imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._imageList.ImageSize = new System.Drawing.Size(16, 16);
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TreeViewDockingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 510);
            this.Controls.Add(this._tree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TreeViewDockingWindow";
            this.Text = "树";
            this.Load += new System.EventHandler(this.TreeViewDockingWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.ImageList _imageList;
    }
}