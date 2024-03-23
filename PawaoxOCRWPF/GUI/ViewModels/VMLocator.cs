using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.ViewModels
{
    public static class VMLocator
    {
        public static Dictionary<Type, ViewModel> _viewModels = new Dictionary<Type, ViewModel>();

        public static T GetViewModel<T>() where T : ViewModel
        {
            T vm;

            Type t = typeof(T);

            if (!_viewModels.ContainsKey(t))
            {
                vm = (T)Activator.CreateInstance(t);
                _viewModels.Add(t, vm);
            }
            else
            {
                vm = (T)_viewModels[t];
            }

            return vm;
        }
    }
}
