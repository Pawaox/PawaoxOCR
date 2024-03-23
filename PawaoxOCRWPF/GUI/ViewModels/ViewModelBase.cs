using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetValue<T>(ref T orig, T newValue, [CallerMemberName] string propertyName = "")
        {
            bool didChange = false;

            if (!EqualityComparer<T>.Default.Equals(orig, newValue))
            {
                orig = newValue;
                RaisePropertyChanged(propertyName);
                didChange = true;
            }

            return didChange;
        }

        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
