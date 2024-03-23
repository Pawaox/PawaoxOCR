using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Messaging
{
    public class MSG_DisplayMessage : BaseMessage
    {
        public Exception Exception { get; set; } = null;

        public string Message { get; set; } = "";
        public bool IsError { get; set; } = false;

        public MSG_DisplayMessage() { }
        public MSG_DisplayMessage(bool error, string message)
        {
            this.IsError = error;
            this.Message = message;
        }
    }
}
