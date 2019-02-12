namespace BohaoCheckinDeamon
{
  partial class Form1
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
        this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.textBox3 = new System.Windows.Forms.TextBox();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.button1 = new System.Windows.Forms.Button();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.textBoxWeb = new System.Windows.Forms.TextBox();
        this.textBoxNotes = new System.Windows.Forms.TextBox();
        this.textBoxEmail = new System.Windows.Forms.TextBox();
        this.textBoxJob = new System.Windows.Forms.TextBox();
        this.textBoxOrg = new System.Windows.Forms.TextBox();
        this.textBoxMobile = new System.Windows.Forms.TextBox();
        this.textBoxName = new System.Windows.Forms.TextBox();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.dataGridView1 = new System.Windows.Forms.DataGridView();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.button3 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.timer2 = new System.Windows.Forms.Timer(this.components);
        this.groupBox1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.groupBox3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        this.groupBox4.SuspendLayout();
        this.SuspendLayout();
        // 
        // richTextBox1
        // 
        this.richTextBox1.BackColor = System.Drawing.Color.Gainsboro;
        this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.richTextBox1.Location = new System.Drawing.Point(6, 20);
        this.richTextBox1.Name = "richTextBox1";
        this.richTextBox1.Size = new System.Drawing.Size(176, 340);
        this.richTextBox1.TabIndex = 0;
        this.richTextBox1.Text = "";
        // 
        // textBox1
        // 
        this.textBox1.Location = new System.Drawing.Point(6, 20);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(100, 21);
        this.textBox1.TabIndex = 2;
        this.textBox1.Text = "wx4eqw88";
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.textBox3);
        this.groupBox1.Controls.Add(this.textBox2);
        this.groupBox1.Controls.Add(this.textBox1);
        this.groupBox1.Location = new System.Drawing.Point(476, 13);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(200, 84);
        this.groupBox1.TabIndex = 3;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Settings";
        // 
        // textBox3
        // 
        this.textBox3.Location = new System.Drawing.Point(150, 21);
        this.textBox3.Name = "textBox3";
        this.textBox3.Size = new System.Drawing.Size(44, 21);
        this.textBox3.TabIndex = 4;
        this.textBox3.Text = "800";
        // 
        // textBox2
        // 
        this.textBox2.Location = new System.Drawing.Point(7, 48);
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(187, 21);
        this.textBox2.TabIndex = 3;
        this.textBox2.Text = "创新工场年会";
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(476, 104);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 4;
        this.button1.Text = "Start";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.textBoxWeb);
        this.groupBox2.Controls.Add(this.textBoxNotes);
        this.groupBox2.Controls.Add(this.textBoxEmail);
        this.groupBox2.Controls.Add(this.textBoxJob);
        this.groupBox2.Controls.Add(this.textBoxOrg);
        this.groupBox2.Controls.Add(this.textBoxMobile);
        this.groupBox2.Controls.Add(this.textBoxName);
        this.groupBox2.Location = new System.Drawing.Point(476, 152);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(200, 226);
        this.groupBox2.TabIndex = 5;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Sender";
        // 
        // textBoxWeb
        // 
        this.textBoxWeb.Location = new System.Drawing.Point(7, 189);
        this.textBoxWeb.Name = "textBoxWeb";
        this.textBoxWeb.Size = new System.Drawing.Size(165, 21);
        this.textBoxWeb.TabIndex = 6;
        this.textBoxWeb.Text = "http://bohaojingling.com";
        // 
        // textBoxNotes
        // 
        this.textBoxNotes.Location = new System.Drawing.Point(7, 161);
        this.textBoxNotes.Name = "textBoxNotes";
        this.textBoxNotes.Size = new System.Drawing.Size(165, 21);
        this.textBoxNotes.TabIndex = 5;
        this.textBoxNotes.Text = "最适合中国人使用的拨号软件";
        // 
        // textBoxEmail
        // 
        this.textBoxEmail.Location = new System.Drawing.Point(7, 133);
        this.textBoxEmail.Name = "textBoxEmail";
        this.textBoxEmail.Size = new System.Drawing.Size(165, 21);
        this.textBoxEmail.TabIndex = 4;
        this.textBoxEmail.Text = "support@bohaojingling.com";
        // 
        // textBoxJob
        // 
        this.textBoxJob.Location = new System.Drawing.Point(7, 105);
        this.textBoxJob.Name = "textBoxJob";
        this.textBoxJob.Size = new System.Drawing.Size(100, 21);
        this.textBoxJob.TabIndex = 3;
        this.textBoxJob.Text = "客服电话";
        // 
        // textBoxOrg
        // 
        this.textBoxOrg.Location = new System.Drawing.Point(7, 77);
        this.textBoxOrg.Name = "textBoxOrg";
        this.textBoxOrg.Size = new System.Drawing.Size(165, 21);
        this.textBoxOrg.TabIndex = 2;
        this.textBoxOrg.Text = "北京和丰信科技有限公司";
        // 
        // textBoxMobile
        // 
        this.textBoxMobile.Location = new System.Drawing.Point(7, 49);
        this.textBoxMobile.Name = "textBoxMobile";
        this.textBoxMobile.Size = new System.Drawing.Size(165, 21);
        this.textBoxMobile.TabIndex = 1;
        this.textBoxMobile.Text = "+86 01057525200,,1053";
        // 
        // textBoxName
        // 
        this.textBoxName.Location = new System.Drawing.Point(7, 21);
        this.textBoxName.Name = "textBoxName";
        this.textBoxName.Size = new System.Drawing.Size(100, 21);
        this.textBoxName.TabIndex = 0;
        this.textBoxName.Text = "拨号精灵";
        // 
        // groupBox3
        // 
        this.groupBox3.Controls.Add(this.richTextBox1);
        this.groupBox3.Location = new System.Drawing.Point(12, 12);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(188, 366);
        this.groupBox3.TabIndex = 6;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Log";
        // 
        // timer1
        // 
        this.timer1.Interval = 3000;
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // dataGridView1
        // 
        this.dataGridView1.AllowUserToAddRows = false;
        this.dataGridView1.AllowUserToDeleteRows = false;
        this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
        this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView1.Location = new System.Drawing.Point(6, 20);
        this.dataGridView1.Name = "dataGridView1";
        this.dataGridView1.ReadOnly = true;
        this.dataGridView1.RowTemplate.Height = 23;
        this.dataGridView1.Size = new System.Drawing.Size(240, 309);
        this.dataGridView1.TabIndex = 7;
        // 
        // groupBox4
        // 
        this.groupBox4.Controls.Add(this.button3);
        this.groupBox4.Controls.Add(this.dataGridView1);
        this.groupBox4.Location = new System.Drawing.Point(207, 13);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.Size = new System.Drawing.Size(252, 366);
        this.groupBox4.TabIndex = 7;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "Data";
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(170, 335);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 8;
        this.button3.Text = "Save";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(601, 103);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 8;
        this.button2.Text = "Stop";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // timer2
        // 
        this.timer2.Interval = 20000;
        this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(688, 397);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.groupBox4);
        this.Controls.Add(this.groupBox3);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.groupBox1);
        this.Name = "Form1";
        this.Text = "拨号精灵签到";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.groupBox3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        this.groupBox4.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.TextBox textBoxOrg;
    private System.Windows.Forms.TextBox textBoxMobile;
    private System.Windows.Forms.TextBox textBoxJob;
    private System.Windows.Forms.TextBox textBoxNotes;
    private System.Windows.Forms.TextBox textBoxEmail;
    private System.Windows.Forms.TextBox textBoxWeb;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Timer timer2;
  }
}

