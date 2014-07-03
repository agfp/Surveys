using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;

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
            table.Columns.Add(new TableColumn() { Width = new GridLength(25) });
            table.Columns.Add(new TableColumn());

            TableRowGroup rowgroup = new TableRowGroup();
            table.RowGroups.Add(rowgroup);

            for (int i = 0; i < questions.Count; i++)
            {
                TableRow row = new TableRow();

                TableCell cell1 = new TableCell();
                row.Cells.Add(cell1);

                Paragraph number = new Paragraph() { FontWeight = FontWeights.Bold };
                cell1.Blocks.Add(number);
                number.Inlines.Add((i + 1) + ".");

                var cell2 = NewMethod(questions[i]);
                row.Cells.Add(cell2);

                rowgroup.Rows.Add(row);

                TableRow r = new TableRow();
                rowgroup.Rows.Add(r);

                TableCell c = new TableCell() { ColumnSpan = 2 };
                r.Cells.Add(c);
                c.Blocks.Add(space);
            }

            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
            fd.PageHeight = pd.PrintableAreaHeight;
            fd.PageWidth = pd.PrintableAreaWidth;
            fd.PagePadding = new Thickness(50);
            fd.ColumnGap = 0;
            fd.ColumnWidth = pd.PrintableAreaWidth;

            IDocumentPaginatorSource dps = fd;
            pd.PrintDocument(dps.DocumentPaginator, survey);

        }

        private static TableCell NewMethod(Questions question)
        {
            TableCell cell2 = new TableCell();

            Paragraph q1 = new Paragraph();
            Paragraph q2 = new Paragraph();

            cell2.Blocks.Add(q1);
            cell2.Blocks.Add(q2);

            q1.Inlines.Add(new Bold(new Run(question.Question)));
            q2.Inlines.Add("Instrução genérica");

            if (!String.IsNullOrEmpty(question.Instruction))
            {
                Paragraph q3 = new Paragraph();
                cell2.Blocks.Add(q3);
                q3.Inlines.Add(new Italic(new Run(question.Instruction)));
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
