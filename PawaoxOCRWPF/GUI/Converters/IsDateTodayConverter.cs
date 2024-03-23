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
    public class IsDateTodayConverter : IValueConverter
    {
        public IsDateTodayConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = false;

            if (null == value)
                return res;

            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;

                if (DateTime.Today.Date.Equals(dateTime.Date))
                    res = true;
            }


            return res;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? date = null;

            if (null == value)
                return null;

            if (value is bool && (bool)value)
                date = DateTime.Today.Date;

            return date;
        }
    }
}
