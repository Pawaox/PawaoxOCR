using PawaoxOCRWPF.GUI.ViewModels;
using PawaoxOCRWPF.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
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

namespace PawaoxOCRWPF.GUI.Views
{
    public partial class UC_OCR : UserControl
    {
        private Brush _prevShowParseAreaBackgroundBrush;
        private Brush _prevShowParseAreaBorderBrush;
        private Brush _showParseAreaHoverBrushBackground = (Brush)new BrushConverter().ConvertFrom("#BBAFBEC0");
        private Brush _showParseAreaHoverBrushBorder = (Brush)new BrushConverter().ConvertFrom("#5030CF");

        private Brush _prevHideParseAreaBackgroundBrush;
        private Brush _prevHideParseAreaBorderBrush;
        private Brush _hideParseAreaHoverBrushBackground = (Brush)new BrushConverter().ConvertFrom("#EFFEF0");
        private Brush _hideParseAreaHoverBrushBorder = (Brush)new BrushConverter().ConvertFrom("#60DF9A");

        VM_OCR _vm;
        W_TargetBoundingBox _targetBoundingBox;

        public UC_OCR()
        {
            _vm = VMLocator.GetViewModel<VM_OCR>();
            _targetBoundingBox = new W_TargetBoundingBox();
            _vm.TargetBoundingBox = _targetBoundingBox;

            this.DataContext = _vm;
            InitializeComponent();
        }

        private void ShowParseAreaButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border brd)
            {
                if (_prevShowParseAreaBackgroundBrush == null)
                    _prevShowParseAreaBackgroundBrush = brd.Background;

                if (_prevShowParseAreaBorderBrush == null)
                    _prevShowParseAreaBorderBrush = brd.BorderBrush;

                brd.Background = _showParseAreaHoverBrushBackground;
                brd.BorderBrush = _showParseAreaHoverBrushBorder;
            }
        }

        private void ShowParseAreaButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border brd)
            {
                brd.Background = _prevShowParseAreaBackgroundBrush;
                brd.BorderBrush = _prevShowParseAreaBorderBrush;

                _prevShowParseAreaBackgroundBrush = null;
                _prevShowParseAreaBorderBrush = null;
            }
        }
        private void HideParseAreaButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border brd)
            {
                if (_prevHideParseAreaBackgroundBrush == null)
                    _prevHideParseAreaBackgroundBrush = brd.Background;

                if (_prevHideParseAreaBorderBrush == null)
                    _prevHideParseAreaBorderBrush = brd.BorderBrush;

                brd.Background = _hideParseAreaHoverBrushBackground;
                brd.BorderBrush = _hideParseAreaHoverBrushBorder;
            }
        }

        private void HideParseAreaButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border brd)
            {
                brd.Background = _prevHideParseAreaBackgroundBrush;
                brd.BorderBrush = _prevHideParseAreaBorderBrush;
            }
        }

        private void ToggleParseArea_Click(object sender, MouseButtonEventArgs e)
        {
            _vm.CommandToggleParseAreaVisibility?.Execute(null);

            if (_vm.IsShowingParseArea)
                HideParseAreaButton_MouseEnter(brdBtnHideParseArea, e);
            else
                ShowParseAreaButton_MouseEnter(brdBtnShowParseArea, e);
        }

        private void imgRunning_MouseEnter(object sender, MouseEventArgs e)
        {
            GUIHelper.ElementMouseEnter(sender, 0.4);
        }

        private void imgRunning_MouseLeave(object sender, MouseEventArgs e)
        {
            GUIHelper.ElementMouseLeave(sender);
        }

        private void imgRunning_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch(_vm.RunningMode)
            {
                case 1: // off
                    _vm.CommandStartParser?.Execute(null);
                    break;
                case 2: // starting
                    break;
                case 3: // on
                    _vm.CommandStopParser?.Execute(null);
                    break;
                default:
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    break;
            }
        }
    }
}
