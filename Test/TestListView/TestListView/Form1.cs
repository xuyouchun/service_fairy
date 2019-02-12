using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TestListView
{
    public partial class Form1 : Form
    {   
        bool lv1_mdown = false;
        bool lv2_mdown = false;

        public Form1()
        {
            //  
            //   Required   for   Windows   Form   Designer   support  
            //  
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            ImageList il = new ImageList();
            //il.Images.Add(new System.Drawing.Icon("E:\\林APP\\WinFormCollection\\DragDropListView \\TICK.ico"));//D:\\smk\\odlv\\tick.ico  
            listView1.SmallImageList = il;

            ImageList i2 = new ImageList();
            //i2.Images.Add(new System.Drawing.Icon("E:\\林APP\\WinFormCollection\\DragDropListView \\KEY04.ICO"));//D:\\smk\\odlv\\key04.ico  
            listView2.SmallImageList = i2;


            string[] items = new string[2];
            items[0] = "LA"; items[1] = "Los   Angeles";
            listView1.Items.Add(new ListViewItem(items, 0));
            items[0] = "WA"; items[1] = "Seattle";
            listView1.Items.Add(new ListViewItem(items, 0));
            items[0] = "IL"; items[1] = "Chicago";
            listView1.Items.Add(new ListViewItem(items, 0));

            items[0] = "FR"; items[1] = "Paris";
            listView2.Items.Add(new ListViewItem(items, 0));
            items[0] = "BR"; items[1] = "London";
            listView2.Items.Add(new ListViewItem(items, 0));
            items[0] = "IN"; items[1] = "Mumbai";
            listView2.Items.Add(new ListViewItem(items, 0));

        }

        private void listView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string textBox1 = e.Data.GetData(DataFormats.Text).ToString();
            string[] items = textBox1.Split(',');
            listView1.Items.Add(new ListViewItem(items, 0));
            lv1_mdown = false;
            lv2_mdown = false;
        }
        private void listView2_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string textBox1 = e.Data.GetData(DataFormats.Text).ToString();
            string[] items = textBox1.Split(',');
            listView2.Items.Add(new ListViewItem(items, 0));
            lv2_mdown = false;
            lv1_mdown = false;
        }

        private void listView2_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!lv1_mdown) return;
            if (e.Button == MouseButtons.Right) return;

            string str = GetItemText(listView1);
            if (str == "") return;

            listView1.DoDragDrop(str, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void listView2_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!lv2_mdown) return;
            if (e.Button == MouseButtons.Right) return;

            string str = GetItemText(listView2);
            if (str == "") return;

            listView2.DoDragDrop(str, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void listView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lv1_mdown = true;
        }

        private void listView2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lv2_mdown = true;
        }

        public string GetItemText(ListView LVIEW)
        {
            int nTotalSelected = LVIEW.SelectedIndices.Count;
            if (nTotalSelected <= 0) return "";
            IEnumerator selCol = LVIEW.SelectedItems.GetEnumerator();
            selCol.MoveNext();
            ListViewItem lvi = (ListViewItem)selCol.Current;
            string mDir = "";
            for (int i = 0; i < lvi.SubItems.Count; i++)
                mDir += lvi.SubItems[i].Text + ",";

            mDir = mDir.Substring(0, mDir.Length - 1);
            return mDir;
        }
    }
}
