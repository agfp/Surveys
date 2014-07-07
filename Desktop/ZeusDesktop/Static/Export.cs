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
                    text.Append("-;-;");
                }
                text.Append(interview.Interviewer_Id + ";");

                var questions = (from o in interview.Answers
                                 orderby o.Questions.Order
                                 select o.Questions).Distinct();

                foreach (var question in questions)
                {
                    var answers = from a in question.Answers
                                  where a.Interview_Id == interview.Id
                                  select a;

                    switch (question.Type)
                    {
                        case 1:
                            text.Append(answers.ElementAt(0).OpenEnded + ";");
                            break;

                        case 2:
                            var unfilled = question.NumAnswers - answers.Count();
                            foreach (var option in answers)
                            {
                                text.Append(option.CloseEnded + ";");
                            }
                            for (int i = 0; i < unfilled; i++)
                            {
                                text.Append("-1;");
                            }
                            break;

                        case 3:
                            text.Append(answers.ElementAt(0).CloseEnded + ";");
                            break;

                        case 4:
                            text.Append(answers.ElementAt(0).CloseEnded + ";");
                            text.Append(answers.ElementAt(0).OpenEnded + ";");
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
