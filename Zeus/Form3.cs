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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = Interview.Survey.NumberOfQuestions;
            progressBar1.Show();
            LoadQuestion(1);
        }

        private Panel _p;

        private void LoadQuestion(int index)
        {
            progressBar1.Value = index;
            lblHeader.Text = String.Format("Pergunta {0} de {1}", index, Interview.Survey.NumberOfQuestions);
            
            var question = Interview.Survey.Questions.Rows[index - 1]["Question"].ToString();
            var type = Convert.ToInt16(Interview.Survey.Questions.Rows[index - 1]["Type"]);
            var numAnswers = Interview.Survey.Questions.Rows[index - 1]["NumAnswers"];

            lblQuestion.Text = question;

            switch (type)
            {
                case 1:
                    lblInstructions.Text = "Digite uma resposta:";
                    TextBox txt = new TextBox();
                    txt.Top = lblInstructions.Bottom + 10;
                    txt.Left = 3;
                    txt.Width = 200;
                    this.Controls.Add(txt);
                    _p = new Panel();
                    _p.Top = txt.Bottom + 10;
                    _p.BackColor = Color.Red;
                    RadioButton r1 = new RadioButton();
                    r1.Text = "Legalize já, uma erva natural não pode te prejudicar";
                    r1.Top = 30;
                    RadioButton r2 = new RadioButton();
                    r2.Text = "Legalize ganja";
                    r2.Top = 90;
                    RadioButton r3 = new RadioButton();
                    r3.Text = "Legalize canabis";
                    r3.Top = 150;

                    _p.Width = this.Width - 15;
                    _p.Controls.Add(r1);
                    _p.Controls.Add(r2);
                    _p.Controls.Add(r3);
                    _p.Height = r3.Bottom + 10;
                    
                    this.Controls.Add(_p);

                    break;

                case 2:
                    lblInstructions.Text = "Selecione N opções:";
                    break;

                case 3:
                    lblInstructions.Text = "Selecione uma opção:";
                    break;

                case 4:
                    lblInstructions.Text = "Selecione uma opção:";
                    break;
            }
            

            
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            _p.Dispose();
        }
    }
}