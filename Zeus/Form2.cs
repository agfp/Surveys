using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zeus.Global;

namespace Zeus
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadFields();
        }

        private void LoadFields()
        {
            lblNomeQuestionario.Text = Interview.Survey.Name;
            cbbPesquisadores.Items.Clear();
            foreach (DataRow dr in Interview.Survey.Interviewers.Rows)
            {
                cbbPesquisadores.Items.Add(dr["Name"]);
            }
            inputPanel1.Enabled = false;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (cbbPesquisadores.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um pesquisador");
                return;
            }

            var interviwerId = (from o in Interview.Survey.Interviewers.AsEnumerable()
                                where o.Field<string>("Name") == cbbPesquisadores.SelectedItem.ToString()
                                select o.Field<int>("Id")).First();

            Interview.NewInterview(interviwerId);
            Form3 frm3 = new Form3();
            frm3.Owner = this;
            frm3.Show();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}