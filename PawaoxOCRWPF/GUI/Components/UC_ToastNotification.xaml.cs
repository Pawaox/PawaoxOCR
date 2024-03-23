using PawaoxOCRWPF.GUI.GUIModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace PawaoxOCRWPF.GUI.Components
{
    public partial class UC_ToastNotification : UserControl, UIToaster
    {
        bool _toastsVisible;

        DispatcherTimer timer;
        DoubleAnimation animTo0, animTo1, animWidthShow, animWidthHide;
        BooleanAnimationUsingKeyFrames animToTrue, animToFalse;

        public UC_ToastNotification()
        {
            animWidthShow = new DoubleAnimation(0, 270, new Duration(TimeSpan.FromMilliseconds(400)));
            animWidthHide = new DoubleAnimation(270, 0, new Duration(TimeSpan.FromMilliseconds(400)));

            animTo1 = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(550)));
            animTo0 = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(550)));

            animToTrue = new BooleanAnimationUsingKeyFrames();
            animToTrue.Duration = new Duration(TimeSpan.FromMilliseconds(550));
            animToTrue.KeyFrames.Add(new DiscreteBooleanKeyFrame(true));

            animToFalse = new BooleanAnimationUsingKeyFrames();
            animToFalse.Duration = new Duration(TimeSpan.FromMilliseconds(550));
            animToFalse.KeyFrames.Add(new DiscreteBooleanKeyFrame(false));

            animTo0.Completed += (a, b) => { Dispatcher.Invoke(() => { grdRoot.Visibility = Visibility.Collapsed; }); };

            timer = new DispatcherTimer();
            timer.Tick += (s, e) =>
            {
                HideToasts();
            };

            InitializeComponent();
        }


        public void ShowToasts(bool isError, string msg)
        {
            Dispatcher.Invoke(() =>
            {
                grdRoot.Visibility = Visibility.Visible;

                timer.Stop();

                Brush color = isError ? Brushes.Red : Brushes.Black;

                toastMiniText.Foreground = color;
                toastBorder.BorderBrush = color;
                toastBorderText.Foreground = Brushes.White;

                toastBorderText.Text = msg;

                if (isError)
                {
                    timer.Interval = TimeSpan.FromMilliseconds(15000);
                    toastMiniText.Text = "X";

                    toastBorder.BeginAnimation(IsEnabledProperty, animToTrue);
                    toastBorder.BeginAnimation(WidthProperty, animWidthShow);
                    toastBorder.BeginAnimation(OpacityProperty, animTo1);
                }
                else
                {
                    toastBorderText.Text = "";
                    toastBorder.BeginAnimation(IsEnabledProperty, animToFalse);
                    toastBorder.BeginAnimation(WidthProperty, animWidthHide);
                    toastBorder.BeginAnimation(OpacityProperty, animTo0);

                    timer.Interval = TimeSpan.FromMilliseconds(4000);
                    toastMiniText.Text = "✓";
                }

                toastMini.BeginAnimation(IsEnabledProperty, animToTrue);
                toastMini.BeginAnimation(OpacityProperty, animTo1);

                timer.Start();

                if (!_toastsVisible)
                    _toastsVisible = true;
            });
        }

        private void toasts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Clipboard.SetText(toastBorderText.Text);
            }
            HideToasts();
        }

        void HideToasts()
        {
            if (_toastsVisible)
            {
                timer.Stop();

                toastBorderText.Text = "";

                toastMini.BeginAnimation(IsEnabledProperty, animToFalse);
                toastMini.BeginAnimation(OpacityProperty, animTo0);

                toastBorder.BeginAnimation(IsEnabledProperty, animToFalse);
                toastBorder.BeginAnimation(WidthProperty, animWidthHide);
                toastBorder.BeginAnimation(OpacityProperty, animTo0);

                _toastsVisible = false;
            }
        }

    }
}
