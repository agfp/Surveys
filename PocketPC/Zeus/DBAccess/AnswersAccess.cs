using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlServerCe;
using System.Data;
using Zeus.Global;

namespace Zeus.DBAccess
{
    class AnswersAccess
    {
        public int LastSaved;
        private string _connectionString;
        private string _interviewId;

        public AnswersAccess(string dbpath, int interviewerId)
        {
            _connectionString = "Data Source = " + dbpath;
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();

                var cmd1 = conn.CreateCommand();
                cmd1.CommandText = "INSERT INTO Interview(Interviewer_Id, StartTime) VALUES (@InterviewerId, @StartTime)";
                cmd1.Parameters.Add(new SqlCeParameter("@InterviewerId", interviewerId));
                cmd1.Parameters.Add(new SqlCeParameter("@StartTime", DateTime.Now.ToString("s")));
                cmd1.Prepare();
                cmd1.ExecuteNonQuery();

                var cmd2 = conn.CreateCommand();
                cmd2.CommandText = "SELECT @@IDENTITY";
                cmd2.Prepare();
                _interviewId = cmd2.ExecuteScalar().ToString();
            }
        }

        public void Save(int index, int[] closeEnded, string openEnded)
        {
            var questionId = Convert.ToInt32(Interview.Survey.Questions.Rows[index]["Id"]);

            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();

                var cmd1 = conn.CreateCommand();
                cmd1.CommandText = "DELETE FROM Answers WHERE Interview_Id = @InterviewId AND Question_Id = @QuestionId";
                cmd1.Parameters.Add(new SqlCeParameter("@InterviewId", _interviewId));
                cmd1.Parameters.Add(new SqlCeParameter("@QuestionId", questionId));
                cmd1.Prepare();
                cmd1.ExecuteNonQuery();

                foreach (var answer in closeEnded)
                {
                    var cmd2 = conn.CreateCommand();
                    cmd2.CommandText = "INSERT INTO Answers(Interview_Id, Question_Id, CloseEnded, OpenEnded) VALUES (@InterviewId, @QuestionId, @CloseEnded, @OpenEnded)";
                    cmd2.Parameters.Add(new SqlCeParameter("@InterviewId", _interviewId));
                    cmd2.Parameters.Add(new SqlCeParameter("@QuestionId", questionId));
                    cmd2.Parameters.Add(new SqlCeParameter("@CloseEnded", answer));
                    cmd2.Parameters.Add(new SqlCeParameter("@OpenEnded", openEnded.Replace(';', '_')));
                    cmd2.Prepare();
                    cmd2.ExecuteNonQuery();
                }
            }
            if (LastSaved < index)
            {
                LastSaved = index;
            }
        }

        public void Close()
        {
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Interview SET EndTime = @EndTime WHERE Id = @Id";
                cmd.Parameters.Add(new SqlCeParameter("@Id", _interviewId));
                cmd.Parameters.Add(new SqlCeParameter("@EndTime", DateTime.Now.ToString("s")));
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable Get(int questionId)
        {
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();
                DataTable table = new DataTable();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Answers WHERE Interview_Id = @InterviewId AND Question_Id = @QuestionId";
                cmd.Parameters.Add(new SqlCeParameter("@InterviewId", _interviewId));
                cmd.Parameters.Add(new SqlCeParameter("@QuestionId", questionId));
                cmd.Prepare();

                var da = new SqlCeDataAdapter(cmd);
                da.Fill(table);
                da.Dispose();
                return table;
            }
        }

        public string ConvertToText()
        {
            var text = new StringBuilder();
            var answersDT = GetQuestionsAndAnswersTable();

            var startTime = Convert.ToDateTime(answersDT.Rows[0]["StartTime"]);
            var endTime = Convert.ToDateTime(answersDT.Rows[0]["EndTime"]);
            var duration = Convert.ToInt32(endTime.Subtract(startTime).TotalMinutes);

            text.Append(startTime.ToString("s") + ";" + duration + ";");
            text.Append(answersDT.Rows[0]["Interviewer_Id"] + ";");

            var questions = from o in Interview.Survey.Questions.AsEnumerable()
                            orderby o.Field<int>("Order")
                            select new
                            {
                                Id = o.Field<int>("Id"),
                                Type = o.Field<int>("Type"),
                                NumAnswers = o.Field<int?>("NumAnswers")
                            };

            foreach (var question in questions)
            {
                var answer = from o in answersDT.AsEnumerable()
                             where o.Field<int>("Question_Id") == question.Id
                             select new
                             {
                                 CloseEnded = o.Field<int?>("CloseEnded"),
                                 OpenEnded = o.Field<string>("OpenEnded")
                             };

                switch (question.Type)
                {
                    case 1:
                        text.Append(answer.ElementAt(0).OpenEnded);
                        text.Append(";");
                        break;

                    case 2:
                        for (int i = 0; i < question.NumAnswers; i++)
                        {
                            var x = answer.Where(a => a.CloseEnded == i + 1);
                            if (x.Count() == 1)
                            {
                                text.Append(x.Single().CloseEnded);
                            }
                            text.Append(";");
                        }
                        break;

                    case 3:
                        text.Append(answer.ElementAt(0).CloseEnded);
                        text.Append(";");
                        break;

                    case 4:
                        text.Append(answer.ElementAt(0).CloseEnded);
                        text.Append(";");
                        text.Append(answer.ElementAt(0).OpenEnded);
                        text.Append(";");
                        break;
                }
            }
            text.Remove(text.Length - 1, 1);
            return text.ToString();
        }

        private DataTable GetQuestionsAndAnswersTable()
        {
            var answersDT = new DataTable();
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT [Questions].[Type], [Questions].[Order], [Questions].[NumAnswers], 
                           [Interview].[Interviewer_Id], [Interview].[StartTime], [Interview].[EndTime],
                           [Answers].[OpenEnded], [Answers].[CloseEnded], [Answers].[Question_Id]
                    FROM [Questions]  
                      INNER JOIN [Answers]
                        ON [Answers].[Question_Id] = [Questions].[Id]
                      INNER JOIN [Interview]
                        ON [Answers].[Interview_Id] = [Interview].[Id]
                    WHERE [Interview_Id] = @InterviewId";

                cmd.Parameters.Add(new SqlCeParameter("@InterviewId", _interviewId));
                cmd.Prepare();
                var da = new SqlCeDataAdapter(cmd);
                da.Fill(answersDT);
                da.Dispose();
            }
            return answersDT;
        }
    }
}
