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
    public class NumberMaxDecimalsConverter : IValueConverter
    {
        private int _maxDecimals;
        public int MaxDecimals
        {
            get { return _maxDecimals; }
            set { _maxDecimals = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = "";

            string val = value?.ToString() ?? "";
            if (double.TryParse(val, out double number))
            {
                string left = "";
                string right = "";

                int index = val.IndexOf(",");

                if (index <= 0)
                    index = val.IndexOf(".");

                if (index > 0)
                {
                    left = val.Substring(0, index);
                    right = val.Substring(index + 1);
                    if (right.Length > MaxDecimals)
                        right = right.Substring(0, MaxDecimals);

                    result = left + CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator + right;
                }
                else
                    result = value ?? "";
            }

            return result ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
