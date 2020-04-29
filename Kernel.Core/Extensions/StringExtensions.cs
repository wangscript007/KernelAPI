using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kernel.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TrimStart(this string oldValue, string trimString, bool trimWhiteSpace = false)
        {
            string whiteSpace = trimWhiteSpace ? @"\s*" : "";
            string strRegex = $@"^({trimString}{whiteSpace})";
            return Regex.Replace(oldValue, strRegex, "");
        }
    }
}
