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
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Collapsed;

            Visibility vis = value == null ? Visibility.Collapsed : (((bool)value) ? Visibility.Visible : Visibility.Collapsed);
            return vis;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : (Visibility.Visible.Equals(value));
        }
    }
}
