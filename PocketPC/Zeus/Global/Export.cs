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
            var localFolder = Zeus.Properties.Resources.LocalInterviewFolder;
            ExportTo(localFolder);

            var storageCard = Zeus.Properties.Resources.StorageCard;
            if (Directory.Exists(storageCard))
            {
                var sdCard = Zeus.Properties.Resources.CardInterviewFolder;
                ExportTo(sdCard);
            }
        }

        private static void ExportTo(string folder)
        {
            var filename = Utils.GetSafeFileName(Interview.Survey.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            var fullname = Path.Combine(folder, filename);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!File.Exists(fullname))
            {
                File.Create(filename).Dispose();
            }
            var text = Interview.Answer.ConvertToText();
            using (var stream = File.AppendText(fullname))
            {
                stream.WriteLine(text);
            }
        }
    }
}
