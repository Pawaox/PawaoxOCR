using ControlzEx.Standard;
using PawaoxOCR;
using PawaoxOCRWPF.GUI.GUIModels;
using PawaoxOCRWPF.GUI.MVVM;
using PawaoxOCRWPF.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PawaoxOCRWPF.GUI.ViewModels
{
    public class VM_OCR : ViewModel, IDisposable
    {
        PawaoxOCREngine _ocrEngine;

        private System.Timers.Timer _timerCapture;
        int _prevCaptureIntervalMS = -1;

        #region Properties
        #region Output
        private string _output;
        public string Output { get { return _output; } set { SetValue(ref _output, value); } }

        private string _outputTimestamp;
        public string OutputTimestamp { get { return _outputTimestamp; } set { SetValue(ref _outputTimestamp, value); } }
        #endregion

        private byte _runningMode = 1;
        public byte RunningMode { get { return _runningMode; } set { SetValue(ref _runningMode, value); } }
        private bool _isRunning;
        public bool IsRunning { get { return _isRunning; } set { SetValue(ref _isRunning, value); } }


        private bool _isShowingParseArea;
        public bool IsShowingParseArea { get { return _isShowingParseArea; } set { SetValue(ref _isShowingParseArea, value); } }


        private bool _autoCopyOutputToClipboard;
        public bool AutoCopyOutputToClipboard { get { return _autoCopyOutputToClipboard; } set { SetValue(ref _autoCopyOutputToClipboard, value); } }
        private bool _showRawOutput;
        public bool ShowRawOutput { get { return _showRawOutput; } set { SetValue(ref _showRawOutput, value); } }


        private BoundingBox _targetBoundingBox;
        public BoundingBox TargetBoundingBox { get { return _targetBoundingBox; } set { SetValue(ref _targetBoundingBox, value); } }

        #region Parse Area Values
        private string _parseTopPosition;
        public string ParseTopPosition { get { return _parseTopPosition; } set { SetValue(ref _parseTopPosition, value); } }
        private string _parseLeftPosition;
        public string ParseLeftPosition { get { return _parseLeftPosition; } set { SetValue(ref _parseLeftPosition, value); } }
        private string _parseWidth;
        public string ParseWidth { get { return _parseWidth; } set { SetValue(ref _parseWidth, value); } }
        private string _parseHeight;
        public string ParseHeight { get { return _parseHeight; } set { SetValue(ref _parseHeight, value); } }

        private string _parseTransparency;
        public string ParseTransparency { get { return _parseTransparency; } set { SetValue(ref _parseTransparency, value); } }

        private string _captureInterval;
        public string CaptureInterval { get { return _captureInterval; } set { SetValue(ref _captureInterval, value); } }


        private List<PawaoxOCREngineType> _engineTypes;
        public List<PawaoxOCREngineType> EngineTypes { get { return _engineTypes; } set { SetValue(ref _engineTypes, value); } }
        private PawaoxOCREngineType _selectedEngineType;
        public PawaoxOCREngineType SelectedEngineType { get { return _selectedEngineType; } set { SetValue(ref _selectedEngineType, value); } }

        private HashSet<string> _languages;
        public HashSet<string> Languages { get { return _languages; } set { SetValue(ref _languages, value); } }
        private string _selectedLanguage;
        public string SelectedLanguage { get { return _selectedLanguage; } set { SetValue(ref _selectedLanguage, value); } }

        #endregion

        #region Commands
        public RelayCommand CommandToggleParseAreaVisibility { get; private set; }

        public RelayCommand CommandOnOutputChanged { get; private set; }
        public RelayCommand CommandOnAutoClipboardOutputChanged { get; private set; }

        public RelayCommand CommandParseTopChanged { get; private set; }
        public RelayCommand CommandParseLeftChanged { get; private set; }
        public RelayCommand CommandParseWidthChanged { get; private set; }
        public RelayCommand CommandParseHeightChanged { get; private set; }
        public RelayCommand CommandParseTransparencyChanged { get; private set; }

        public RelayCommand CommandToggleParseAreaIgnoreInput { get; private set; }

        public RelayCommand CommandCaptureIntervalChanged { get; private set; }

        public RelayCommand CommandStartParser { get; private set; }
        public RelayCommand CommandStopParser { get; private set; }

        public RelayCommand CommandSelectedEngineTypeChanged { get; private set; }
        public RelayCommand CommandSelectedEngineLanguageChanged { get; private set; }
        #endregion
        #endregion

        public VM_OCR() : base(UserControlType.OCR)
        {
            Languages = new HashSet<string>();
            EngineTypes = new List<PawaoxOCREngineType>();

            CommandToggleParseAreaVisibility = new RelayCommand(ToggleParseAreaVisibility);
            CommandOnOutputChanged = new RelayCommand(OnOutputChanged);
            CommandOnAutoClipboardOutputChanged = new RelayCommand(OnAutoClipboardOutputChanged);

            CommandParseTopChanged = new RelayCommand(OnParseTopChanged);
            CommandParseLeftChanged = new RelayCommand(OnParseLeftChanged);
            CommandParseWidthChanged = new RelayCommand(OnParseWidthChanged);
            CommandParseHeightChanged = new RelayCommand(OnParseHeightChanged);
            CommandParseTransparencyChanged = new RelayCommand(OnParseTransparency);

            CommandToggleParseAreaIgnoreInput = new RelayCommand(ToggleParseAreaIgnoreInput);

            CommandStartParser = new RelayCommand(StartParser);
            CommandStopParser = new RelayCommand(StopParser);

            CommandSelectedEngineTypeChanged = new RelayCommand(SelectedEngineTypeChanged);
            CommandSelectedEngineLanguageChanged = new RelayCommand(SelectedEngineLanguageChanged);

            CommandCaptureIntervalChanged = new RelayCommand(CaptureIntervalChanged);
        }

        private void CreateCaptureTimer()
        {
            if (_ocrEngine == null)
            {
                ErrorHandler.Error("Cannot create Capture Timer - No OCR Engine has been initialized!");
                return;
            }

            try
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    try
                    {
                        if (_timerCapture != null && _timerCapture.Enabled)
                            _timerCapture.Stop();

                        int captureInterval = 1000;
                        int.TryParse(CaptureInterval, out captureInterval);

                        _prevCaptureIntervalMS = captureInterval;
                        _timerCapture = new System.Timers.Timer(captureInterval);
                        _timerCapture.Elapsed += _timerCapture_Elapsed;
                        _timerCapture.Start();
                    }
                    catch (Exception exc)
                    {
                        ErrorHandler.Exception(exc);
                    }
                });
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void _timerCapture_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_ocrEngine == null || _targetBoundingBox == null)
                return;

            try
            {
                _timerCapture.Stop();

                if (RunningMode == 1)
                    return;

                int top = _targetBoundingBox.GetTopPosition();
                int left = _targetBoundingBox.GetLeftPosition();
                int width = _targetBoundingBox.GetWidth();
                int height = _targetBoundingBox.GetHeight();

                if (width < 5 || height < 5)
                {
                    ErrorHandler.Error("Target area is too small! (Width & Height should be >= 5)");
                    return;
                }
                Bitmap image = ScreenshotHelper.CaptureScreenRegion(left, top, width, height);

                if (true)
                {
                    BitmapImageSettings settings = new BitmapImageSettings()
                    {
                        Brightness = -100,
                        Contrast = 100,
                        InvertColorsPostProcess = true,
                        ResizePercentSize = 50.0
                    };
                    image = BitmapImageHelper.ImplementSettings(image, settings);

                    image.Save("C:/Temp/OCR/0_Debug.png");
                    /*
                    image.Save("C:/Temp/OCR/1_base.png");


                    Bitmap img1 = BitmapImageHelper.AdjustBrightnessContrast(image, -100, 100);
                    img1.Save("C:/Temp/OCR/2_img.png");
                    img1.Dispose();

                    Bitmap img2 = BitmapImageHelper.AdjustBrightnessContrast(image, -255, 255);
                    img2.Save("C:/Temp/OCR/3_img.png");
                    img2.Dispose();

                    Bitmap img3 = BitmapImageHelper.AdjustBrightnessContrast(image, -200, 200);
                    img3.Save("C:/Temp/OCR/4_img.png");
                    img3.Dispose();

                    Bitmap img4 = BitmapImageHelper.AdjustBrightnessContrast(image, -50, 50);
                    img4.Save("C:/Temp/OCR/5_img.png");
                    img4.Dispose();
                    */

                }
                byte[] bytes = GUIHelper.ImageToByteArray(image);

                ProcessResult result = _ocrEngine.ProcessImage(bytes);

                if (result != null)
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        if (ShowRawOutput)
                            this.Output = result.RawParsedText;
                        else
                        {
                            string work = result.RawParsedText;
                            work = work.Replace(@"\r\n\r\n", @"\r\n");
                            work = work.Replace(@"\r\r", @"\r");
                            work = work.Replace(@"\r", @"\n");
                            work = work.Replace(@"\n\n", @"\n");


                            string[] splt = work.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                            StringBuilder sb = new StringBuilder();
                            foreach (string line in splt)
                            {
                                string lin = line.Trim();

                                if (string.IsNullOrEmpty(lin))
                                    continue;

                                sb.AppendLine(line);
                            }

                            this.Output = sb.ToString();
                            this.OutputTimestamp = DateTime.Now.ToString("HH:mm:ss.ff") + " - Confidence: " + (result?.MeanConfidence.ToString() ?? "");
                        }
                    });
                }
                else
                {
                    this.Output = "";
                    this.OutputTimestamp = DateTime.Now.ToString("HH:mm:ss.ff");
                }
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
            finally
            {
                _timerCapture.Start();
            }
        }

        private void CaptureIntervalChanged()
        {
            try
            {
                if (int.TryParse(CaptureInterval, out int interval))
                {

                    if (_prevCaptureIntervalMS == interval)
                        return;

                    if (!IsRunning)
                        return;

                    CreateCaptureTimer();
                }
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void SelectedEngineTypeChanged()
        {
            try
            {
                if (IsRunning)
                {
                    ErrorHandler.Error("Cannot change Engine while it is running!");
                    return;
                }

                string dataPath = "C://Temp//tesseractData//";

                string errorMessage = "";


                HashSet<string> langs = PawaoxOCREngine.GetPossibleLanguages(SelectedEngineType, dataPath, out errorMessage);
                if (string.IsNullOrEmpty(errorMessage))
                    this.Languages = langs;
                else
                {
                    this.Languages = new HashSet<string>();
                    ErrorHandler.Error(errorMessage);
                }
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void SelectedEngineLanguageChanged()
        {
            try
            {
                if (IsRunning)
                {
                    ErrorHandler.Error("Cannot change Engine while it is running!");
                    return;
                }

                if (string.IsNullOrEmpty(SelectedLanguage))
                    return;

                if (_ocrEngine != null)
                    _ocrEngine.ChangeLanguage(SelectedLanguage);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private async void StartParser()
        {
            try
            {
                await Task.Factory.StartNew(() => { RunningMode = 2; });

                int width = _targetBoundingBox.GetWidth();
                int height = _targetBoundingBox.GetHeight();

                if (width < 5 || height < 5)
                {
                    RunningMode = 1;
                    ErrorHandler.Error("Target area is too small! (Width & Height should be >= 5)");
                    return;
                }

                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(420);

                    try
                    {
                        if (_ocrEngine == null)
                            _ocrEngine = new PawaoxOCREngine();

                        switch (SelectedEngineType)
                        {
                            case PawaoxOCREngineType.TESSERACT:
                                _ocrEngine.UseTesseractEngine("C://Temp//tesseractData//", SelectedLanguage);
                                break;
                        }

                        CreateCaptureTimer();
                        RunningMode = 3;
                    }
                    catch (Exception exc)
                    {
                        RunningMode = 1;
                        ErrorHandler.Exception(exc);
                    }
                });
            }
            catch (Exception exc)
            {
                RunningMode = 1;
                ErrorHandler.Exception(exc);
            }
        }

        private void StopParser()
        {
            try
            {
                RunningMode = 1;
                _timerCapture.Stop();
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void ToggleParseAreaIgnoreInput()
        {
            try
            {
                if (_targetBoundingBox == null)
                    return;

                bool value = _targetBoundingBox.GetIgnoresInput();
                _targetBoundingBox.SetIgnoresInput(!value);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }
        private void OnParseTopChanged()
        {
            try
            {
                if (_targetBoundingBox == null)
                    return;

                if (int.TryParse(ParseTopPosition, out int top))
                    _targetBoundingBox.SetTopPosition(top);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }
        private void OnParseLeftChanged()
        {
            try
            {
                if (_targetBoundingBox == null)
                    return;

                if (int.TryParse(ParseLeftPosition, out int left))
                    _targetBoundingBox.SetLeftPosition(left);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }
        private void OnParseWidthChanged()
        {
            try
            {
                if (_targetBoundingBox == null)
                    return;

                if (int.TryParse(ParseWidth, out int width))
                    _targetBoundingBox.SetWidth(width);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }
        private void OnParseHeightChanged()
        {
            try
            {
                if (_targetBoundingBox == null)
                    return;

                if (int.TryParse(ParseHeight, out int height))
                    _targetBoundingBox.SetHeight(height);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }
        private void OnParseTransparency()
        {
            if (_targetBoundingBox == null)
                return;

            try
            {
                int transparency = 0;

                if (!string.IsNullOrEmpty(ParseTransparency))
                {
                    if (int.TryParse(ParseTransparency, out transparency))
                    {
                        if (transparency < 0)
                        {
                            transparency = 0;
                        }
                        else if (transparency > 100)
                        {
                            transparency = 100;
                        }
                    }
                    else if (ParseTransparency.Length > 0)
                    {
                        return;
                    }
                }

                double newOpacity = 1 - transparency / 100.0;
                double currentOpacity = _targetBoundingBox.GetOpacity();

                if (currentOpacity != newOpacity)
                    _targetBoundingBox.SetOpacity(newOpacity);
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void OnOutputChanged()
        {
            try
            {
                if (AutoCopyOutputToClipboard)
                    Clipboard.SetDataObject(this.Output ?? "");
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }


        private void OnAutoClipboardOutputChanged()
        {
            try
            {
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void ToggleParseAreaVisibility()
        {
            try
            {
                this.IsShowingParseArea = !this.IsShowingParseArea;

                if (this.IsShowingParseArea)
                {
                    _targetBoundingBox.Show();

                    if (string.IsNullOrEmpty(ParseTopPosition))
                        ParseTopPosition = _targetBoundingBox.GetTopPosition().ToString();
                    if (string.IsNullOrEmpty(ParseLeftPosition))
                        ParseLeftPosition = _targetBoundingBox.GetLeftPosition().ToString();
                }
                else
                    _targetBoundingBox.Hide();
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        public override void OnLoaded()
        {
            try
            {
                if (_targetBoundingBox != null)
                {
                    _targetBoundingBox.PositionChangedEvent += _targetBoundingBox_PositionChangedEvent;
                    _targetBoundingBox.SizeChangedEvent += _targetBoundingBox_SizeChangedEvent;
                }

                ParseTopPosition = "0";
                ParseLeftPosition = "0";
                _targetBoundingBox.SetTopPosition(0);
                _targetBoundingBox.SetLeftPosition(0);

                CaptureInterval = "1000";

                List<PawaoxOCREngineType> types = new List<PawaoxOCREngineType>();
                foreach (PawaoxOCREngineType typ in Enum.GetValues(typeof(PawaoxOCREngineType)))
                {
                    if (typ == PawaoxOCREngineType.UNKNOWN)
                        continue;
                    types.Add(typ);
                }
                this.EngineTypes = types;
                if (types.Count > 0)
                {
                    SelectedEngineType = types[0];
                    SelectedEngineTypeChanged();

                    if (Languages.Contains("eng"))
                        SelectedLanguage = "eng";
                }
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void _targetBoundingBox_SizeChangedEvent(BoundingBox sender, BoundingBoxSizeChanges changes)
        {
            try
            {
                ParseWidth = changes.NewWidth.ToString();
                ParseHeight = changes.NewHeight.ToString();
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        private void _targetBoundingBox_PositionChangedEvent(BoundingBox sender, BoundingBoxPositionChanges changes)
        {
            try
            {
                ParseTopPosition = changes.NewTop.ToString();
                ParseLeftPosition = changes.NewLeft.ToString();
            }
            catch (Exception exc)
            {
                ErrorHandler.Exception(exc);
            }
        }

        public override void OnUnloaded()
        {
        }

        public void Dispose()
        {
            try
            {
                if (_ocrEngine != null)
                    ((IDisposable)_ocrEngine).Dispose();
            }
            catch (Exception exc)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
        }
    }
}
