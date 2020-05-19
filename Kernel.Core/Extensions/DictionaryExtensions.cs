using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static string GetValue(this Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key))
                return dict[key];

            return "";
        }
    }
}
