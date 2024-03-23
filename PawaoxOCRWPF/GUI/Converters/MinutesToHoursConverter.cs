using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PawaoxOCRWPF.GUI.Converters
{
    public class MinutesToHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double minutes = 0;
            double hours = 0;

            if (double.TryParse(value?.ToString() ?? "", out minutes))
                hours = (minutes / 60d);

            return hours;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double minutes = 0;
            double hours = 0;

            if (double.TryParse(value?.ToString() ?? "", out hours))
                hours = Math.Floor(hours * 60d);

            return minutes;
        }
    }
}
