using PawaoxOCRWPF.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.GUIModels
{
    public class TabModel : ViewModelBase
    {
        private UserControlType _type;
        public UserControlType Type
        {
            get { return _type; }
            set { SetValue(ref _type, value); }
        }

        private string _header;
        public string Header
        {
            get { return _header; }
            set { SetValue(ref _header, value); }
        }

        private string _explanation;
        public string Explanation
        {
            get { return _explanation; }
            set { SetValue(ref _explanation, value); }
        }

        private bool _isHidden;
        public bool IsHidden
        {
            get { return _isHidden; }
            set { SetValue(ref _isHidden, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }
    }
}
