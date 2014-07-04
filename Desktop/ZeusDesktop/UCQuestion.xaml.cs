using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ZeusDesktop
{
    public partial class UCQuestion : UserControl
    {
        public event EventHandler<QuestionEventArgs> SaveQuestion;

        private ObservableCollection<Options> _options = new ObservableCollection<Options>();

        public UCQuestion()
        {
            InitializeComponent();
            lvOptions.ItemsSource = _options;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvOptions.ItemsSource);
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Order", System.ComponentModel.ListSortDirection.Ascending));
            Storyboard sb4 = this.FindResource("Storyboard4") as Storyboard;
            sb4.Begin();
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
                if (question.Type == 2 || question.Type == 3)
                {
                    cbbType.SelectedIndex = 1;
                    txtNumAnswers.Text = question.NumAnswers.ToString();
                }
                else if (question.Type == 4)
                {
                    cbbType.SelectedIndex = 2;
                }
            }
            _options = new ObservableCollection<Options>(question.Options);
            lvOptions.ItemsSource = _options;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvOptions.ItemsSource);
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Order", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void cbbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem)cbbType.SelectedValue).Content.ToString())
            {
                case "Aberta":
                    Storyboard sb1 = this.FindResource("Storyboard1") as Storyboard;
                    sb1.Begin();
                    break;

                case "Fechada":
                    Storyboard sb2 = this.FindResource("Storyboard2") as Storyboard;
                    sb2.Begin();
                    break;

                case "Mista":
                    Storyboard sb3 = this.FindResource("Storyboard3") as Storyboard;
                    sb3.Begin();
                    break;
            }
            txtQuestion.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((Grid)this.Parent).Children.Remove(this);
        }

        private void txtOption_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!String.IsNullOrEmpty(txtOption.Text))
                {
                    _options.Add(new Options() { Order = lvOptions.Items.Count + 1, Option = txtOption.Text });
                    txtOption.Text = String.Empty;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                Questions q = new Questions()
                {
                    Question = txtQuestion.Text,
                    Instruction = txtInstruction.Text,
                    NumAnswers = 1,
                };

                if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() != "Aberta")
                {
                    q.Options.AddRange(_options);
                }

                if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Aberta")
                {
                    q.Type = 1;
                }
                else if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Fechada" && Convert.ToInt32(txtNumAnswers.Text) > 1)
                {
                    q.Type = 2;
                    q.NumAnswers = Convert.ToInt32(txtNumAnswers.Text);
                }
                else if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Fechada")
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
            if (String.IsNullOrEmpty(txtQuestion.Text))
            {
                MessageBox.Show("Preencha o campo pergunta", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() != "Aberta")
            {
                if (_options.Count < 2)
                {
                    MessageBox.Show("Inclua pelo menos duas opções", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            if (((ComboBoxItem)cbbType.SelectedValue).Content.ToString() == "Fechada")
            {
                int numAnswers;
                if (!Int32.TryParse(txtNumAnswers.Text, out numAnswers))
                {
                    MessageBox.Show("Digite o número de opções que o usuário pode escolher", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
                if (numAnswers < 1 || numAnswers > _options.Count)
                {
                    MessageBox.Show("Número de respostas deve estar entre 1 e " + _options.Count, "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            return true;
        }

        private void lvOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (lvOptions.SelectedIndex >= 0)
                {
                    _options.Remove((Options)lvOptions.SelectedItem);

                    for (int i = 0; i < lvOptions.Items.Count; i++)
                    {
                        ((Options)lvOptions.Items[i]).Order = i + i;
                    }
                }
            }
        }
    }

    public class QuestionEventArgs : EventArgs
    {
        public Questions Question { get; set; }
    }
}
