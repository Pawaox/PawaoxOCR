using PawaoxOCR;
using PawaoxOCRWPF.Messaging;
using PawaoxOCRWPF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Helpers
{
    public static class ErrorHandler
    {
        public static void Error(string message)
        {
            MessageBroker.Send(new MSG_DisplayMessage(true, message));
        }

        public static void Exception(Exception exc)
        {
            if (exc.GetType() == typeof(TaskCanceledException))
            {
                MessageBroker.Send(new MSG_DisplayMessage(true, exc.Message) { Exception = exc });
            }
            else if (exc.GetType() == typeof(MessageException))
            {
                MessageBroker.Send(new MSG_DisplayMessage(true, exc.Message) { Exception = exc });
            }
            else if (exc.GetType() == typeof(PawaoxOCRException))
            {
                MessageBroker.Send(new MSG_DisplayMessage(true, exc.Message) { Exception = exc });
            }
            else
            {
                if (Debugger.IsAttached)
                    Debugger.Break();

                MessageBroker.Send(new MSG_DisplayMessage(true, exc.Message));
            }
        }
    }
}
