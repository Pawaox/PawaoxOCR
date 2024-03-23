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
    public class BooleanToVisibilityConverterHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Hidden;

            Visibility vis = value == null ? Visibility.Hidden : (((bool)value) ? Visibility.Visible : Visibility.Hidden);
            return vis;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : (Visibility.Visible.Equals(value));
        }
    }
}
