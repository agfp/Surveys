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

            lblNomeQuestionario.Text = Database.QuestionarioDT[0]["Nome"].ToString();
            cbbPesquisadores.DataSource = Database.PesquisadoresDT;
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