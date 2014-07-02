using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace Zeus.DBAccess
{
    class SurveyAccess
    {
        private string _connectionString;
        private string _name;
        private int _numberOfQuestions = -1;
        private DataTable _surveyInfo;
        private DataTable _interviewers;
        private DataTable _questions;
        private DataTable _options;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public DataTable Interviewers
        {
            get
            {
                if (_interviewers == null)
                {
                    using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
                    {
                        string cmd = "SELECT * FROM [Interviewers] ORDER BY [Name]";
                        FillTable(conn, cmd, out _interviewers);
                    }
                }
                return _interviewers;
            }
        }

        public DataTable Questions
        {
            get
            {
                if (_questions == null)
                {
                    using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
                    {
                        string cmd = "SELECT * FROM [Questions] ORDER BY [Order]";
                        FillTable(conn, cmd, out _questions);
                    }
                }
                return _questions;
            }
        }

        public DataTable Options
        {
            get
            {
                if (_options == null)
                {
                    using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
                    {
                        string cmd = "SELECT * FROM [Options] ORDER BY [Order]";
                        FillTable(conn, cmd, out _options);
                    }
                }
                return _options;
            }
        }

        public int NumberOfQuestions
        {
            get
            {
                if (_numberOfQuestions == -1)
                {
                    _numberOfQuestions = Questions.Rows.Count;
                }
                return _numberOfQuestions;
            }
        }

        public SurveyAccess(string dbpath)
        {
            _connectionString = "Data Source = " + dbpath;
            using (SqlCeConnection conn = new SqlCeConnection(_connectionString))
            {
                string cmd = "SELECT * FROM [SurveyInfo]";
                FillTable(conn, cmd, out _surveyInfo);
                _name = _surveyInfo.Rows[0]["SurveyName"].ToString();
            }
        }

        private void FillTable(SqlCeConnection conn, string command, out DataTable table)
        {
            conn.Open();
            table = new DataTable();
            SqlCeDataAdapter da = new SqlCeDataAdapter(command, conn);
            da.Fill(table);
            da.Dispose();
        }
    }
}
