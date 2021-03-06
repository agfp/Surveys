﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zeus.Global;
using MeasureStringSample;

namespace Zeus
{
    public partial class Form3 : Form
    {
        #region Private variables

        private Panel _panel;
        private int _currentIndex;
        private int _currentQuestionId;
        private TextBox _textBox;
        private List<RadioButton> _radioButtonList;
        private List<CheckBox> _checkBoxList;
        private static int _goTo = -1;

        #endregion

        #region Public variables and constructor

        public static int GoTo
        {
            get { return _goTo; }
            set { _goTo = value; }
        }

        public Form3()
        {
            InitializeComponent();
            _currentIndex = 0;
            LoadQuestion();
        }

        #endregion

        #region Events

        private void menuItemGoTo_Click(object sender, EventArgs e)
        {
            if (Interview.Answer.LastSaved == 0)
            {
                MessageBox.Show("Não é possível navegar por um questionário não respondido", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            Form4 frm = new Form4();
            frm.Owner = this;
            frm.Show();
        }

        private void menuItemNextQuestion_Click(object sender, EventArgs e)
        {
            NextQuestion();
        }

        private void menuItemPreviousQuestion_Click(object sender, EventArgs e)
        {
            _panel.Dispose();
            _currentIndex--;
            LoadQuestion();
        }

        private void menuItemAbort_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Se você continuar a entrevista será perdida. Deseja continuar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void _textBox_GotFocus(object sender, EventArgs e)
        {
            inputPanel1.Enabled = true;
            this.AutoScrollPosition = new Point(0, this.Height);
        }

        private void _textBox_LostFocus(object sender, EventArgs e)
        {
            inputPanel1.Enabled = false;
        }

        private void Form3_GotFocus(object sender, EventArgs e)
        {
            if (_goTo >= 0)
            {
                _currentIndex = _goTo;
                _goTo = -1;
                _panel.Dispose();
                LoadQuestion();
            }
        }

        #endregion

        #region Private Methods

        private void LoadQuestion()
        {
            if (_currentIndex == Interview.Survey.Questions.Rows.Count)
            {
                inputPanel1.Enabled = false;
                EndInterview();
                return;
            }

            ReloadForm();
            var type = Convert.ToInt32(Interview.Survey.Questions.Rows[_currentIndex]["Type"]);
            switch (type)
            {
                case 1:
                    LoadType1Panel();
                    break;

                case 2:
                    LoadType2Panel();
                    break;

                case 3:
                    LoadType3Panel();
                    break;

                case 4:
                    LoadType4Panel();
                    break;
            }
            LoadAnswer();
            this.AutoScrollPosition = new Point(0, 0);
        }

        private void ReloadForm()
        {
            if (_currentIndex == Interview.Survey.Questions.Rows.Count - 1)
            {
                menuItemNextQuestion.Text = "Finalizar";
            }
            else
            {
                menuItemNextQuestion.Text = "Próximo";
            }

            menuItemPreviousQuestion.Enabled = _currentIndex > 0;
            //menuItemGoTo.Enabled = _currentIndex > 0;
            inputPanel1.Enabled = false;
            lblHeader.Text = String.Format("Pergunta {0} de {1}", _currentIndex + 1, Interview.Survey.NumberOfQuestions);

            var question = Interview.Survey.Questions.Rows[_currentIndex]["Question"].ToString();
            _currentQuestionId = Convert.ToInt32(Interview.Survey.Questions.Rows[_currentIndex]["Id"]);

            lblQuestion.Text = question;
            lblQuestion.Width = this.Width - 18;
            lblQuestion.Height = CFMeasureString.MeasureString(lblQuestion, lblQuestion.Text, lblQuestion.ClientRectangle).Height;
            lblInstructions.Top = lblQuestion.Bottom + 10;
            lblDbInstruction.Top = lblInstructions.Top + 20;

            _panel = new Panel();
            _panel.Top = lblDbInstruction.Top;
            _panel.Width = this.Width - 18;
            _panel.Left = 3;

            if (!String.IsNullOrEmpty(Interview.Survey.Questions.Rows[_currentIndex]["Instruction"].ToString()))
            {
                lblDbInstruction.Text = Interview.Survey.Questions.Rows[_currentIndex]["Instruction"].ToString();
                lblDbInstruction.Height = CFMeasureString.MeasureString(lblDbInstruction, lblDbInstruction.Text, lblDbInstruction.ClientRectangle).Height;
                _panel.Top = lblDbInstruction.Bottom + 5;
                lblDbInstruction.Show();
            }
            else
            {
                lblDbInstruction.Hide();
                _panel.Top = lblDbInstruction.Top;
            }
            this.Controls.Add(_panel);
        }

        private void LoadType1Panel()
        {
            lblInstructions.Text = "Digite uma resposta:";
            _textBox = new TextBox();
            _textBox.GotFocus += new EventHandler(_textBox_GotFocus);
            _textBox.LostFocus += new EventHandler(_textBox_LostFocus);
            _textBox.Width = 200;
            _panel.Controls.Add(_textBox);
            _panel.Height = _textBox.Bottom + 90;
        }

        private void LoadType2Panel()
        {
            lblInstructions.Text = "Selecione as opções aplicáveis:";

            var filter = "Question_Id = " + _currentQuestionId;
            var options = Interview.Survey.Options.Select(filter);
            _checkBoxList = new List<CheckBox>();
            foreach (var option in options)
            {
                _checkBoxList.Add(new CheckBox());
            }
            int height = PlaceComponents(options, _checkBoxList);
            _panel.Height = height;
        }

        private void LoadType3Panel()
        {
            lblInstructions.Text = "Selecione uma opção:";

            var height = AddRadioButtons();
            _panel.Height = height;
        }

        private void LoadType4Panel()
        {
            lblInstructions.Text = "Selecione uma opção:";

            var top = AddRadioButtons();
            _radioButtonList.Last().Click += (sender, args) => { _textBox.Focus(); };
            _textBox = new TextBox();
            _textBox.GotFocus += new EventHandler(_textBox_GotFocus);
            _textBox.LostFocus += new EventHandler(_textBox_LostFocus);
            _textBox.Width = 200;
            _textBox.Top = top;
            _panel.Height = _textBox.Bottom + 90;
            _panel.Controls.Add(_textBox);
        }

        private int AddRadioButtons()
        {
            var filter = "Question_Id = " + _currentQuestionId;
            var options = Interview.Survey.Options.Select(filter);
            _radioButtonList = new List<RadioButton>();
            foreach (var option in options)
            {
                _radioButtonList.Add(new RadioButton());
            }
            return PlaceComponents(options, _radioButtonList);
        }

        private int PlaceComponents<T>(DataRow[] options, List<T> list)
        {
            int top = 0;
            for (int i = 0; i < options.Count(); i++)
            {
                Control option = list[i] as Control;
                option.Tag = i + 1;
                var label = new Label();
                label.Text = options[i]["Option"].ToString();
                label.Left = 20;
                label.Width = _panel.Width - label.Left;
                label.Height = CFMeasureString.MeasureString(label, label.Text, label.ClientRectangle).Height;
                option.Width = 20;
                option.Top = top;
                label.Top = top + 2;
                _panel.Controls.Add(option);
                _panel.Controls.Add(label);
                top = label.Bottom + 10;
            }
            return top;
        }

        private void NextQuestion()
        {
            if (ValidanteAndSave())
            {
                _panel.Dispose();
                _currentIndex++;
                LoadQuestion();
            }
        }

        private bool ValidanteAndSave()
        {
            var type = Convert.ToInt32(Interview.Survey.Questions.Rows[_currentIndex]["Type"]);
            string message = String.Empty;
            if (type == 1)
            {
                var answer = _textBox.Text;
                if (String.IsNullOrEmpty(answer))
                {
                    message = "Digite uma resposta";
                }
                else
                {
                    Interview.Answer.Save(_currentIndex, new int[] { 0 }, answer);
                    return true;
                }
            }
            else if (type == 2)
            {
                var selectedAnswers = from c in _checkBoxList
                                      where c.Checked == true
                                      select Convert.ToInt32(c.Tag);

                Interview.Answer.Save(_currentIndex, selectedAnswers.ToArray(), String.Empty);
                return true;
            }
            else
            {
                var openEnded = type == 4 ? _textBox.Text : String.Empty;
                var closeEnded = from r in _radioButtonList
                                 where r.Checked == true
                                 select Convert.ToInt32(r.Tag);

                if (closeEnded.Count() == 0)
                {
                    message = "Selecione uma resposta";
                }
                else if (type == 4 && closeEnded.Single() == _radioButtonList.Count && String.IsNullOrEmpty(openEnded))
                {
                    message = "Digite uma resposta";
                }
                else
                {
                    Interview.Answer.Save(_currentIndex, closeEnded.ToArray(), openEnded);
                    return true;
                }
            }
            MessageBox.Show(message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            return false;
        }

        private void LoadAnswer()
        {
            var answers = Interview.Answer.Get(_currentQuestionId);
            if (answers.Rows.Count > 0)
            {
                var type = Convert.ToInt32(Interview.Survey.Questions.Rows[_currentIndex]["Type"]);
                if (type == 1)
                {
                    _textBox.Text = answers.Rows[0]["OpenEnded"].ToString();
                }
                else if (type == 2)
                {
                    var checkboxes = from a in answers.AsEnumerable()
                                     from c in _checkBoxList
                                     where a.Field<int>("CloseEnded") == Convert.ToInt32(c.Tag)
                                     select c;

                    foreach (var c in checkboxes)
                    {
                        c.Checked = true;
                    }
                }
                else
                {
                    var radiobutton = (from a in answers.AsEnumerable()
                                       from r in _radioButtonList
                                       where a.Field<int>("CloseEnded") == Convert.ToInt32(r.Tag)
                                       select r).Single();

                    radiobutton.Checked = true;
                    if (type == 4 && Convert.ToInt32(radiobutton.Tag) == _radioButtonList.Count)
                    {
                        _textBox.Text = answers.Rows[0]["OpenEnded"].ToString();
                    }
                }
            }
        }

        private void EndInterview()
        {
            Interview.End();
            MessageBox.Show("Entrevista concluída.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            this.Close();
        }

        #endregion

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }

        }
    }
}