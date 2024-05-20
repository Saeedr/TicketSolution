using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.UtilityFunc
{
    public class UtilityFunc
    {
        public static string ConvertDateTimeToShamsi(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return String.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
        }
    }
}
