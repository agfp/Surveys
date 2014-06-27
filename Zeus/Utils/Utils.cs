using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Zeus
{
    static class Utils
    {
        public static string GetSafeFileName(string filename)
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
