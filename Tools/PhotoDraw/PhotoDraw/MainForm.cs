using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PhotoDraw
{
  public partial class MainForm : Form
  {
    public string photosFolderPath;
    public PhotoPlayerForm photoPlayerForm;
    Queue<Dictionary<string, string>> myContactQueue = new Queue<Dictionary<string, string>>();

    public MainForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
      {
        photosFolderPath = folderBrowserDialog1.SelectedPath;
        this.GenPhotoList();
        button2.Show();
      }
    }

    private void GenPhotoList()
    {
      this.myContactQueue.Clear();

      List<Dictionary<string, string>> defContactList = new List<Dictionary<string, string>>();

      string filePath = this.photosFolderPath + "\\contact-list.txt";
      string strData = File.ReadAllText(filePath, Encoding.Unicode);

      string[] myData = strData.Split(new char[] { '\n' });

      int myDataCount = myData.Length;
      int curContactNo = 1;
      foreach (string line in myData)
      {
        if (String.IsNullOrEmpty(line))
          continue;

        label6.Text = String.Format("{0} 联系人名单加载中... {1}/{2}", GetShowWheel(), curContactNo, myDataCount);
        this.Refresh();

        string[] d = line.Split(new char[] { '\t' });

        Dictionary<string, string> dataNode = new Dictionary<string, string>
        {
          {"id", d[0]},
          {"name", d[1]},
          {"com", d[2]},
          {"jobtitle", d[3]},
          {"phone", d[4]},
          {"sender", d[5]}
        };

        defContactList.Add(dataNode);

        curContactNo++;
      }

      Random r = new Random();
      int pc = defContactList.Count;
      while (pc > 0)
      {
        label6.Text = String.Format("{0} 名单列表随机生成中... {1}/{2}", GetShowWheel(), pc, myDataCount);
        this.Refresh();

        int s = r.Next(pc);
        Dictionary<string, string> d = defContactList[s];
        this.myContactQueue.Enqueue(d);
        defContactList.RemoveAt(s);

        pc = defContactList.Count;
      }

      label6.Text = "名单准备完成 " + this.myContactQueue.Count.ToString();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      label2.Text = "作者：王博";
      linkLabel1.Text = "wangbo@bohaojingling.com";
      label5.Text = "beta 0.1";
      button2.Hide();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (this.myContactQueue.Count == 0)
      {
        return;
      }

      this.photoPlayerForm = new PhotoPlayerForm();
      this.photoPlayerForm.myContactsQueue = this.myContactQueue;
      photoPlayerForm.Show();
    }

    private static int IdxWheel = 0;
    private static char GetShowWheel()
    {
      char retSymbol = "-\\|/".ToCharArray()[IdxWheel];
      IdxWheel = (IdxWheel >= 3) ? 0 : IdxWheel + 1;

      return retSymbol;
    }
  }
}
