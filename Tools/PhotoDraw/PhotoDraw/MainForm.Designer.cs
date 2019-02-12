namespace PhotoDraw
{
  partial class MainForm
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
      this.label6 = new System.Windows.Forms.Label();
      this.button2 = new System.Windows.Forms.Button();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("SimSun", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(85, 35);
      this.label1.TabIndex = 0;
      this.label1.Text = "抽奖";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.label2.ForeColor = System.Drawing.Color.Gray;
      this.label2.Location = new System.Drawing.Point(15, 60);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(10, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = " ";
      // 
      // label4
      // 
      this.label4.ForeColor = System.Drawing.Color.Black;
      this.label4.Location = new System.Drawing.Point(16, 131);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(125, 23);
      this.label4.TabIndex = 2;
      this.label4.Text = "请选择名单：";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // button1
      // 
      this.button1.ForeColor = System.Drawing.Color.Blue;
      this.button1.Location = new System.Drawing.Point(178, 131);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "点击选择";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label5
      // 
      this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label5.Location = new System.Drawing.Point(178, 9);
      this.label5.Name = "label5";
      this.label5.Padding = new System.Windows.Forms.Padding(2);
      this.label5.Size = new System.Drawing.Size(75, 18);
      this.label5.TabIndex = 4;
      this.label5.Text = "拨号精灵";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label6
      // 
      this.label6.ForeColor = System.Drawing.Color.Black;
      this.label6.Location = new System.Drawing.Point(16, 174);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(151, 24);
      this.label6.TabIndex = 2;
      this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.label6.UseMnemonic = false;
      // 
      // button2
      // 
      this.button2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.button2.ForeColor = System.Drawing.Color.Red;
      this.button2.Location = new System.Drawing.Point(178, 174);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 24);
      this.button2.TabIndex = 5;
      this.button2.Text = "开始抽奖";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // linkLabel1
      // 
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new System.Drawing.Point(16, 87);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new System.Drawing.Size(0, 12);
      this.linkLabel1.TabIndex = 6;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.ClientSize = new System.Drawing.Size(265, 207);
      this.Controls.Add(this.linkLabel1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "MainForm";
      this.Text = "抽奖";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.LinkLabel linkLabel1;

  }
}

