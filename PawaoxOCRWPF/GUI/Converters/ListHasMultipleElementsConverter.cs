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
    public class ListHasMultipleElementsConverter : IValueConverter
    {
        private int? _moreThan;
        public int? MoreThan
        {
            get { return _moreThan; }
            set { _moreThan = value; }
        }
        private int? _lessThan;
        public int? LessThan
        {
            get { return _lessThan; }
            set { _lessThan = value; }
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

                if (MoreThan.HasValue)
                {
                    result = count > MoreThan.Value;
                }
                else if (LessThan.HasValue)
                {
                    result = count < LessThan.Value;
                }
                else
                    result = false;
            }

            return (AsVisibility ? (object)(result ? VisIfTrue : VisIfFalse) : result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object res = null;
            
            return res;
        }
    }
}
