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
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZeusDesktop
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Interviewers> _interviewers = new ObservableCollection<Interviewers>();
        private ObservableCollection<Questions> _questions = new ObservableCollection<Questions>();
        private string _filename;
        private bool _savePending;

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
            SetWindowTitle();

            _interviewers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_interviewers_CollectionChanged);
            lvInterviewers.ItemsSource = _interviewers;
            CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(lvInterviewers.ItemsSource);
            view1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Ascending));

            _questions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_questions_CollectionChanged);
            lvQuestions.ItemsSource = _questions;
            HideAddInterviewerGrid();
        }

        private void SetSavePending(bool savePending)
        {
            _savePending = savePending;
            SetWindowTitle(_filename);
        }

        private void SetWindowTitle(string filename = null)
        {
            string title = "ZeusDesktop (v.{0}) - {1}{2}";
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string survey, savePending = String.Empty;


            if (String.IsNullOrEmpty(filename))
            {
                survey = "Novo questionário";
            }
            else
            {
                survey = Path.GetFileNameWithoutExtension(filename);
            }

            if (_savePending)
            {
                savePending = "*";
            }

            Title = String.Format(title, version, survey, savePending);
        }

        #region Events

        #region Buttons

        private void btnNewQuestion_Click(object sender, RoutedEventArgs e)
        {
            UCQuestion ucQuestion = new UCQuestion();
            Grid.SetRowSpan(ucQuestion, 4);
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
            grpInterviewers.Header = "Adicionar aplicador";
            grdInterviewer.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            

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

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (lvQuestions.SelectedItems.Count == 0)
            {
                return;
            }

            int first = -1;
            for (int i = 0; i < lvQuestions.Items.Count; i++)
            {
                var item = lvQuestions.Items[i];
                var selected = lvQuestions.SelectedItems;
                if (selected.Contains(item))
                {
                    var index = lvQuestions.Items.IndexOf(item);
                    first = first == -1 ? index : first;
                    if (index == 0)
                    {
                        return;
                    }
                    _questions.RemoveAt(index);
                    _questions.Insert(index - 1, (Questions)item);
                    lvQuestions.SelectedItems.Add(item);
                }
            }
            if (!IsQuestionVisible(first))
            {
                ScrollQuestionsTo(first);
            }
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (lvQuestions.SelectedItems.Count == 0)
            {
                return;
            }

            int last = -1;
            var selectedItems = new List<Questions>();

            for (int i = lvQuestions.Items.Count - 1; i >= 0; i--)
            {
                var item = lvQuestions.Items[i];
                var selected = lvQuestions.SelectedItems;
                if (selected.Contains(item))
                {
                    var index = lvQuestions.Items.IndexOf(item);
                    last = last == -1 ? index : last;
                    if (index == lvQuestions.Items.Count - 1)
                    {
                        return;
                    }
                    _questions.Insert(index + 2, (Questions)item);
                    _questions.RemoveAt(index);
                    selectedItems.Add((Questions)item);
                }
            }
            foreach (var item in selectedItems)
            {
                lvQuestions.SelectedItems.Add(item);
            }
            if (!IsQuestionVisible(last))
            {
                ScrollQuestionsTo(last);
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
                SetWindowTitle();
                txtSurveyDescription.Text = String.Empty;
                txtSurveyName.Text = String.Empty;
                txtInterviewerId.Text = String.Empty;
                txtInterviewerName.Text = String.Empty;
                _questions = new ObservableCollection<Questions>();
                _questions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_questions_CollectionChanged);
                _interviewers = new ObservableCollection<Interviewers>();
                _interviewers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_interviewers_CollectionChanged);
                lvInterviewers.ItemsSource = _interviewers;
                lvQuestions.ItemsSource = _questions;
                SetViewMode(ViewMode.Edit);
                SetSavePending(false);
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
            if (Validate() && _savePending)
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
                SetWindowTitle(_filename);
                //MessageBox.Show("Questionário salvo com sucesso", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
                SetSavePending(false);
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
                    SetWindowTitle(dlg.FileName);
                    //MessageBox.Show("Questionário salvo com sucesso", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
                    Open(dlg.FileName);
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
                    //MessageBox.Show("Questionário exportado com sucesso", "Mensagem", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }

        #endregion

        #region Save Pending Events

        private void lvInterviewers_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            SetSavePending(true);
        }

        private void lvQuestions_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            SetSavePending(true);
        }

        private void txtSurveyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetSavePending(true);
        }

        private void txtSurveyDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetSavePending(true);
        }

        #endregion

        private void lvQuestions_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var grid = lvQuestions.View as GridView;
            grid.Columns[0].Width = 70;
            grid.Columns[1].Width = lvQuestions.ActualWidth - 100;
        }

        private void lvInterviewers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var grid = lvInterviewers.View as GridView;
            grid.Columns[0].Width = 50;
            grid.Columns[1].Width = lvInterviewers.ActualWidth - 80;
        }

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_savePending)
            {
                var result = MessageBox.Show("Deseja salvar as alterações?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    menuSave_Click(null, null);
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region UI methods

        private void HideAddInterviewerGrid()
        {
            txtInterviewerId.Text = String.Empty;
            txtInterviewerName.Text = String.Empty;
            grdInterviewer.Background = new SolidColorBrush(Colors.Transparent);
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
                Grid.SetRowSpan(ucQuestion, 4);
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

        public static T GetVisualChild<T>(DependencyObject referenceVisual) where T : Visual
        {
            Visual child = null;
            int childCount = VisualTreeHelper.GetChildrenCount(referenceVisual);

            for (int i = 0; i < childCount; i++)
            {
                child = VisualTreeHelper.GetChild(referenceVisual, i) as Visual;

                if (child != null && child is T)
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetVisualChild<T>(child);
                    if (child != null && child is T) break;
                }
            }
            return child as T;
        }

        private bool IsQuestionVisible(int index)
        {
            ScrollViewer scrollViewer = GetVisualChild<ScrollViewer>(lvQuestions);
            if (scrollViewer != null)
            {
                ScrollBar scrollBar = scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer) as ScrollBar;
                if (scrollBar != null)
                {
                    var first = Math.Ceiling(scrollViewer.VerticalOffset);
                    var last = Math.Floor(scrollViewer.VerticalOffset + scrollViewer.ViewportHeight);

                    if (index > first && index < last)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ScrollQuestionsTo(int index)
        {
            VirtualizingStackPanel vsp =
              (VirtualizingStackPanel)typeof(ItemsControl).InvokeMember("_itemsHost",
               BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic, null,
               lvQuestions, null);

            double scrollHeight = vsp.ScrollOwner.ScrollableHeight;
            double offset = scrollHeight * index / lvQuestions.Items.Count;

            vsp.SetVerticalOffset(offset);
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
                SetSavePending(false);
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
                _questions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_questions_CollectionChanged);
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
                _interviewers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_interviewers_CollectionChanged);
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

        void _questions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetSavePending(true);
            grpQuestions.Header = String.Format("Perguntas ({0})", _questions.Count);
        }

        void _interviewers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetSavePending(true);
        }

        #endregion

    }
}
