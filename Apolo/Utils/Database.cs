using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;

namespace Apolo
{
    class Database
    {
        public static DataSet1.QuestionarioDataTable QuestionarioDT;
        public static DataSet1.PesquisadoresDataTable PesquisadoresDT;

        public static void Open()
        {
            QuestionarioDT = new DataSet1.QuestionarioDataTable();
            DataSet1TableAdapters.QuestionarioTableAdapter qta = new DataSet1TableAdapters.QuestionarioTableAdapter();
            qta.Fill(QuestionarioDT);

            PesquisadoresDT = new DataSet1.PesquisadoresDataTable();
            DataSet1TableAdapters.PesquisadoresTableAdapter pta = new DataSet1TableAdapters.PesquisadoresTableAdapter();
            pta.Fill(PesquisadoresDT);
        }
    }
}
