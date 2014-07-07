using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Zeus.DBAccess;
using System.IO;

namespace Zeus.Global
{
    static class Interview
    {
        private static SurveyAccess _currentSurvey;
        private static AnswersAccess _currentAnswers;
        private static string _surveyDbPath;
        private static string _interviewDbPath;

        public static void OpenSurvey(string surveyDbPath)
        {
            _surveyDbPath = surveyDbPath;
            _currentSurvey = new SurveyAccess(surveyDbPath);
        }

        public static SurveyAccess Survey
        {
            get { return _currentSurvey; }
        }

        public static AnswersAccess Answer
        {
            get
            {
                return _currentAnswers;
            }
        }

        public static void NewInterview(int interviewerId)
        {
            var folder = Zeus.Properties.Resources.LocalInterviewFolder;
            var basename = Path.GetFileNameWithoutExtension(_surveyDbPath);
            var filename = Utils.GetSafeFileName(basename + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".zeus");
            var fullname = Path.Combine(folder, filename);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!File.Exists(fullname))
            {
                File.Copy(_surveyDbPath, fullname);
            }
            _interviewDbPath = fullname;
            _currentAnswers = new AnswersAccess(_interviewDbPath, interviewerId);
        }

        public static void End()
        {
            _currentAnswers.Close();
            var basename = Path.GetFileNameWithoutExtension(_surveyDbPath);
            Export.ToText(basename);
        }
    }
}
