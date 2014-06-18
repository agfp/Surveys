using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlServerCe;

namespace Zeus.DBAccess
{
    class AnswersAccess
    {
        private string _connectionString;
        private string _interviewId;

        public AnswersAccess(string dbpath, string interviewer)
        {
            _connectionString = "Data Source = " + dbpath;
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                conn.Open();
                var cmd1 = "INSERT INTO Interview(Interviewer, StartTime) VALUES ('{0}', '{1}')";
                var cmd2 = "SELECT @@IDENTITY";
                cmd1 = String.Format(cmd1, interviewer, DateTime.Now.ToString("s"));

                var cmd = new SqlCeCommand(cmd1, conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand(cmd2, conn);
                _interviewId = cmd.ExecuteScalar().ToString();
            }
        }
    }
}
