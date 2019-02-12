namespace ServiceFairy.Cluster.WinForm.ObjectViewers
{
    partial class AppClientObjectViewControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._txtClientId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._txtStatus = new System.Windows.Forms.TextBox();
            this._btnStartOrStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client ID:";
            // 
            // _txtClientId
            // 
            this._txtClientId.Location = new System.Drawing.Point(87, 3);
            this._txtClientId.Name = "_txtClientId";
            this._txtClientId.Size = new System.Drawing.Size(263, 21);
            this._txtClientId.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "状态:";
            // 
            // _txtStatus
            // 
            this._txtStatus.Location = new System.Drawing.Point(87, 30);
            this._txtStatus.Name = "_txtStatus";
            this._txtStatus.Size = new System.Drawing.Size(263, 21);
            this._txtStatus.TabIndex = 1;
            // 
            // _btnStartOrStop
            // 
            this._btnStartOrStop.Location = new System.Drawing.Point(275, 57);
            this._btnStartOrStop.Name = "_btnStartOrStop";
            this._btnStartOrStop.Size = new System.Drawing.Size(75, 23);
            this._btnStartOrStop.TabIndex = 2;
            this._btnStartOrStop.Text = "启动";
            this._btnStartOrStop.UseVisualStyleBackColor = true;
            this._btnStartOrStop.Click += new System.EventHandler(this._btnStartOrStop_Click);
            // 
            // AppClientObjectViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._btnStartOrStop);
            this.Controls.Add(this._txtStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._txtClientId);
            this.Controls.Add(this.label1);
            this.Name = "AppClientObjectViewControl";
            this.Size = new System.Drawing.Size(423, 244);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _txtClientId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _txtStatus;
        private System.Windows.Forms.Button _btnStartOrStop;
    }
}
