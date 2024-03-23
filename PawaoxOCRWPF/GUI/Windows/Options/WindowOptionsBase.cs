using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.Views.Windows.Options
{
    public class WindowOptionBase
    {
        public string Title { get; set; } = "";
        public string Explanation { get; set; } = "";
        public string ButtonContinueText { get; set; } = "";
        public string ButtonCloseText { get; set; } = "";

        public WindowOptionBase()
        {
            ButtonContinueText = "Continue";
            ButtonCloseText = "Cancel";
        }
    }
}
