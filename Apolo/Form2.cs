using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Apolo
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
            //lblNomeQuestionario.Text = Database.QuestionarioDT[0]["Nome"].ToString();
            //cbbPesquisadores.Items.Clear();
            //cbbPesquisadores.Items.Add("");
            //foreach (DataRow dr in Database.PesquisadoresDT.Rows)
            //{
            //    cbbPesquisadores.Items.Add(dr["Nome"]);
            //}
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (cbbPesquisadores.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor selecione um pesquisador");
            }
            else
            {

            }
        }
    }
}