using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LitJson;
using System.IO;

namespace BohaoCheckinDeamon
{
  public partial class Form1 : Form
  {
    public const string ConstValidCodeHashPasscode = "BoHaoJingLing@2011_05_04";
    public static string ConstUserId = Guid.NewGuid().ToString("N");

    private int StartNum;

    private DataTable MyContactData = new DataTable();
    private HashSet<string> MyContactIds = new HashSet<string>();

    public Form1()
    {
      InitializeComponent();

      this.MyContactData.Columns.Add("id");
      this.MyContactData.Columns.Add("name");
      this.MyContactData.Columns.Add("com");
      this.MyContactData.Columns.Add("jobtitle");
      this.MyContactData.Columns.Add("phone");
      this.MyContactData.Columns.Add("sender");
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.dataGridView1.DataSource = this.MyContactData;
      textBoxJob.Text = ConstUserId;
    }

    protected string DoHash(string strForHash)
    {
      return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strForHash + ConstValidCodeHashPasscode, "md5");
    }
    private void Log(object log, bool ln)
    {
      this.richTextBox1.Text += String.Format("{0}{1}", log, (ln) ? "\n" : String.Empty);
    }

    private void DoPing()
    {
      this.Log("ping... ", false);

      Dictionary<string, string> myPostData = new Dictionary<string, string>();

      string myGeoHashId = this.textBox1.Text;
      string myUserId = ConstUserId;
      string myUserName = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBox2.Text));
      string myValidCode = this.DoHash(myGeoHashId + myUserId + myUserName);

      myPostData.Add("GEOHASHID", myGeoHashId);
      myPostData.Add("USERID", myUserId);
      myPostData.Add("USERNAME", myUserName.Replace("+", "%2B"));
      myPostData.Add("VALIDCODE", myValidCode);

      Conn myConn = new Conn();

      string retPing = myConn.PostData("http://ns01.bohaojingling.com/ping", myPostData);
      if (String.IsNullOrEmpty(retPing))
      {
        this.Log("!", true);
        return;
      }

      if (retPing == "{}")
      {
        this.Log("-", true);
        return;
      }

      JsonData lj = JsonMapper.ToObject(retPing);

      for (int i = 0; i < lj["list"].Count;i++)
      {
        this.Log(String.Format("data {0}\n----------", i), true);
        JsonData jd = lj["list"][i];

        if (jd["myDocument"].Count < 2)
          continue;

        string senderId = jd["senderId"].ToString();
        string senderName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["senderName"].ToString()));
        string contactId = jd["contactId"].ToString();
        string contactName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["contactName"].ToString()));

        string firstname = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["firstname"].ToString()));
        string middlename = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["middlename"].ToString()));
        string lastname = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["lastname"].ToString()));
        string nickname = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["nickname"].ToString()));

        string organization = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["organization"].ToString()));
        string department = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["department"].ToString()));
        string jobtitle = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["jobtitle"].ToString()));


        string jsPhones = (!((IDictionary)jd["myDocument"]).Contains("dphones")) ? String.Empty : jd["myDocument"]["dphones"].ToString();
        //string emailaddress = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(jd["myDocument"]["emailaddress"].ToString()));

        List<string> listPhones = new List<string>();
        if (!String.IsNullOrEmpty(jsPhones))
        {
            JsonData jdPhones = JsonMapper.ToObject(jsPhones);
            for (int j = 0; j < jdPhones.Count; j += 2)
                listPhones.Add(Encoding.UTF8.GetString(Convert.FromBase64String(jdPhones[j].ToString())));
        }

        string phone = String.Join(" , ", listPhones.ToArray());
        //this.Log(senderId, true);
        this.Log(senderName, true);
        //this.Log(contactId, true);
        this.Log(contactName, true);
        this.Log(organization, true);
        this.Log(department, true);
        this.Log(jobtitle, true);
        this.Log(phone, true);
        //this.Log(phonenumberlabels, true);
        //this.Log(phonenumbers, true);
        //this.Log(emailaddress, true);

        this.Log("----------", true);

        if (this.MyContactIds.Contains(contactId))
          continue;

        this.StartNum++;
        DataRow dr = this.MyContactData.NewRow();
        dr["id"] = this.StartNum;
        dr["name"] = contactName;
        dr["com"] = organization;
        dr["jobtitle"] = jobtitle;
        dr["phone"] = phone;
        dr["sender"] = senderName;
        this.MyContactData.Rows.Add(dr);

        this.MyContactIds.Add(contactId);

        this.doSend(senderId, contactName);
      }

      this.textBox3.Text = this.StartNum.ToString();
    }

    private void doSend(string receiverIds,string cn)
    {
      this.Log("send... ", false);

      Dictionary<string, string> myPostData = new Dictionary<string, string>();

      string myUserId = ConstUserId;
      string myUserName = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBox2.Text));
      string myContactId = ConstUserId + "-" + this.StartNum.ToString();
      string contactName = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBoxName.Text + " / " + this.StartNum.ToString()));

      string contactMobile = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBoxMobile.Text));
      string contactOrg = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("参加人: " + cn));//this.textBoxOrg.Text));
      string contactJob = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("抽奖号: " + this.StartNum.ToString()));//this.textBoxJob.Text));
      string contactNotes = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBoxNotes.Text));
      string contactEmail = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBoxEmail.Text));
      string contactWeb = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.textBoxWeb.Text));

      string myValidCode = this.DoHash(myUserId + myUserName + myContactId + contactName + receiverIds);

      myPostData.Add("USERID", myUserId);
      myPostData.Add("USERNAME", myUserName.Replace("+", "%2B"));
      myPostData.Add("CONTACTID", myContactId);
      myPostData.Add("CONTACTNAME", contactName.Replace("+", "%2B"));
      myPostData.Add("RECEIVERIDS", receiverIds);
      myPostData.Add("VALIDCODE", myValidCode);


      JsonData jd = new JsonData();
      jd["note"] = new JsonData(contactNotes.Replace("+", "%2B"));
      jd["lastname"] = new JsonData(contactName.Replace("+", "%2B"));
      jd["organization"] = new JsonData(contactOrg.Replace("+", "%2B"));
      jd["jobtitle"] = new JsonData(contactJob.Replace("+", "%2B"));
      jd["emailaddress"] = new JsonData(contactEmail.Replace("+", "%2B"));
      //jd["url"] = new JsonData(contactWeb);
      jd["phonenumberlabels"] = "";//"YnBsaXN0MDDUAQIDBAUIGRpUJHRvcFgkb2JqZWN0c1gkdmVyc2lvblkkYXJjaGl2ZXLRBgdUcm9vdIABpAkKEBFVJG51bGzSCwwND1pOUy5vYmplY3RzViRjbGFzc6EOgAKAA15fJCE8TW9iaWxlPiEkX9ISExQYWCRjbGFzc2VzWiRjbGFzc25hbWWjFRYXXk5TTXV0YWJsZUFycmF5V05TQXJyYXlYTlNPYmplY3ReTlNNdXRhYmxlQXJyYXkSAAGGoF8QD05TS2V5ZWRBcmNoaXZlcggRFh8oMjU6PEFHTFdeYGJkc3iBjJCfp7C/xAAAAAAAAAEBAAAAAAAAABsAAAAAAAAAAAAAAAAAAADW".Replace("+", "%2B");
      jd["phonenumbers"] = "";//"YnBsaXN0MDDUAQIDBAUIGRpUJHRvcFgkb2JqZWN0c1gkdmVyc2lvblkkYXJjaGl2ZXLRBgdUcm9vdIABpAkKEBFVJG51bGzSCwwND1pOUy5vYmplY3RzViRjbGFzc6EOgAKAA18QFSs4NiAwMTA1NzUyNTIwMCwsMTA1M9ISExQYWCRjbGFzc2VzWiRjbGFzc25hbWWjFRYXXk5TTXV0YWJsZUFycmF5V05TQXJyYXlYTlNPYmplY3ReTlNNdXRhYmxlQXJyYXkSAAGGoF8QD05TS2V5ZWRBcmNoaXZlcggRFh8oMjU6PEFHTFdeYGJkfIGKlZmosLnIzQAAAAAAAAEBAAAAAAAAABsAAAAAAAAAAAAAAAAAAADf".Replace("+", "%2B");

      jd["firstname"] = new JsonData(String.Empty);
      jd["aaddress"] = new JsonData(String.Empty);
      jd["nickname"] = new JsonData(String.Empty);
      jd["email"] = new JsonData(String.Empty);
      jd["address"] = new JsonData(String.Empty);
      jd["middlename"] = new JsonData(String.Empty);
      jd["image"] = new JsonData(String.Empty);
      jd["department"] = new JsonData(String.Empty);

      myPostData.Add("NCDOC", jd.ToJson());

      Conn myConn = new Conn();

      string retSend = myConn.PostData("http://ns01.bohaojingling.com/send", myPostData);
      if (String.IsNullOrEmpty(retSend))
      {
        this.Log("!", true);
        return;
      }

      this.Log(retSend, true);
    }

    private void DoSaveContactList()
    {
      List<string> output = new List<string>();

      foreach (DataRow dr in this.MyContactData.Rows)
      {
        List<string> r = new List<string>();
        foreach (DataColumn dc in this.MyContactData.Columns)
          r.Add(dr[dc].ToString());

        output.Add(String.Join("\t", r.ToArray()));
      }

      File.WriteAllLines("contact-list.txt", output.ToArray(), Encoding.Unicode);

      this.Log(String.Empty, true);
      this.Log("cl saved!\n", true);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.StartNum = Convert.ToInt32(this.textBox3.Text);

      //this.DoPing();

      this.Log(">", true);
      this.timer1.Start();
      this.timer2.Start();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.timer1.Stop();
      this.timer2.Stop();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.DoPing();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.DoSaveContactList();
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
      this.DoSaveContactList();
    }
  }
}
