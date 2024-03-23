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
    public class NullToVisiblityInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Hidden;

            if (value is string)
                result = string.IsNullOrEmpty(((string)value)) ? Visibility.Collapsed : Visibility.Visible;
            else
                result = value == null ? Visibility.Collapsed : Visibility.Visible; ;

            return result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object res = null;

            if (value is Visibility)
            {
                switch ((Visibility)value)
                {
                    case Visibility.Visible:
                        res = new object();
                        break;
                    case Visibility.Hidden:
                    case Visibility.Collapsed:
                        res = null;
                        break;
                }
            }

            return res;
        }
    }
}
