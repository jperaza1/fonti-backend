using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common.Clases
{
    public class DateService
    {
        public static DateTime CurrentDate => DateTime.Now.ToUniversalTime().Date;

        public static DateTime CurrentDateTime => DateTime.Now.ToUniversalTime();

        public static DateTime GetCLientLocalTime(DateTime date, string timezone) 
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, timezone);
        }
    }
}
