using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCR
{
    public interface IOCREngine : IDisposable
    {
        void ChangeLanguage(string languageCode);
        string GetLanguage();
        ProcessResult Process(byte[] imageData);
    }
}
