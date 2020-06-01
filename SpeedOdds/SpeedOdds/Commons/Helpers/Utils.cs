using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Commons.Helpers
{
    public static class Utils
    {
        public static string FormatDateTimeToGrid(DateTime _date)
        {
            return _date.ToString("dd-MM-yyyy ") +
                _date.TimeOfDay.Hours.ToString() + "h" +
                (_date.TimeOfDay.Minutes < 10 ? "0" : "") +
                _date.TimeOfDay.Minutes.ToString();
        }
    }
}
