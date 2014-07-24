using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Text;

namespace ZeusDesktop
{
    public partial class UCQuestion : UserControl
    {
        public event EventHandler<QuestionEventArgs> SaveQuestion;

        public UCQuestion()
        {
            InitializeComponent();
            Storyboard sb0 = this.FindResource("Storyboard0") as Storyboard;
            sb0.Begin();
        }

        public UCQuestion(Questions question)
        {
            InitializeComponent();

            txtQuestion.Text = question.Question;
            txtInstruction.Text = question.Instruction;

            if (question.Type == 1)
            {
                cbbType.SelectedIndex = 0;
            }
            else
            {
                if (question.Type == 2)
                {
                    cbbType.SelectedIndex = 2;
                }
                else if (question.Type == 3)
                {
                    cbbType.SelectedIndex = 1;
                }
                else if (question.Type == 4)
                {
                    cbbType.SelectedIndex = 3;
                }
                var options = new StringBuilder();
                foreach (var o in question.Options)
                {
                    options.AppendLine(o.Option);
                }
                txtOptions.Text = options.ToString();
            }
        }

        private void cbbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem)cbbType.SelectedValue).Content.ToString())
            {
                case "Aberta":
                    Storyboard sb1 = this.FindResource("Storyboard1") as Storyboard;
                    sb1.Begin();
                    break;

                case "Fechada - Resposta única":
                    Storyboard sb2 = this.FindResource("Storyboard2") as Storyboard;
                    sb2.Begin();
                    break;

                case "Fechada - Respostas múltiplas":
                    Storyboard sb3 = this.FindResource("Storyboard3") as Storyboard;
                    sb3.Begin();
                    break;

                case "Mista":
                    Storyboard sb4 = this.FindResource("Storyboard4") as Storyboard;
                    sb4.Begin();
                    break;
            }
            txtQuestion.Focus();
            txtQuestion.CaretIndex = txtQuestion.Text.Length;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((Grid)this.Parent).Children.Remove(this);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                Questions q = new Questions()
                {
                    Question = txtQuestion.Text.Trim(),
                    Instruction = txtInstruction.Text.Trim(),
                    NumAnswers = 1
                };

                if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() != "Aberta")
                {
                    string[] delimiters = { Environment.NewLine };
                    string[] options = txtOptions.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var option in options)
                    {
                        if (!String.IsNullOrWhiteSpace(option))
                        {
                            q.Options.Add(new Options()
                            {
                                Order = q.Options.Count,
                                Option = option.Trim()
                            });
                        }
                    }
                }

                if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Aberta")
                {
                    q.Type = 1;
                }
                else if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Fechada - Respostas múltiplas")
                {
                    q.Type = 2;
                    q.NumAnswers = q.Options.Count;
                }
                else if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Fechada - Resposta única")
                {
                    q.Type = 3;
                }
                else
                {
                    q.Type = 4;
                }

                SaveQuestion(this, new QuestionEventArgs() { Question = q });
                ((Grid)this.Parent).Children.Remove(this);
            }
        }

        private bool Validate()
        {
            var optionsList = new List<Options>();
            string[] delimiters = { Environment.NewLine };
            string[] options = txtOptions.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            foreach (var option in options)
            {
                if (!String.IsNullOrWhiteSpace(option))
                {
                    optionsList.Add(new Options()
                    {
                        Order = optionsList.Count,
                        Option = option
                    });
                }
            }

            if (String.IsNullOrEmpty(txtQuestion.Text))
            {
                MessageBox.Show("Preencha o campo pergunta", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() != "Aberta")
            {
                if (optionsList.Count < 2)
                {
                    MessageBox.Show("Inclua pelo menos duas opções", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            return true;
        }
    }

    public class QuestionEventArgs : EventArgs
    {
        public Questions Question { get; set; }
    }
}
