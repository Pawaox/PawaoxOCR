using PawaoxOCRWPF.GUI.GUIModels;
using PawaoxOCRWPF.GUI.ViewModels;
using PawaoxOCRWPF.GUI.Views.Windows;
using PawaoxOCRWPF.Helpers;
using PawaoxOCRWPF.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PawaoxOCRWPF.GUI.Views
{
    public partial class UC_System : UserControl
    {
        private readonly UserControlType _currentType = UserControlType.NONE;

        private VM_System _vm;

        private bool _canToggleMenu = true;
        private Border _prevSelected;

        public UC_System()
        {
            _vm = VMLocator.GetViewModel<VM_System>();
            DataContext = _vm;
            InitializeComponent();

            MessageBroker.Register<MSG_ChangeUserControl>(OnChangeUserControl);
            MessageBroker.Register<MSG_DisplayMessage>(OnDisplayMessage);

            MessageBroker.Register<MSG_DialogMessage>(OpenDialogMessage);
            MessageBroker.Register<MSG_DialogWarning>(OpenDialogWarning);
            MessageBroker.Register<MSG_DialogError>(OpenDialogError);
            MessageBroker.Register<MSG_DialogInputText>(OpenInputText);
        }
        private void OpenDialogMessage(MSG_DialogMessage msg)
        {
            if (msg?.Options != null)
            {
                W_Message win = new W_Message(msg.Options);
                win.ShowDialog();

                msg.OnClose?.Invoke();
            }
        }

        private void OpenDialogWarning(MSG_DialogWarning msg)
        {
            if (msg?.Options != null)
            {
                W_MessageWarning win = new W_MessageWarning(msg.Options);
                win.ShowDialog();

                msg.OnClose?.Invoke(win.DidContinue);
            }
        }

        private void OpenDialogError(MSG_DialogError msg)
        {
            if (msg?.Options != null)
            {
                W_MessageError win = new W_MessageError(msg.Options);
                win.ShowDialog();

                msg.OnClose?.Invoke();
            }
        }
        private void OpenInputText(MSG_DialogInputText msg)
        {
            if (msg?.Options != null)
            {
                W_InputText win = new W_InputText(msg.Options);
                win.ShowDialog();

                msg.OnClose?.Invoke(new MSG_DialogInputText.Response(win.DidContinue, win.PrimaryInputText, win.SecondaryInputText));
            }
        }

        private void OnDisplayMessage(MSG_DisplayMessage e)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    toast.ShowToasts(e.IsError, e.Message);
                }
                catch { }
            });
        }

        private void OnChangeUserControl(MSG_ChangeUserControl e)
        {
            if (_currentType == e.Type)
                return;

            switch (e.Type)
            {
                case UserControlType.MAIN:
                    ucRoot.Content = new UC_Main();
                    break;
                case UserControlType.OCR:
                    ucRoot.Content = new UC_OCR();
                    break;

                default:
                    if (Debugger.IsAttached)
                        Debugger.Break();

                    ucRoot.Content = new UC_Main();
                    break;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ucRoot.Content = new UC_Main();
        }

        private void MenuToggler_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_canToggleMenu)
                return;

            _vm.CommandToggleMenu?.Execute();

            if (_vm.IsMenuOpen)
            {
                int time = 625;
                //Open Menu
                DoubleAnimation animFadeIn = new DoubleAnimation(grdInnerMenu.Opacity, 1, TimeSpan.FromMilliseconds(time));
                DoubleAnimation animExpand = new DoubleAnimation(brdMenu.ActualWidth, clmMenu.MaxWidth, TimeSpan.FromMilliseconds(time));
                animExpand.Completed += (s, args) =>
                {
                    _canToggleMenu = true;
                    grdInnerMenu.IsEnabled = true;
                };

                _canToggleMenu = false;
                brdMenu.BeginAnimation(WidthProperty, animExpand);
                grdInnerMenu.BeginAnimation(OpacityProperty, animFadeIn);
            }
            else
            {
                int time = 525;
                //Close Menu
                DoubleAnimation animFadeOut = new DoubleAnimation(grdInnerMenu.Opacity, 0, TimeSpan.FromMilliseconds(time));
                DoubleAnimation animCollapse = new DoubleAnimation(brdMenu.ActualWidth, clmMenu.MinWidth, TimeSpan.FromMilliseconds(time));

                animCollapse.Completed += (s, args) =>
                {
                    _canToggleMenu = true;
                    grdInnerMenu.IsEnabled = false;
                };

                _canToggleMenu = false;
                brdMenu.BeginAnimation(WidthProperty, animCollapse);
                grdInnerMenu.BeginAnimation(OpacityProperty, animFadeOut);
            }
        }

        private void MenuToggler_MouseEnter(object sender, MouseEventArgs e)
        {
            grdMenuHoverOverlay.Opacity = 0.05;
            rectLineA.Height = 2;
            rectLineB.Height = 2;
            rectLineC.Height = 2;
        }

        private void MenuToggler_MouseLeave(object sender, MouseEventArgs e)
        {
            grdMenuHoverOverlay.Opacity = 0;
            rectLineA.Height = 1;
            rectLineB.Height = 1;
            rectLineC.Height = 1;
        }

        private void MenuTab_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border)
            {
                Border brd = (Border)sender;

                if (brd.Child is ContentControl)
                {
                    ContentControl inner = (ContentControl)brd.Child;
                    if (inner.Tag != null && inner.Tag is TabModel)
                    {
                        TabModel tab = (TabModel)inner.Tag;

                        if (tab.IsSelected)
                            return;

                        brd.Opacity = 0.5;

                        brd.Tag = brd.BorderBrush;
                        brd.BorderBrush = Brushes.CornflowerBlue;
                    }
                }
            }
        }
        private void MenuTab_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border)
            {
                Border brd = (Border)sender;

                if (brd.Child is ContentControl)
                {
                    ContentControl inner = (ContentControl)brd.Child;
                    if (inner.Tag != null && inner.Tag is TabModel)
                    {
                        TabModel tab = (TabModel)inner.Tag;

                        if (tab.IsSelected)
                            return;

                        brd.Opacity = 1;

                        if (brd.Tag is Brush brush)
                            brd.BorderBrush = brush;
                    }
                }
            }
        }

        private void MenuTab_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border)
            {
                Border brd = (Border)sender;
                if (brd.Child is ContentControl)
                {
                    ContentControl inner = (ContentControl)brd.Child;
                    if (inner.Tag != null && inner.Tag is TabModel)
                    {
                        TabModel tab = (TabModel)inner.Tag;
                        _vm.CommandSelectTab?.Execute(tab);

                        if (_prevSelected != null)
                            MenuTab_MouseLeave(_prevSelected, e);
                        _prevSelected = brd;
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.CommandMenuSearch?.Execute();
        }

        private void ToggleTabVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mItem)
            {
                if (mItem.Tag is TabModel tabModel)
                {
                    _vm.CommandToggleTabVisibility?.Execute(tabModel);
                }
            }

        }

        private void TextBoxSearch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement fme)
                ccLogo.Width = fme.Width;
        }
    }
}
