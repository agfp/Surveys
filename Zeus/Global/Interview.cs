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

        public static void NewInterview(string interviewer)
        {
            var folder = "\\Zeus";
            var filename = GetSafeFileName(Survey.Name + "-Respostas-" + DateTime.Now.ToString("yyyy-MM-dd") + ".sdf");
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
            _currentAnswers = new AnswersAccess(_interviewDbPath, interviewer);

        }

        private static string GetSafeFileName(string filename)
        {
            char[] forbidden = { ' ', '<', '>', ':', '\"', '/', '\\', '|', '?', '*' };
            string safeName = filename;
            foreach (char c in forbidden)
            {
                safeName = safeName.Replace(c, '_');
            }
            return safeName;
        }
    }
}
