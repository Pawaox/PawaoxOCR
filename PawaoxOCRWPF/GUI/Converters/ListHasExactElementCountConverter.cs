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
    public class ListHasExactElementCountConverter : IValueConverter
    {
        private int _target;
        public int Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private bool _asVisibility;
        public bool AsVisibility
        {
            get { return _asVisibility; }
            set { _asVisibility = value; }
        }

        private Visibility _visIfTrue;
        public Visibility VisIfTrue
        {
            get { return _visIfTrue; }
            set { _visIfTrue = value; }
        }

        private Visibility _visIfFalse;
        public Visibility VisIfFalse
        {
            get { return _visIfFalse; }
            set { _visIfFalse = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (value is IEnumerable)
            {
                IEnumerable lst = (IEnumerable)value;
                int count = 0;

                foreach (var v in lst)
                    count++;

                result = count == Target;
            }

            if (AsVisibility)
                return result ? VisIfTrue : VisIfFalse;
            else
                return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object res = null;
            
            return res;
        }
    }
}
