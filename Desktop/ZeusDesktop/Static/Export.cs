using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZeusDesktop
{
    static class Export
    {
        public static void ToText(DefaultDB db, string outputPath)
        {
            var fileContent = GetFileContent(db);

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            using (var stream = File.CreateText(outputPath))
            {
                stream.Write(fileContent);
            }
        }

        private static string GetFileContent(DefaultDB db)
        {
            var fileContent = new StringBuilder();

            foreach (var interview in db.Interview)
            {
                var text = new StringBuilder();

                text.Append(interview.StartTime.ToString("s") + ";");
                if (interview.EndTime.HasValue)
                {
                    var duration = Convert.ToInt32(interview.EndTime.Value.Subtract(interview.StartTime).TotalMinutes);
                    text.Append(duration + ";");
                }
                else
                {
                    text.Append(";");
                }
                text.Append(interview.Interviewer_Id + ";");

                var questions = from o in db.Questions
                                orderby o.Order
                                select o;

                foreach (var question in questions)
                {
                    var answers = from a in question.Answers
                                  where a.Interview_Id == interview.Id
                                  select a;

                    switch (question.Type)
                    {
                        case 1:
                            if (answers.Count() == 1)
                            {
                                text.Append(answers.ElementAt(0).OpenEnded);
                            }
                            text.Append(";");
                            break;

                        case 2:
                            for (int i = 0; i < question.NumAnswers; i++)
                            {
                                var x = answers.Where(a => a.CloseEnded == i + 1);
                                if (x.Count() == 1)
                                {
                                    text.Append(x.Single().CloseEnded);
                                }
                                text.Append(";");
                            }
                            break;

                        case 3:
                            if (answers.Count() == 1)
                            {
                                text.Append(answers.ElementAt(0).CloseEnded);
                            }
                            text.Append(";");
                            break;

                        case 4:
                            if (answers.Count() == 1)
                            {
                                text.Append(answers.ElementAt(0).CloseEnded + ";");
                                text.Append(answers.ElementAt(0).OpenEnded + ";");
                            }
                            else
                            {
                                text.Append(";;");
                            }
                            break;
                    }
                }
                text.Remove(text.Length - 1, 1);
                fileContent.AppendLine(text.ToString());
            }
            return fileContent.ToString();
        }
    }
}
