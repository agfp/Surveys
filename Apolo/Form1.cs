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

        private void menuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialogEx ofd = new OpenFileDialogEx();
            ofd.Filter = "*.sdf";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CurrentSurvey.Set(ofd.FileName);
                    Form2 frm2 = new Form2();
                    frm2.Owner = this;
                    frm2.Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível abrir o questionário especificado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}