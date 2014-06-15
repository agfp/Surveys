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
            lblNomeQuestionario.Text = Interview.Get().Name;
            cbbPesquisadores.Items.Clear();
            foreach (DataRow dr in Interview.Get().Interviewers.Rows)
            {
                cbbPesquisadores.Items.Add(dr["Name"]);
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (cbbPesquisadores.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um pesquisador");
                return;
            }
        }
    }
}