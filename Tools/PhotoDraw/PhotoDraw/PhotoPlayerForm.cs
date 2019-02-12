using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Resources;
using PhotoDraw.Properties;

namespace PhotoDraw
{
  public partial class PhotoPlayerForm : Form
  {
    public Queue<Dictionary<string,string>> myContactsQueue = new Queue<Dictionary<string,string>>();

    public PhotoPlayerForm()
    {
      InitializeComponent();
    }

    private void PhotoPlayerForm_Load(object sender, EventArgs e)
    {
      this.SuspendLayout();
      this.labelId.Top = 10;
      this.labelId.Left = this.Width - this.labelId.Width - 10;

      this.labelName.Left = 10;
      this.labelName.Top = (int)(this.Height * 0.5) - this.labelNameBack.Height;
      this.labelName.Width = this.Width - 20;

      this.labelNameBack.Left = 11;
      this.labelNameBack.Top = (int)(this.Height * 0.5) - this.labelNameBack.Height + 1;
      this.labelNameBack.Width = this.Width - 20;
      /*

this.labelCom.Left = 10;
this.labelCom.Top = this.labelNameBack.Top + this.labelNameBack.Height + 40;
this.labelCom.Width = this.Width - 20;
*/
      this.labelJob.Left = 10;
      this.labelJob.Top = this.labelCom.Top + this.labelCom.Height + 20;
      this.labelJob.Width = this.Width - 20;
      this.ResumeLayout();

      this.labelNameBack.Controls.Add(this.labelName);
      this.labelName.Location = new Point(4, 4);

      this.BackgroundImage = Resources.抽奖背景;
      timer1.Start();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      Dictionary<string, string> d = this.myContactsQueue.Dequeue();
      this.labelName.Text = d["name"];
      this.labelNameBack.Text = d["name"];
      this.labelId.Text = d["id"];
      this.labelCom.Text = d["com"];
      this.labelJob.Text = d["jobtitle"];
      this.labelPhone.Text = d["phone"];
      this.myContactsQueue.Enqueue(d);
    }

    private void PhotoPlayerForm_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Space:
          timer1.Stop();
          break;
        case Keys.Tab:
          timer1.Start();
          break;
        case Keys.Escape:
          this.Close();
          break;
        default: break;
      }
    }

    private void PhotoPlayerForm_MouseClick(object sender, MouseEventArgs e)
    {
      switch (e.Button)
      {
        case MouseButtons.Left:
          timer1.Stop();
          break;
        case MouseButtons.Right:
          timer1.Start();
          break;
        default: break;
      }
    }
  }
}
