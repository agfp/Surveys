using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zeus.DBAccess
{
    class Answers
    {
        public Answers(string name)
        {
            string basedir = "\\Respostas";
            string dbname = name + "_" + DateTime.Now.ToString("yyyy-MM-dd");
            Directory.CreateDirectory(basedir);




            //if (File.Exists("Test.sdf"))
            //    File.Delete("Test.sdf");

            //SqlCeEngine engine = new SqlCeEngine("Data Source = Test.sdf");
            //engine.CreateDatabase();

            //conn = new SqlCeConnection("Data Source = Test.sdf");
        }
    }
}
