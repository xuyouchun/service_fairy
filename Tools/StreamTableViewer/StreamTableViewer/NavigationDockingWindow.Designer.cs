namespace StreamTableViewer
{
    partial class NavigationDockingWindow
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Stream Table Files");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigationDockingWindow));
            this._tree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.Font = new System.Drawing.Font("Calisto MT", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tree.HideSelection = false;
            this._tree.ItemHeight = 20;
            this._tree.Location = new System.Drawing.Point(0, 0);
            this._tree.Name = "_tree";
            treeNode1.Name = "StreamTable";
            treeNode1.StateImageIndex = 0;
            treeNode1.Text = "Stream Table Files";
            this._tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this._tree.ShowRootLines = false;
            this._tree.Size = new System.Drawing.Size(245, 460);
            this._tree.StateImageList = this.imageList1;
            this._tree.TabIndex = 0;
            this._tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tree_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "dbs.ico");
            this.imageList1.Images.SetKeyName(1, "db.ico");
            this.imageList1.Images.SetKeyName(2, "sucaiwcom18000dd.png");
            // 
            // NavigationDockingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 460);
            this.Controls.Add(this._tree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NavigationDockingWindow";
            this.Text = "文件";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.ImageList imageList1;
    }
}