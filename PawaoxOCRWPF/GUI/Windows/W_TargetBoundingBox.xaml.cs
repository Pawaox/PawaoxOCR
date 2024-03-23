using MahApps.Metro.Controls;
using PawaoxOCRWPF.GUI.GUIModels;
using PawaoxOCRWPF.GUI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PawaoxOCRWPF
{
    public partial class W_TargetBoundingBox : MetroWindow, BoundingBox
    {
        private int _currentTop = 0, _currentLeft = 0, _currentWidth = 0, _currentHeight = 0;

        private bool _ignoreSizeChangedEvent = false;

        private Guid _sizeChangeID = Guid.Empty;
        private int _sizeChangeEventDelayMS = 100;

        private bool _isDragging;
        private Point _dragStartPoint;

        private int _preDragTop = 0, _preDragLeft = 0;
        private int _preSizeWidth = 0, _preSizeHeight = 0;
        private bool _isFirstSizeChange = true;

        private event BoundingBoxPositionChangedHandler _positionChangedEvent;
        event BoundingBoxPositionChangedHandler BoundingBox.PositionChangedEvent { add { _positionChangedEvent += value; } remove { _positionChangedEvent -= value; } }
        private event BoundingBoxSizeChangedHandler _sizeChangedEvent;
        event BoundingBoxSizeChangedHandler BoundingBox.SizeChangedEvent { add { _sizeChangedEvent += value; } remove { _sizeChangedEvent -= value; } }

        public W_TargetBoundingBox()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        public int GetWidth()
        {
            return _currentWidth;
        }
        public int GetHeight()
        {
            return _currentHeight;
        }

        public int GetLeftPosition()
        {
            return _currentLeft;
        }

        public int GetTopPosition()
        {
            return _currentTop;
        }


        public bool GetIgnoresInput()
        {
            return !this.IsHitTestVisible;
        }
        public double GetOpacity()
        {
            return this.Opacity;
        }

        public void SetWidth(int value)
        {
            _ignoreSizeChangedEvent = true;
            _currentWidth = value;
            this.Width = value;
            _ignoreSizeChangedEvent = false;
        }

        public void SetHeight(int value)
        {
            _ignoreSizeChangedEvent = true;
            _currentHeight = value;
            this.Height = value;
            _ignoreSizeChangedEvent = false;
        }

        public void SetLeftPosition(int value)
        {
            _currentLeft = value;
            this.Left = value;
        }

        public void SetTopPosition(int value)
        {
            _currentTop = value;
            this.Top = value;
        }

        public void SetIgnoresInput(bool ignoreInput)
        {
            this.IsHitTestVisible = !ignoreInput;
            root.IsHitTestVisible = !ignoreInput;
            brdClear.IsHitTestVisible = !ignoreInput;
            brdThick.IsHitTestVisible = !ignoreInput;

            try
            {
                var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                int windowStyle = GetWindowLong(hwnd, -20); // GET GWL_EXSTYLE

                if (ignoreInput)
                {
                    this.ResizeMode = ResizeMode.NoResize;

                    SetWindowLong(hwnd, -20, windowStyle | 0x20); // SET GWL_EXSTYLE To WS_EX_TRANSPARENT
                }
                else
                {
                    this.ResizeMode = ResizeMode.CanResizeWithGrip;

                    SetWindowLong(hwnd, -20, windowStyle & ~0x20); // Clear WS_EX_TRANSPARENT
                }
            }
            catch
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
        }

        public void SetOpacity(double opacity)
        {
            this.Opacity = opacity;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _currentWidth = (int)e.NewSize.Width;
            _currentHeight = (int)e.NewSize.Height;

            if (_ignoreSizeChangedEvent)
                return;

            if (_sizeChangedEvent != null)
            {
                Guid myID = Guid.NewGuid();

                _sizeChangeID = myID;

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Thread.Sleep(_sizeChangeEventDelayMS);

                        if (myID.Equals(_sizeChangeID))
                        {
                            if (_isFirstSizeChange)
                            {
                                _preSizeWidth = (int)e.NewSize.Width;
                                _preSizeHeight = (int)e.NewSize.Height;
                                _isFirstSizeChange = false;
                            }
                            else
                            {

                            }
                            Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    BoundingBoxSizeChanges changes = new BoundingBoxSizeChanges();
                                    changes.OldWidth = _preSizeWidth;
                                    changes.OldHeight = _preSizeHeight;
                                    changes.NewWidth = (int)e.NewSize.Width;
                                    changes.NewHeight = (int)e.NewSize.Height;
                                    changes.WidthChange = changes.NewWidth - changes.OldWidth;
                                    changes.HeightChange = changes.NewHeight - changes.OldHeight;

                                    _sizeChangedEvent.Invoke(this, changes);
                                }
                                catch { }
                            });
                        }
                    }
                    catch { }
                });
            }
        }

        private void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _isDragging = true;
                _dragStartPoint = e.GetPosition(root);

                _preDragTop = GetTopPosition();
                _preDragLeft = GetLeftPosition();

                brdClear.Visibility = Visibility.Collapsed;
                brdThick.Visibility = Visibility.Visible;
            }
        }

        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(root);
                double offsetX = mousePosition.X - _dragStartPoint.X;
                double offsetY = mousePosition.Y - _dragStartPoint.Y;

                this.Left = this.Left + offsetX;
                this.Top = this.Top + offsetY;

                _currentLeft = (int)this.Left;
                _currentTop = (int)this.Top;
            }
        }

        private void root_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;

                if (_positionChangedEvent != null)
                {
                    BoundingBoxPositionChanges changes = new BoundingBoxPositionChanges();
                    changes.OldTop = _preDragTop;
                    changes.OldLeft = _preDragLeft;
                    changes.NewTop = GetTopPosition();
                    changes.NewLeft = GetLeftPosition();
                    changes.TopChange = changes.NewTop - changes.OldTop;
                    changes.LeftChange = changes.NewLeft - changes.OldLeft;

                    if (changes.TopChange != 0 && changes.LeftChange != 0)
                        _positionChangedEvent.Invoke(this, changes);
                }


                brdClear.Visibility = Visibility.Visible;
                brdThick.Visibility = Visibility.Collapsed;
            }
        }
    }
}
