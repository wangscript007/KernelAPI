using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Models
{
    public class DateFormat : IsoDateTimeConverter
    {
        public DateFormat()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
    public class DateTimeFormat : IsoDateTimeConverter
    {
        public DateTimeFormat()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
