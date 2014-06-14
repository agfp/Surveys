using System;
using System.Collections.Generic;
using System.Text;

namespace Apolo
{
    static class CurrentSurvey
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
    }
}
