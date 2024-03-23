using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.Views.Windows.Options
{
    public class WindowMessageWarningOptions : WindowOptionBase
    {
        public WindowMessageWarningOptions()
        {
            this.Title = "Confirmation Required";
            this.Explanation = "";
            this.ButtonContinueText = "Continue";
            this.ButtonCloseText = "Cancel";
        }
    }
}
