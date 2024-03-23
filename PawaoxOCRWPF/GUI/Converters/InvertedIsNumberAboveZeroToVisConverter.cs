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
    public class InvertedIsNumberAboveZeroToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            IsNumberAboveZeroToVisConverter conv = new IsNumberAboveZeroToVisConverter();
            object obj = conv.Convert(value, targetType, parameter, culture);

            if (obj is Visibility)
                return (Visibility)obj is Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
