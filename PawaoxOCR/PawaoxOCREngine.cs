using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCR
{
    public class PawaoxOCREngine
    {
        private IOCREngine _currentEngine;

        public PawaoxOCREngineType CurrentEngineType { get; private set; }

        public PawaoxOCREngine()
        {
            CurrentEngineType = PawaoxOCREngineType.UNKNOWN;
        }

        public void ChangeLanguage(string languageCode)
        {
            if (_currentEngine == null)
                return;

            string currentLanguage = _currentEngine.GetLanguage();

            if (!string.Equals(currentLanguage, languageCode))
                _currentEngine.ChangeLanguage(languageCode);
        }

        public ProcessResult ProcessImage(byte[] imageData)
        {
            if (_currentEngine == null)
                return null;
            return _currentEngine.Process(imageData);
        }

        public bool UseTesseractEngine(string dataPath, PawaoxOCREngineLanguage lang)
        {
            if (CurrentEngineType == PawaoxOCREngineType.TESSERACT)
                return false;

            _currentEngine.Dispose();
            _currentEngine = new PawaoxTesseractEngine(dataPath, lang);

            return true;
        }
        public bool UseTesseractEngine(string dataPath, string tesseractLanguageCode)
        {
            if (CurrentEngineType == PawaoxOCREngineType.TESSERACT)
                return false;

            if (_currentEngine != null)
                _currentEngine.Dispose();
            _currentEngine = new PawaoxTesseractEngine(dataPath, tesseractLanguageCode);

            return true;
        }


        public static HashSet<string> GetPossibleLanguages(PawaoxOCREngineType engineType, string dataPath, out string errorMessage)
        {
            errorMessage = "";
            HashSet<string> languages = new HashSet<string>();

            switch(engineType)
            {
                case PawaoxOCREngineType.TESSERACT:
                    languages = GetTesseractLanguages(dataPath, out errorMessage);
                    break;
            }

            return languages;
        }

        private static HashSet<string> GetTesseractLanguages(string dataPath, out string errorMessage)
        {
            errorMessage = "";
            HashSet<string> languages = new HashSet<string>();

            if (!Directory.Exists(dataPath))
            {
                errorMessage = "Couldn't find the datapath directory (" + dataPath + ")";
                return languages;
            }
            string[] files = Directory.GetFiles(dataPath);

            if (files == null || files.Length == 0)
            {
                errorMessage = "No files found in datapath directory (" + dataPath + ")";
                return languages;
            }

            HashSet<string> blockedFiles = new HashSet<string>();
            blockedFiles.Add("LICENSE");
            blockedFiles.Add("README");

            foreach (var file in Directory.GetFiles(dataPath))
            {
                string lang = Path.GetFileNameWithoutExtension(file);

                if (!file.EndsWith("traineddata"))
                    continue;

                if (blockedFiles.Contains(lang))
                    continue;


                if (string.IsNullOrEmpty(lang))
                    continue;
                if (!languages.Contains(lang))
                    languages.Add(lang);
            }

            return languages;
        }
    }
}
