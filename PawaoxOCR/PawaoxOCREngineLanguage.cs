using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCR
{
    public enum PawaoxOCREngineLanguage
    {
        ENGLISH,
        JAPANESE
    }

    public static class PawaoxOCREngineLanguageExt
    {
        public static string GetTesseractLanguage(this PawaoxOCREngineLanguage lang)
        {
            string value = "";

            switch (lang)
            {
                case PawaoxOCREngineLanguage.ENGLISH:
                    value = "eng";
                    break;
                case PawaoxOCREngineLanguage.JAPANESE:
                    value = "jpn";
                    break;
                default:
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    break;
            }

            return value;
        }
    }
}
