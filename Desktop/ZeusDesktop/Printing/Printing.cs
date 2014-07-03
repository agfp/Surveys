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
            fd.FontSize = 11;

            Style style = new Style(typeof(Paragraph));
            style.Setters.Add(new Setter(Block.MarginProperty, new Thickness(2)));
            fd.Resources.Add(typeof(Paragraph), style);

            Style style2 = new Style(typeof(TableRow));
            style2.Setters.Add(new Setter(Block.MarginProperty, new Thickness(20)));
            fd.Resources.Add(typeof(TableRow), style2);

            Paragraph title = new Paragraph() { FontWeight = FontWeights.Bold, TextAlignment = System.Windows.TextAlignment.Center, FontSize = 16 };
            fd.Blocks.Add(title);
            title.Inlines.Add(survey);

            Paragraph space = new Paragraph();
            fd.Blocks.Add(space);
            space.Inlines.Add(" ");

            Table table = new Table();
            fd.Blocks.Add(table);
            //table.Columns.Add(new TableColumn() { Width = new GridLength(25) });
            table.Columns.Add(new TableColumn());

            TableRowGroup rowgroup = new TableRowGroup();
            table.RowGroups.Add(rowgroup);

            for (int j = 0; j < 2; j++)
            for (int i = 0; i < questions.Count; i++)
            {
                TableRow row = new TableRow();

                //TableCell cell1 = new TableCell();
                //row.Cells.Add(cell1);

                //Paragraph number = new Paragraph() { FontWeight = FontWeights.Bold };
                //cell1.Blocks.Add(number);
                //number.Inlines.Add((i + 1) + ".");

                var cell2 = NewMethod(questions[i], i+1);
                row.Cells.Add(cell2);

                rowgroup.Rows.Add(row);

                TableRow r = new TableRow();
                rowgroup.Rows.Add(r);

                TableCell c = new TableCell() { ColumnSpan = 1 };
                r.Cells.Add(c);
                c.Blocks.Add(space);
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

        private static TableCell NewMethod(Questions question, int i)
        {
            TableCell cell2 = new TableCell();

            Paragraph q1 = new Paragraph();
            Paragraph q2 = new Paragraph();

            cell2.Blocks.Add(q1);
            cell2.Blocks.Add(q2);

            q1.Inlines.Add(new Bold(new Run(i + ". " + question.Question)));
            

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
                //Paragraph q3 = new Paragraph();
                //cell2.Blocks.Add(q3);
                q2.Inlines.Add(" (");
                q2.Inlines.Add(new Italic(new Run(question.Instruction)));
                q2.Inlines.Add(")");
            }

            switch (question.Type)
            {
                case 1:
                    BlockUIContainer b1 = new BlockUIContainer();
                    cell2.Blocks.Add(b1);
                    b1.Child = new TextBox() { Width = 200, HorizontalAlignment = HorizontalAlignment.Left };
                    break;

                case 2:
                    foreach (var option in question.Options)
                    {
                        BlockUIContainer b2 = new BlockUIContainer();
                        cell2.Blocks.Add(b2);
                        b2.Child = new CheckBox() { Content = option.Option };
                    }
                    break;

                case 3:
                    foreach (var option in question.Options)
                    {
                        BlockUIContainer b2 = new BlockUIContainer();
                        cell2.Blocks.Add(b2);
                        b2.Child = new RadioButton() { Content = option.Option };
                    }
                    break;

                case 4:
                    foreach (var option in question.Options)
                    {
                        BlockUIContainer b2 = new BlockUIContainer();
                        cell2.Blocks.Add(b2);
                        b2.Child = new RadioButton() { Content = option.Option };
                    }

                    BlockUIContainer b3 = new BlockUIContainer();
                    cell2.Blocks.Add(b3);
                    StackPanel s = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal };
                    var r = new RadioButton() { Content = "Outro. Especificar:  " };
                    var t = new TextBox() { Width = 200, HorizontalAlignment = HorizontalAlignment.Left };
                    s.Children.Add(r);
                    s.Children.Add(t);
                    b3.Child = s;
                    break;

            }
            return cell2;
        }
    }
}
