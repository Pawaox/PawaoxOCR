﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Models
{
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message)
        {

        }
    }
}
