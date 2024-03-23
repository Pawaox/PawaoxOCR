using PawaoxOCRWPF.GUI.GUIModels;
using PawaoxOCRWPF.GUI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.ViewModels
{
    public abstract class ViewModel : ViewModelBase
    {
        private UserControlType _currentType;
        public UserControlType CurrentType
        {
            get { return _currentType; }
            set { SetValue(ref _currentType, value); }
        }

        public RelayCommand CommandOnLoadedEvent { get; private set; }
        public RelayCommand CommandOnUnloadedEvent { get; private set; }

        public ViewModel(UserControlType uct)
        {
            CommandOnLoadedEvent = new RelayCommand(OnLoadedEvent);
            CommandOnUnloadedEvent = new RelayCommand(OnUnloadedEvent);
        }
        private void OnLoadedEvent()
        {
            OnLoaded();
        }

        private void OnUnloadedEvent()
        {
            OnUnloaded();
        }
        abstract public void OnLoaded();
        abstract public void OnUnloaded();
    }
}
