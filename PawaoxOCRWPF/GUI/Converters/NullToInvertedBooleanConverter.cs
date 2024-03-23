using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PawaoxOCRWPF.GUI.Converters
{
    public class NullToInvertedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (value is string)
                result = !string.IsNullOrEmpty(((string)value));
            else
                result = value != null;

            return !result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object res = null;

            if (value is bool)
                res = ((bool)value) ? null : "";

            return res;
        }
    }
}
