using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            FlowDocument fd = new FlowDocument();

            fd.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            fd.FontSize = 12;

            Style style = new Style(typeof(Paragraph));
            style.Setters.Add(new Setter(Block.MarginProperty, new Thickness(0, 2, 0, 2)));
            fd.Resources.Add(typeof(Paragraph), style);

            Style style2 = new Style(typeof(Paragraph));
            style2.Setters.Add(new Setter(Block.MarginProperty, new Thickness(0,0,0,20)));
            fd.Resources.Add("Question", style2);

            Paragraph title = new Paragraph() { FontWeight = FontWeights.Bold, TextAlignment = System.Windows.TextAlignment.Center, FontSize = 18 };
            fd.Blocks.Add(title);
            title.Inlines.Add(survey);


            Paragraph title2 = new Paragraph();
            fd.Blocks.Add(title2);
            title2.Inlines.Add(" ");

            for (int j = 0; j < 2; j++)
                for (int i = 0; i < questions.Count; i++)
                {

                    NewMethod(questions[i], i + 1, fd);
                    fd.Blocks.Add(title2);
                }

            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
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

        private static void NewMethod(Questions question, int i, FlowDocument fd)
        {
            //TableCell cell2 = new TableCell();

            Paragraph q1 = new Paragraph();
            Paragraph q2 = new Paragraph();

            q1.SetResourceReference(Control.StyleProperty, "Question");

            fd.Blocks.Add(q1);
            fd.Blocks.Add(q2);

            q1.Inlines.Add(new Bold(new Run(i + ") " + question.Question)));


            switch (question.Type)
            {
                case 1:
                    q2.Inlines.Add("Preencha o campo");
                    break;

                case 2:
                    q2.Inlines.Add("Selecione até " + question.NumAnswers + " opções");
                    break;

                case 3:
                case 4:
                    q2.Inlines.Add("Selecione uma opção");
                    break;

            }


            if (!String.IsNullOrEmpty(question.Instruction))
            {
                q2.Inlines.Add(" (");
                q2.Inlines.Add(new Italic(new Run(question.Instruction)));
                q2.Inlines.Add(")");
            }

            switch (question.Type)
            {
                case 1:
                    BlockUIContainer b1 = new BlockUIContainer();
                    fd.Blocks.Add(b1);
                    b1.Child = new TextBox() { Width = 200, HorizontalAlignment = HorizontalAlignment.Left };
                    break;

                case 2:
                    foreach (var option in question.Options)
                    {
                        BlockUIContainer b2 = new BlockUIContainer();
                        fd.Blocks.Add(b2);
                        b2.Child = new CheckBox() { Content = option.Option };
                    }
                    break;

                case 3:
                    foreach (var option in question.Options)
                    {
                        BlockUIContainer b2 = new BlockUIContainer();
                        fd.Blocks.Add(b2);
                        b2.Child = new RadioButton() { Content = option.Option };
                    }
                    break;

                case 4:
                    foreach (var option in question.Options)
                    {
                        BlockUIContainer b2 = new BlockUIContainer();
                        fd.Blocks.Add(b2);
                        b2.Child = new RadioButton() { Content = option.Option };
                    }

                    BlockUIContainer b3 = new BlockUIContainer();
                    fd.Blocks.Add(b3);
                    StackPanel s = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal };
                    var r = new RadioButton() { Content = "Outro. Especificar:  " };
                    var t = new TextBox() { Width = 200, HorizontalAlignment = HorizontalAlignment.Left };
                    s.Children.Add(r);
                    s.Children.Add(t);
                    b3.Child = s;
                    break;

            }
        }
    }
}
