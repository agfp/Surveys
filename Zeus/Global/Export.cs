using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zeus.Global
{
    static class Export
    {
        public static void ToText()
        {
            //var folder = "\\Zeus\\Respostas";
            //var filename = Utils.GetSafeFileName(Survey.Name + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            //var fullname = Path.Combine(folder, filename);

            //if (!Directory.Exists(folder))
            //{
            //    Directory.CreateDirectory(folder);
            //}
            //if (!File.Exists(fullname))
            //{
            //    File.Create(fullname);
            //}

            var bla = Interview.Answer.ConvertToText();

        }

       
    }
}
