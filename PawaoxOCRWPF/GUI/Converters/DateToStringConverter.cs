using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PawaoxOCRWPF.GUI.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        string _format = "";
        DateTime? _minDate = null;

        public DateToStringConverter() { }
        public DateToStringConverter(string format) { _format = format; }
        public DateToStringConverter(string format, DateTime minDate) { _format = format; _minDate = minDate; }
        public DateToStringConverter(DateTime minDate) { _minDate = minDate; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = "";

            if (null == value)
                return null;

            if (value is DateTime)
            {
                DateTime time = (DateTime)value;

                if (_minDate.HasValue && _minDate > time)
                    return "";

                if (!string.IsNullOrEmpty(_format))
                    res = time.ToString(_format);
                else
                    res = time.ToShortDateString();
            }

            return res;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
                return null;

            object result = null;

            return result;
        }
    }
}
