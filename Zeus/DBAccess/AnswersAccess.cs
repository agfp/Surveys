﻿using System;
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
                var cmd1 = "INSERT INTO Interview(Interviewer_Id, StartTime) VALUES ({0}, '{1}')";
                var cmd2 = "SELECT @@IDENTITY";
                cmd1 = String.Format(cmd1, interviewerId, DateTime.Now.ToString("s"));

                var cmd = new SqlCeCommand(cmd1, conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand(cmd2, conn);
                _interviewId = cmd.ExecuteScalar().ToString();
            }
        }

        public void Save(int index, int[] closeEnded, string openEnded)
        {
            var questionId = Convert.ToInt32(Interview.Survey.Questions.Rows[index]["Id"]);

            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();

                var cmd1 = new SqlCeCommand(
                    String.Format("DELETE FROM Answers WHERE Interview_Id = {0} AND Question_Id = {1}",
                        _interviewId, questionId),
                    conn);

                cmd1.ExecuteNonQuery();

                foreach (var answer in closeEnded)
                {
                    var cmd2 = new SqlCeCommand(
                        String.Format("INSERT INTO Answers(Interview_Id, Question_Id, CloseEnded, OpenEnded) VALUES ('{0}', '{1}', '{2}', '{3}')",
                            _interviewId, questionId, answer, openEnded),
                        conn);

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

                var cmd = new SqlCeCommand(
                    String.Format("UPDATE Interview SET EndTime = '{0}' WHERE Id = {1}",
                        DateTime.Now.ToString("s"), _interviewId),
                    conn);

                cmd.ExecuteNonQuery();
            }
        }

        public DataTable Get(int questionId)
        {
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();
                DataTable table = new DataTable();

                var cmd = String.Format("SELECT * FROM Answers WHERE Interview_Id = {0} AND Question_Id = {1}",
                        _interviewId, questionId);

                var da = new SqlCeDataAdapter(cmd, conn);
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
            text.Append(answersDT.Rows[0]["Interviewer_Id"] + ";" + duration + ";");

            var questions = (from o in answersDT.AsEnumerable()
                             orderby o.Field<Int16>("Order")
                             select new
                             {
                                 Id = o.Field<int>("Question_Id"),
                                 Type = o.Field<byte>("Type"),
                                 NumAnswers = o.Field<byte?>("NumAnswers")
                             }
                            ).Distinct();

            foreach (var question in questions)
            {
                var answer = from o in answersDT.AsEnumerable()
                             where o.Field<int>("Question_Id") == question.Id
                             select new
                             {
                                 CloseEnded = o.Field<byte?>("CloseEnded"),
                                 OpenEnded = o.Field<string>("OpenEnded")
                             };

                switch (question.Type)
                {
                    case 1:
                        text.Append(answer.ElementAt(0).OpenEnded);
                        break;

                    case 2:
                        var unfilled = question.NumAnswers - answer.Count();
                        foreach (var option in answer)
                        {
                            text.Append(option.CloseEnded);
                            text.Append(";");
                        }
                        for (int i = 0; i < unfilled; i++)
                        {
                            text.Append(-1);
                            text.Append(";");
                        }
                        text.Remove(text.Length - 1, 1);
                        break;

                    case 3:
                        text.Append(answer.ElementAt(0).CloseEnded);
                        break;

                    case 4:
                        text.Append(answer.ElementAt(0).CloseEnded);
                        text.Append(";");
                        text.Append(answer.ElementAt(0).OpenEnded);
                        break;
                }
                text.Append(";");
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
                var query = @"
                    SELECT [Questions].[Type], [Questions].[Order], [Questions].[NumAnswers], 
                           [Interview].[Interviewer_Id], [Interview].[StartTime], [Interview].[EndTime],
                           [Answers].[OpenEnded], [Answers].[CloseEnded], [Answers].[Question_Id]
                    FROM [Questions]  
                      INNER JOIN [Answers]
                        ON [Answers].[Question_Id] = [Questions].[Id]
                      INNER JOIN [Interview]
                        ON [Answers].[Interview_Id] = [Interview].[Id]
                    WHERE [Interview_Id] = {0}";

                var cmd = String.Format(query, _interviewId);

                var da = new SqlCeDataAdapter(cmd, conn);
                da.Fill(answersDT);
                da.Dispose();
            }
            return answersDT;
        }
    }
}
