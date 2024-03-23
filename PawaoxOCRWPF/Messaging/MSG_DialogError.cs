using PawaoxOCRWPF.GUI.Views.Windows.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Messaging
{
    public class MSG_DialogError : BaseMessage
    {
        public WindowMessageErrorOptions Options { get; private set; }

        public Action OnClose { get; set; }

        public MSG_DialogError(WindowMessageErrorOptions options, Action onClose)
        {
            this.Options = options;

            this.OnClose = onClose;
        }
    }
}
