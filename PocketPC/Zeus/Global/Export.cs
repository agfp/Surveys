using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zeus.Global
{
    static class Export
    {
        public static void ToText(string basename)
        {
            var localFolder = Zeus.Properties.Resources.LocalInterviewFolder;
            var text = Interview.Answer.ConvertToText();
            var storageCard = Zeus.Properties.Resources.StorageCard;

            ExportTo(localFolder, basename, text);
            
            if (Directory.Exists(storageCard))
            {
                var sdCard = Zeus.Properties.Resources.CardInterviewFolder;
                ExportTo(sdCard, basename, text);
            }
        }

        private static void ExportTo(string folder, string basename, string text)
        {
            var filename = Utils.GetSafeFileName(basename + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            var fullname = Path.Combine(folder, filename);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!File.Exists(fullname))
            {
                File.Create(filename).Dispose();
            }

            using (var stream = File.AppendText(fullname))
            {
                stream.WriteLine(text);
            }
        }
    }
}
