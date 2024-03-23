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
    public class IsNumberAboveZeroToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = Visibility.Collapsed;

            if (value == null)
                return false;

            try
            {
                IsNumberAboveZeroConverter conv = new IsNumberAboveZeroConverter();

                object resultObj = conv.Convert(value, targetType, parameter, culture);

                if (resultObj is bool)
                {
                    bool result = (bool)resultObj;

                    if (result)
                        vis = Visibility.Visible;
                }
            }
            catch { }

            return vis;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
