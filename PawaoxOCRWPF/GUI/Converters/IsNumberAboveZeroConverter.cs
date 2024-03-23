using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PawaoxOCRWPF.GUI.Converters
{
    public class IsNumberAboveZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAboveZero = false;

            if (value == null)
                return false;

            try
            {
                if (value is bool)
                    isAboveZero = (bool)value;
                else if (value is byte)
                    isAboveZero = ((byte)value) > 0;
                else if (value is sbyte)
                    isAboveZero = ((sbyte)value) > 0;
                else if (value is short)
                    isAboveZero = ((short)value) > 0;
                else if (value is ushort)
                    isAboveZero = ((ushort)value) > 0;
                else if (value is int)
                    isAboveZero = ((int)value) > 0;
                else if (value is uint)
                    isAboveZero = ((uint)value) > 0U;
                else if (value is long)
                    isAboveZero = ((long)value) > 0L;
                else if (value is ulong)
                    isAboveZero = ((ulong)value) > 0UL;
                else if (value is float)
                    isAboveZero = ((float)value) > 0.0F;
                else if (value is double)
                    isAboveZero = ((double)value) > 0.0D;
                else if (value is decimal)
                    isAboveZero = ((decimal)value) > 0.0M;
                else
                {
                    try
                    {
                        long res = -1;
                        if (long.TryParse(value?.ToString(), out res))
                            isAboveZero = res > 0;
                    }
                    catch { }

                    if (!isAboveZero)
                        try
                        {
                            double res = -1;
                            if (double.TryParse(value?.ToString(), out res))
                                isAboveZero = res > 0;
                        }
                        catch { }
                }
            }
            catch { }

            return isAboveZero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? 1 : 0;
        }
    }
}
