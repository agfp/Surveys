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
            _interviewDbPath = _surveyDbPath;
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
