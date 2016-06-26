using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSer
{
    class DateTimeHelper
    {
        // datetime to unix
        private static ulong ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            //return the total seconds (which is a UNIX timestamp)
            return (ulong)span.TotalSeconds;
        }
        //unix to datetime
        private static DateTime ConvertToDateTime(double value)
        {
            DateTime retvalue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            retvalue = retvalue.AddSeconds((double)value);
            return retvalue.ToLocalTime();
        }
    }
}
