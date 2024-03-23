using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PawaoxOCR
{
    public class PawaoxTesseractEngine : IOCREngine
    {
        private bool _isDisposed;

        private TesseractEngine _engine;

        private string _dataPath;
        private string _languageCode;

        public PawaoxTesseractEngine(string dataPath, PawaoxOCREngineLanguage language) : this(dataPath, language.GetTesseractLanguage()) { }
        public PawaoxTesseractEngine(string dataPath, string tesseractLanguageCode)
        {
            _dataPath = dataPath;
            _languageCode = tesseractLanguageCode;

            CreateEngine();
        }

        ~PawaoxTesseractEngine()
        {
            Dispose(false);
        }

        private void CreateEngine()
        {
            if (string.IsNullOrEmpty(_languageCode))
                throw new PawaoxOCRException("Cannot create 'PawaoxTesseractEngine' - Language is not set");
            _engine = new TesseractEngine(_dataPath, _languageCode, EngineMode.Default);
        }

        public string GetLanguage()
        {
            return _languageCode;
        }

        public void ChangeLanguage(string languageCode)
        {
            try
            {
                _languageCode = languageCode;

                CreateEngine();
            }
            catch
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
        }

        public ProcessResult Process(byte[] imageData)
        {
            if (_engine == null || imageData == null || imageData.Length == 0)
                return null;

            ProcessResult result = new ProcessResult();

            List<string> blockLines = new List<string>();

            using (Pix image = Pix.LoadFromMemory(imageData))
            {
                using (Page page = _engine.Process(image))
                {
                    result.RawParsedText = page.GetText();
                    result.MeanConfidence = page.GetMeanConfidence();

                    
                    using (var it = page.GetIterator())
                    {
                        it.Begin();
                        while (it.Next(PageIteratorLevel.Block))
                        {
                            while (it.Next(PageIteratorLevel.Para))
                            {
                                while (it.Next(PageIteratorLevel.TextLine))
                                {
                                    string line = "";

                                    while (it.Next(PageIteratorLevel.Word))
                                    {
                                        if (it.IsAtBeginningOf(PageIteratorLevel.Block))
                                        {
                                            if (!string.IsNullOrEmpty(line))
                                                blockLines.Add(line);

                                            line = "";
                                        }

                                        string word = it.GetText(PageIteratorLevel.Word);
                                        if (!string.IsNullOrEmpty(word))
                                        {
                                            if (!string.IsNullOrEmpty(line))
                                                line += " ";
                                            line += word.Trim();
                                        }

                                        if (it.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                        {
                                            if (!string.IsNullOrEmpty(line))
                                                blockLines.Add(line);

                                            line = "";
                                        }
                                    }

                                    if (it.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                    {
                                        if (!string.IsNullOrEmpty(line))
                                            blockLines.Add(line);

                                        line = "";
                                    }
                                }
                            }

                            result.Blocks.Add(blockLines);
                        }

                        result.Success = true;
                    }
                }
            }

            return result;
        }



        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                try
                {
                    if (_engine != null)
                    {
                        _engine.Dispose();
                        _engine = null;
                    }
                }
                catch { }

                _isDisposed = true;
            }
        }


        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
