using PawaoxOCRWPF.GUI.Views.Windows.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Messaging
{
    public class MSG_DialogWarning : BaseMessage
    {
        public WindowMessageWarningOptions Options { get; private set; }

        public Action<bool> OnClose { get; set; }

        public MSG_DialogWarning(WindowMessageWarningOptions options, Action<bool> onClose)
        {
            this.Options = options;

            this.OnClose = onClose;
        }
    }
}
