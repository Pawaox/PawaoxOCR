using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PawaoxOCRWPF.GUI.Converters
{
    public class ListHasElementsToVisInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = (Visibility) new ListHasElementsToVisConverter().Convert(value, targetType, parameter, culture);

            return result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object res = null;
            
            return res;
        }
    }
}
