using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace Zeus.DBAccess
{
    class Survey
    {
        private SqlCeConnection _conn;
        private string _name;
        private DataTable _interviewers;

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
                    string cmd = "SELECT * FROM [Interviewers] ORDER BY NAME";
                    SqlCeDataAdapter da = new SqlCeDataAdapter(cmd, _conn);
                    _interviewers = new DataTable();
                    da.Fill(_interviewers);
                }
                return _interviewers;
            }
        }

        public Survey(string dbpath)
        {
            SqlCeEngine engine = new SqlCeEngine("Data Source = " + dbpath);
            engine.Upgrade();
            _conn = new SqlCeConnection("Data Source = " + dbpath);
            _conn.Open();

            string cmd = "SELECT * FROM [SurveyInfo]";
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd, _conn);
            DataTable metadata = new DataTable();
            da.Fill(metadata);
            _name = metadata.Rows[0]["SurveyName"].ToString();
        }

        ~Survey()
        {
            _conn.Close();
        }

    }
}
