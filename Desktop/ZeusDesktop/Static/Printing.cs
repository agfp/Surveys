using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZeusDesktop
{
    static class Printing
    {
        public static void Print(string survey, List<Questions> questions)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                FlowDocument fd = new FlowDocument();

                fd.FontFamily = new System.Windows.Media.FontFamily("Calibri");
                fd.FontSize = 14;

                Style style = new Style(typeof(Paragraph));
                style.Setters.Add(new Setter(Block.MarginProperty, new Thickness(0, 20, 0, 2)));
                fd.Resources.Add("Question", style);

                Paragraph title = new Paragraph() { FontWeight = FontWeights.Bold, TextAlignment = System.Windows.TextAlignment.Center, FontSize = 22 };
                fd.Blocks.Add(title);
                title.Inlines.Add(survey);
                
                for (int i = 0; i < questions.Count; i++)
                {
                    CreateQuestion(questions[i], i + 1, fd);
                }

                fd.PageHeight = pd.PrintableAreaHeight;
                fd.PageWidth = pd.PrintableAreaWidth;
                fd.PagePadding = new Thickness(50);
                fd.ColumnGap = 20;
                fd.ColumnWidth = pd.PrintableAreaWidth / 3;
                fd.ColumnRuleBrush = new SolidColorBrush(Colors.Black);
                fd.ColumnRuleWidth = 1;

                IDocumentPaginatorSource dps = fd;
                pd.PrintDocument(dps.DocumentPaginator, survey);
            }
        }

        public delegate Control OptionControl();

        private static void CreateQuestion(Questions question, int number, FlowDocument doc)
        {
            Paragraph p = new Paragraph() { KeepTogether = true };
            doc.Blocks.Add(p);
            p.SetResourceReference(Control.StyleProperty, "Question");

            p.Inlines.Add(new Bold(new Run(number + ") " + question.Question + "\n")));

            if (question.Type == 1)
            {
                AddQuestionHeader(p,
                    "Preencha o campo",
                    question.Instruction);

                AddTextbox(p);
            }
            else if (question.Type == 2)
            {
                AddQuestionHeader(p,
                    "Selecione até " + question.NumAnswers + " opções",
                    question.Instruction);

                AddOptions(p, question, () => { return new CheckBox(); });
            }
            else
            {
                AddQuestionHeader(p,
                    "Selecione uma opção",
                    question.Instruction);

                AddOptions(p, question, () => { return new RadioButton(); });

                if (question.Type == 4)
                {
                    p.Inlines.Add("\n");
                    var radiobutton = new InlineUIContainer() { Child = new RadioButton() };
                    p.Inlines.Add(radiobutton);
                    p.Inlines.Add(" Outro. Especificar: ");
                    AddTextbox(p);
                }
            }
        }

        private static void AddQuestionHeader(Paragraph p, string instruction1, string instruction2)
        {
            p.Inlines.Add(instruction1);
            if (!String.IsNullOrEmpty(instruction2))
            {
                p.Inlines.Add(" (");
                p.Inlines.Add(new Italic(new Run(instruction2)));
                p.Inlines.Add(")");
            }
            p.Inlines.Add(":\n");
        }

        private static void AddTextbox(Paragraph p)
        {
            InlineUIContainer textbox = new InlineUIContainer();
            textbox.Child = new TextBox() { Width = 200, HorizontalAlignment = HorizontalAlignment.Left };
            p.Inlines.Add(textbox);
        }

        private static void AddOptions(Paragraph p, Questions question, OptionControl optionControl)
        {
            var options = question.Options.OrderBy(o => o.Order);

            for (int i = 0; i < options.Count(); i++)
            {
                if (i != 0)
                {
                    p.Inlines.Add("\n");
                }
                var control = new InlineUIContainer() { Child = optionControl() };
                p.Inlines.Add(control);
                p.Inlines.Add(" ");
                p.Inlines.Add(options.ElementAt(i).Option);
            }
        }
    }
}
