using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MobilePractices.OpenFileDialogEx;

namespace Apolo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database.Open("");
            Form2 frm2 = new Form2();
            frm2.Owner = this;
            frm2.Show();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialogEx ofd = new OpenFileDialogEx();
            ofd.Filter = "*.sdf";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Database.Open(ofd.FileName);
                MessageBox.Show(Database.ds.Tables[0].Rows[0]["Nome"].ToString());


            }
            else
            {
                MessageBox.Show("User canceled the dialog", "Status");
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {

        }

    }
}