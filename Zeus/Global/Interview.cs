using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Zeus.DBAccess;

namespace Zeus.Global
{
    static class Interview
    {
        private static Survey _currentSurvey;

        public static void Set(string dbpath)
        {
            _currentSurvey = new Survey(dbpath);
        }

        public static Survey Get()
        {
            return _currentSurvey;
        }

        public static void CreateAnswer()
        {

        }
    }
}
