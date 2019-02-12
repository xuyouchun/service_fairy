namespace BhFairy.Test
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
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._loginButton = new System.Windows.Forms.Button();
            this._loginNameTxt = new System.Windows.Forms.TextBox();
            this._passwordTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._register = new System.Windows.Forms.Button();
            this._loginNameType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._password = new System.Windows.Forms.TextBox();
            this._loginName = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnGetProxyList = new System.Windows.Forms.Button();
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(667, 448);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "关闭";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(730, 430);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(722, 401);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "注册与登录";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._loginButton);
            this.groupBox2.Controls.Add(this._loginNameTxt);
            this.groupBox2.Controls.Add(this._passwordTxt);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 161);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(710, 234);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "登录";
            // 
            // _loginButton
            // 
            this._loginButton.Location = new System.Drawing.Point(146, 74);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(75, 23);
            this._loginButton.TabIndex = 2;
            this._loginButton.Text = "登录";
            this._loginButton.UseVisualStyleBackColor = true;
            this._loginButton.Click += new System.EventHandler(this._loginButton_Click);
            // 
            // _loginNameTxt
            // 
            this._loginNameTxt.Location = new System.Drawing.Point(66, 20);
            this._loginNameTxt.Name = "_loginNameTxt";
            this._loginNameTxt.Size = new System.Drawing.Size(155, 21);
            this._loginNameTxt.TabIndex = 0;
            // 
            // _passwordTxt
            // 
            this._passwordTxt.Location = new System.Drawing.Point(66, 47);
            this._passwordTxt.Name = "_passwordTxt";
            this._passwordTxt.Size = new System.Drawing.Size(155, 21);
            this._passwordTxt.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "密　码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "登录名：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._register);
            this.groupBox1.Controls.Add(this._loginNameType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._password);
            this.groupBox1.Controls.Add(this._loginName);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "注册";
            // 
            // _register
            // 
            this._register.Location = new System.Drawing.Point(146, 72);
            this._register.Name = "_register";
            this._register.Size = new System.Drawing.Size(75, 23);
            this._register.TabIndex = 3;
            this._register.Text = "注册";
            this._register.UseVisualStyleBackColor = true;
            this._register.Click += new System.EventHandler(this._register_Click);
            // 
            // _loginNameType
            // 
            this._loginNameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._loginNameType.FormattingEnabled = true;
            this._loginNameType.Items.AddRange(new object[] {
            "邮箱",
            "手机号",
            "自定义"});
            this._loginNameType.Location = new System.Drawing.Point(227, 18);
            this._loginNameType.Name = "_loginNameType";
            this._loginNameType.Size = new System.Drawing.Size(64, 20);
            this._loginNameType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密　码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "登录名：";
            // 
            // _password
            // 
            this._password.Location = new System.Drawing.Point(66, 45);
            this._password.Name = "_password";
            this._password.Size = new System.Drawing.Size(155, 21);
            this._password.TabIndex = 2;
            // 
            // _loginName
            // 
            this._loginName.Location = new System.Drawing.Point(66, 18);
            this._loginName.Name = "_loginName";
            this._loginName.Size = new System.Drawing.Size(155, 21);
            this._loginName.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtProxy);
            this.tabPage2.Controls.Add(this.btnGetProxyList);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(722, 401);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "代理服务器";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnGetProxyList
            // 
            this.btnGetProxyList.Location = new System.Drawing.Point(184, 115);
            this.btnGetProxyList.Name = "btnGetProxyList";
            this.btnGetProxyList.Size = new System.Drawing.Size(75, 23);
            this.btnGetProxyList.TabIndex = 0;
            this.btnGetProxyList.Text = "获取列表";
            this.btnGetProxyList.UseVisualStyleBackColor = true;
            this.btnGetProxyList.Click += new System.EventHandler(this.btnGetProxyList_Click);
            // 
            // txtProxy
            // 
            this.txtProxy.Location = new System.Drawing.Point(30, 24);
            this.txtProxy.Multiline = true;
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.Size = new System.Drawing.Size(229, 85);
            this.txtProxy.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 483);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "拨号精灵服务器端测试程序";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox _loginNameType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _password;
        private System.Windows.Forms.TextBox _loginName;
        private System.Windows.Forms.Button _register;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button _loginButton;
        private System.Windows.Forms.TextBox _loginNameTxt;
        private System.Windows.Forms.TextBox _passwordTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProxy;
        private System.Windows.Forms.Button btnGetProxyList;
    }
}

