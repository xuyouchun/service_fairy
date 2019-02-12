namespace ServiceFairy.Client.WinForm
{
    partial class UserMainWindow
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
            this._statusTextBox = new System.Windows.Forms.TextBox();
            this._outputTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._onlineCheckBox = new System.Windows.Forms.CheckBox();
            this._firstNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._lastNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._changeInfoButton = new System.Windows.Forms.Button();
            this._statusChangedButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _statusTextBox
            // 
            this._statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._statusTextBox.Location = new System.Drawing.Point(79, 20);
            this._statusTextBox.Name = "_statusTextBox";
            this._statusTextBox.Size = new System.Drawing.Size(398, 21);
            this._statusTextBox.TabIndex = 0;
            this._statusTextBox.TextChanged += new System.EventHandler(this._statusTextBox_TextChanged);
            this._statusTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._statusTextBox_KeyDown);
            // 
            // _outputTextBox
            // 
            this._outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._outputTextBox.BackColor = System.Drawing.SystemColors.Window;
            this._outputTextBox.ContextMenuStrip = this.contextMenuStrip1;
            this._outputTextBox.Location = new System.Drawing.Point(12, 187);
            this._outputTextBox.Name = "_outputTextBox";
            this._outputTextBox.ReadOnly = true;
            this._outputTextBox.Size = new System.Drawing.Size(564, 140);
            this._outputTextBox.TabIndex = 2;
            this._outputTextBox.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // _clearToolStripMenuItem
            // 
            this._clearToolStripMenuItem.Name = "_clearToolStripMenuItem";
            this._clearToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this._clearToolStripMenuItem.Text = "清空";
            this._clearToolStripMenuItem.Click += new System.EventHandler(this._clearToolStripMenuItem_Click);
            // 
            // _onlineCheckBox
            // 
            this._onlineCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._onlineCheckBox.AutoSize = true;
            this._onlineCheckBox.Checked = true;
            this._onlineCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._onlineCheckBox.Location = new System.Drawing.Point(510, 53);
            this._onlineCheckBox.Name = "_onlineCheckBox";
            this._onlineCheckBox.Size = new System.Drawing.Size(48, 16);
            this._onlineCheckBox.TabIndex = 3;
            this._onlineCheckBox.Text = "在线";
            this._onlineCheckBox.UseVisualStyleBackColor = true;
            this._onlineCheckBox.CheckedChanged += new System.EventHandler(this._onlineCheckBox_CheckedChanged);
            // 
            // _firstNameTextBox
            // 
            this._firstNameTextBox.Location = new System.Drawing.Point(79, 27);
            this._firstNameTextBox.Name = "_firstNameTextBox";
            this._firstNameTextBox.Size = new System.Drawing.Size(137, 21);
            this._firstNameTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "FirstName:";
            // 
            // _lastNameTextBox
            // 
            this._lastNameTextBox.Location = new System.Drawing.Point(321, 27);
            this._lastNameTextBox.Name = "_lastNameTextBox";
            this._lastNameTextBox.Size = new System.Drawing.Size(137, 21);
            this._lastNameTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "LastName:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._changeInfoButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._firstNameTextBox);
            this.groupBox1.Controls.Add(this._lastNameTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(564, 85);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "我的信息";
            // 
            // _changeInfoButton
            // 
            this._changeInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._changeInfoButton.Location = new System.Drawing.Point(483, 56);
            this._changeInfoButton.Name = "_changeInfoButton";
            this._changeInfoButton.Size = new System.Drawing.Size(75, 23);
            this._changeInfoButton.TabIndex = 6;
            this._changeInfoButton.Text = "确认";
            this._changeInfoButton.UseVisualStyleBackColor = true;
            this._changeInfoButton.Click += new System.EventHandler(this._changeInfoButton_Click);
            // 
            // _statusChangedButton
            // 
            this._statusChangedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._statusChangedButton.Location = new System.Drawing.Point(483, 20);
            this._statusChangedButton.Name = "_statusChangedButton";
            this._statusChangedButton.Size = new System.Drawing.Size(75, 23);
            this._statusChangedButton.TabIndex = 7;
            this._statusChangedButton.Text = "确认";
            this._statusChangedButton.UseVisualStyleBackColor = true;
            this._statusChangedButton.Click += new System.EventHandler(this._statusChangedButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this._statusChangedButton);
            this.groupBox2.Controls.Add(this._statusTextBox);
            this.groupBox2.Controls.Add(this._onlineCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(564, 75);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "我的状态";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "个性签名：";
            // 
            // UserMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 339);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._outputTextBox);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Name = "UserMainWindow";
            this.Text = "我的信息";
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox _statusTextBox;
        private System.Windows.Forms.RichTextBox _outputTextBox;
        private System.Windows.Forms.CheckBox _onlineCheckBox;
        private System.Windows.Forms.TextBox _firstNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _lastNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button _changeInfoButton;
        private System.Windows.Forms.Button _statusChangedButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _clearToolStripMenuItem;
    }
}