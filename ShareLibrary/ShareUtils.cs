using System;
using System.Collections.Generic;
using System.Text;

namespace ShareLibrary
{
    public sealed class ShareUtils
    {
        public static bool IsNullOrEmpty(string sv)
        {
            return string.IsNullOrEmpty(sv) || sv.Equals("null");
        }

        public static DateTime Now()
        {
            return DateTime.Now;
        }

        public static string UUID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
