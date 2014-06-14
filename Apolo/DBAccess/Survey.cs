using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;

namespace Apolo
{
    class Survey
    {
        private SqlCeConnection _conn;
        private string _name;

        private DataTable _researcher;
        private DataTable _questions;
        private DataTable _options;

        public Survey(string dbpath)
        {
            _conn = new SqlCeConnection("Data Source = " + dbpath);
            _conn.Open();

            string cmd = "SELECT * FROM [Meta2data]";
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
