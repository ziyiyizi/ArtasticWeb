using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Utils
{
    public sealed class StringValueUtils
    {
        public static bool IsNullOrEmpty(StringValues sv)
        {
            return StringValues.IsNullOrEmpty(sv) || sv.Equals("null");
        }
    }
}
