namespace PhotoDraw
{
  partial class PhotoPlayerForm
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
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.labelNameBack = new System.Windows.Forms.Label();
      this.labelId = new System.Windows.Forms.Label();
      this.labelCom = new System.Windows.Forms.Label();
      this.labelJob = new System.Windows.Forms.Label();
      this.labelName = new System.Windows.Forms.Label();
      this.labelPhone = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // timer1
      // 
      this.timer1.Interval = 50;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // labelNameBack
      // 
      this.labelNameBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.labelNameBack.BackColor = System.Drawing.Color.Transparent;
      this.labelNameBack.Font = new System.Drawing.Font("SimSun", 123.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.labelNameBack.ForeColor = System.Drawing.Color.Black;
      this.labelNameBack.Location = new System.Drawing.Point(12, 149);
      this.labelNameBack.Name = "labelNameBack";
      this.labelNameBack.Size = new System.Drawing.Size(636, 172);
      this.labelNameBack.TabIndex = 1;
      this.labelNameBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelId
      // 
      this.labelId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelId.BackColor = System.Drawing.Color.Transparent;
      this.labelId.Font = new System.Drawing.Font("SimSun", 96F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.labelId.ForeColor = System.Drawing.Color.White;
      this.labelId.Location = new System.Drawing.Point(348, 9);
      this.labelId.Name = "labelId";
      this.labelId.Size = new System.Drawing.Size(300, 110);
      this.labelId.TabIndex = 1;
      this.labelId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelCom
      // 
      this.labelCom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.labelCom.BackColor = System.Drawing.Color.Transparent;
      this.labelCom.Font = new System.Drawing.Font("SimSun", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.labelCom.ForeColor = System.Drawing.Color.White;
      this.labelCom.Location = new System.Drawing.Point(12, 392);
      this.labelCom.Name = "labelCom";
      this.labelCom.Size = new System.Drawing.Size(636, 82);
      this.labelCom.TabIndex = 1;
      this.labelCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelJob
      // 
      this.labelJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.labelJob.BackColor = System.Drawing.Color.Transparent;
      this.labelJob.Font = new System.Drawing.Font("SimSun", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.labelJob.ForeColor = System.Drawing.Color.White;
      this.labelJob.Location = new System.Drawing.Point(12, 492);
      this.labelJob.Name = "labelJob";
      this.labelJob.Size = new System.Drawing.Size(636, 90);
      this.labelJob.TabIndex = 1;
      this.labelJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelName
      // 
      this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.labelName.BackColor = System.Drawing.Color.Transparent;
      this.labelName.Font = new System.Drawing.Font("SimSun", 123.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.labelName.ForeColor = System.Drawing.Color.White;
      this.labelName.Location = new System.Drawing.Point(12, 171);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(636, 172);
      this.labelName.TabIndex = 2;
      this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelPhone
      // 
      this.labelPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.labelPhone.BackColor = System.Drawing.Color.Transparent;
      this.labelPhone.Font = new System.Drawing.Font("SimSun", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.labelPhone.ForeColor = System.Drawing.Color.White;
      this.labelPhone.Location = new System.Drawing.Point(12, 331);
      this.labelPhone.Name = "labelPhone";
      this.labelPhone.Size = new System.Drawing.Size(636, 53);
      this.labelPhone.TabIndex = 1;
      this.labelPhone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // PhotoPlayerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.ClientSize = new System.Drawing.Size(660, 605);
      this.Controls.Add(this.labelPhone);
      this.Controls.Add(this.labelName);
      this.Controls.Add(this.labelId);
      this.Controls.Add(this.labelJob);
      this.Controls.Add(this.labelCom);
      this.Controls.Add(this.labelNameBack);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.KeyPreview = true;
      this.Name = "PhotoPlayerForm";
      this.ShowInTaskbar = false;
      this.TopMost = true;
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Load += new System.EventHandler(this.PhotoPlayerForm_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PhotoPlayerForm_KeyDown);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PhotoPlayerForm_MouseClick);
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Label labelNameBack;
    private System.Windows.Forms.Label labelId;
    private System.Windows.Forms.Label labelCom;
    private System.Windows.Forms.Label labelJob;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.Label labelPhone;

  }
}