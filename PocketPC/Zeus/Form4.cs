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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            label2.Text = "(Digite um valor entre 1 e " + (Interview.Answer.LastSaved + 2) + ")";
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            Form3.GoTo = -1;
            this.Close();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            int input = 0;
            try
            {
                input = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception) { }
            if (input < 1 || input > Interview.Answer.LastSaved + 2)
            {
                MessageBox.Show("Digite um valor dentro do intervalo informado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }
            Form3.GoTo = input - 1;
            this.Close();
        }
    }
}