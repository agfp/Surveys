using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlServerCe;
using System.Data;

namespace Zeus.DBAccess
{
    class AnswersAccess
    {
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

        public void Save(int questionId, int[] closeEnded, string openEnded)
        {
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

                SqlCeDataAdapter da = new SqlCeDataAdapter(cmd, conn);
                da.Fill(table);
                da.Dispose();

                return table;
            }
        }
    }
}
