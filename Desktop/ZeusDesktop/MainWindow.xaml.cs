using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPF.JoshSmith.ServiceProviders.UI;
using System.Windows.Documents;

namespace ZeusDesktop
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Interviewers> _interviewers = new ObservableCollection<Interviewers>();
        private ObservableCollection<Questions> _questions = new ObservableCollection<Questions>();
        private ListViewDragDropManager<Questions> _dragMgr;
        private string _filename;

        private enum ViewMode
        {
            Edit,
            Export
        }

        public MainWindow()
        {
            InitializeComponent();
            ConfigureWindow();
        }

        public MainWindow(string filename)
        {
            InitializeComponent();
            ConfigureWindow();
            Open(filename);
        }

        private void ConfigureWindow()
        {
            Title = "ZeusDesktop (v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ") - " + "Novo questionário";

            lvInterviewers.ItemsSource = _interviewers;
            CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(lvInterviewers.ItemsSource);
            view1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Ascending));

            lvQuestions.ItemsSource = _questions;

            _dragMgr = new ListViewDragDropManager<Questions>(lvQuestions);
            _dragMgr.ShowDragAdorner = true;
            _dragMgr.DragAdornerOpacity = 0.5;
        }

        #region Events

        #region Buttons

        private void btnNewQuestion_Click(object sender, RoutedEventArgs e)
        {
            UCQuestion ucQuestion = new UCQuestion();
            Grid.SetRowSpan(ucQuestion, 3);
            grdMain.Children.Add(ucQuestion);
            ucQuestion.SaveQuestion += ucQuestion_AddQuestion;
        }

        private void btnNewInterviewer_Click(object sender, RoutedEventArgs e)
        {
            grdMask1.Visibility = System.Windows.Visibility.Visible;
            grdMask2.Visibility = System.Windows.Visibility.Visible;
            grdMask3.Visibility = System.Windows.Visibility.Visible;
            grdAddInterviewer.Visibility = System.Windows.Visibility.Visible;
            txtInterviewerId.Focus();
            grpInterviewers.Header = "(Adicionar aplicador)";

            borderInterview.BorderThickness = new Thickness(2);
            borderInterview.CornerRadius = new CornerRadius(5);
        }

        private void btnDeleteInterviewer_Click(object sender, RoutedEventArgs e)
        {
            if (lvInterviewers.SelectedItem != null)
            {
                _interviewers.Remove((Interviewers)lvInterviewers.SelectedItem);
            }
        }

        private void btnSaveInterviewer_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (String.IsNullOrEmpty(txtInterviewerId.Text) || String.IsNullOrEmpty(txtInterviewerName.Text))
            {
                MessageBox.Show("Preencha os campos Código e Nome", "Aviso", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            if (!Int32.TryParse(txtInterviewerId.Text, out id))
            {
                MessageBox.Show("O campo código deve conter um número", "Aviso", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            var idExists = (from o in _interviewers
                            where o.Id == id
                            select o).Count() > 0;

            if (idExists)
            {
                MessageBox.Show("Código já existente. Por favor insira outro código", "Aviso", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            _interviewers.Add(new Interviewers() { Id = id, Name = txtInterviewerName.Text });


            HideAddInterviewerGrid();
        }

        private void btnCancelInterviewer_Click(object sender, RoutedEventArgs e)
        {
            HideAddInterviewerGrid();
        }

        private void btnEditQuestion_Click(object sender, RoutedEventArgs e)
        {
            EditQuestion();
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (lvQuestions.SelectedItem != null)
            {
                _questions.Remove((Questions)lvQuestions.SelectedItem);
            }
        }

        #endregion

        #region Menu bar

        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Deseja criar um novo questionário?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _filename = null;
                Title = "ZeusDesktop (v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ") - " + "Novo questionário";
                txtSurveyDescription.Text = String.Empty;
                txtSurveyName.Text = String.Empty;
                txtInterviewerId.Text = String.Empty;
                txtInterviewerName.Text = String.Empty;
                _questions = new ObservableCollection<Questions>();
                _interviewers = new ObservableCollection<Interviewers>();
                lvInterviewers.ItemsSource = _interviewers;
                lvQuestions.ItemsSource = _questions;
                SetViewMode(ViewMode.Edit);
            }
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zeus");
            dlg.DefaultExt = ".zeus";
            dlg.Filter = "Questionários (.zeus)|*.zeus";

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                Open(dlg.FileName);
            }
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (String.IsNullOrEmpty(_filename))
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.InitialDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zeus");
                    dlg.DefaultExt = ".zeus";
                    dlg.Filter = "Questionários (.zeus)|*.zeus";

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {
                        _filename = dlg.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                CopyDatabase(_filename);
                ExportData(_filename);
                Title = "ZeusDesktop (v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ") - " + Path.GetFileName(_filename);
                MessageBox.Show("Questionário salvo com sucesso", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.InitialDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zeus");
                dlg.DefaultExt = ".zeus";
                dlg.Filter = "Questionários (.zeus)|*.zeus";

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    CopyDatabase(dlg.FileName);
                    ExportData(dlg.FileName);
                    _filename = dlg.FileName;
                    Title = "ZeusDesktop (v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ") - " + Path.GetFileName(_filename);
                    MessageBox.Show("Questionário salvo com sucesso", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void menuPrint_Click(object sender, RoutedEventArgs e)
        {
            Printing.Print(txtSurveyName.Text, _questions.ToList());
        }

        private void menuExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zeus");
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Arquivos de texto (.txt)|*.txt";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                using (var connection = new SqlCeConnection("Data Source = " + _filename))
                {
                    DefaultDB db = new DefaultDB(connection);
                    Export.ToText(db, dlg.FileName);
                    MessageBox.Show("Questionário exportado com sucesso", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }

        #endregion

        private void ucQuestion_AddQuestion(object sender, QuestionEventArgs e)
        {
            _questions.Add(e.Question);
        }

        private void ucQuestion_EditQuestion(object sender, QuestionEventArgs e)
        {
            var index = lvQuestions.SelectedIndex;
            _questions.RemoveAt(index);
            _questions.Insert(index, e.Question);
        }

        private void lvQuestions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditQuestion();
        }

        #endregion

        #region UI methods

        private void HideAddInterviewerGrid()
        {
            txtInterviewerId.Text = String.Empty;
            txtInterviewerName.Text = String.Empty;
            grdMask1.Visibility = System.Windows.Visibility.Hidden;
            grdMask2.Visibility = System.Windows.Visibility.Hidden;
            grdMask3.Visibility = System.Windows.Visibility.Hidden;
            grdAddInterviewer.Visibility = System.Windows.Visibility.Hidden;
            grpInterviewers.Header = "Aplicadores";

            borderInterview.BorderThickness = new Thickness(0);
            borderInterview.CornerRadius = new CornerRadius(0);
        }

        private void EditQuestion()
        {
            if (lvQuestions.SelectedItem != null)
            {
                UCQuestion ucQuestion = new UCQuestion((Questions)lvQuestions.SelectedItem);
                Grid.SetRowSpan(ucQuestion, 3);
                grdMain.Children.Add(ucQuestion);
                ucQuestion.SaveQuestion += ucQuestion_EditQuestion;
            }
        }

        private void SetViewMode(ViewMode viewMode, int n = 0)
        {
            bool flag = true;
            switch (viewMode)
            {
                case ViewMode.Export:
                    lblAnswersWarning.Visibility = System.Windows.Visibility.Visible;
                    lblAnswersWarning.Content = "Esse questionário foi aplicado " + n + " vezes";
                    flag = false;
                    break;

                case ViewMode.Edit:
                    lblAnswersWarning.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
            grpInfo.IsEnabled = flag;
            grpInterviewers.IsEnabled = flag;
            grpQuestions.IsEnabled = flag;
            menuSave.IsEnabled = flag;
            menuExport.IsEnabled = !flag;
        }

        #endregion

        #region IO methods

        private bool Validate()
        {
            if (String.IsNullOrEmpty(txtSurveyName.Text))
            {
                MessageBox.Show("Digite um nome para o questionário", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (_interviewers.Count == 0)
            {
                MessageBox.Show("Por favor cadastre os aplicadores do questionário", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (_questions.Count == 0)
            {
                MessageBox.Show("Por favor cadastre as perguntas do questionário", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void Open(string filename)
        {
            try
            {
                ImportData(filename);
                _filename = filename;
                Title = "ZeusDesktop (v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ") - " + Path.GetFileName(filename);
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível abrir o arquivo especificado", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CopyDatabase(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("ZeusDesktop.DefaultDB.sdf");

            using (var fileStream = File.Create(path))
            {
                stream.CopyTo(fileStream);
            }
            stream.Dispose();
        }

        private void ExportData(string path)
        {
            using (var connection = new SqlCeConnection("Data Source = " + path))
            {
                DefaultDB db = new DefaultDB(connection);
                SurveyInfo info = new SurveyInfo()
                {
                    SurveyName = txtSurveyName.Text,
                    Description = txtSurveyDescription.Text
                };
                db.SurveyInfo.InsertOnSubmit(info);
                foreach (var interviewer in _interviewers)
                {
                    db.Interviewers.InsertOnSubmit(interviewer);
                }
                for (int i = 0; i < lvQuestions.Items.Count; i++)
                {
                    var question = lvQuestions.Items[i] as Questions;

                    question.Order = i;
                    db.Questions.InsertOnSubmit(question);
                }
                db.SubmitChanges();
            }
        }

        private void ImportData(string path)
        {
            using (var connection = new SqlCeConnection("Data Source = " + path))
            {
                DefaultDB db = new DefaultDB(connection);
                txtSurveyName.Text = db.SurveyInfo.First().SurveyName;
                txtSurveyDescription.Text = db.SurveyInfo.First().Description;

                _questions = new ObservableCollection<Questions>();
                foreach (var question in db.Questions)
                {
                    Questions q = new Questions()
                    {
                        Instruction = question.Instruction,
                        NumAnswers = question.NumAnswers,
                        Options = question.Options,
                        Order = question.Order,
                        Question = question.Question,
                        Type = question.Type
                    };
                    foreach (var option in question.Options)
                    {
                        q.Options.Add(new Options()
                        {
                            Option = option.Option,
                            Order = option.Order,
                        });
                    }
                    _questions.Add(q);
                }

                _interviewers = new ObservableCollection<Interviewers>(db.Interviewers);

                lvInterviewers.ItemsSource = _interviewers;
                CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(lvInterviewers.ItemsSource);
                view1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Ascending));

                lvQuestions.ItemsSource = _questions;

                if (db.Interview.Count() > 0)
                {
                    SetViewMode(ViewMode.Export, db.Interview.Count());
                }
                else
                {
                    SetViewMode(ViewMode.Edit);
                }
            }
        }

        #endregion

    }
}
