using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.GUIModels
{
    public interface UIToaster
    {
        void ShowToasts(bool isError, string msg);
    }
}