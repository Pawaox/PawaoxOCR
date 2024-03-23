using MahApps.Metro.Controls;
using PawaoxOCRWPF.GUI.Views.Windows.Options;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PawaoxOCRWPF.GUI.Views.Windows
{
    public partial class W_MessageError : MetroWindow
    {
        WindowMessageErrorOptions _options;

        public W_MessageError(WindowMessageErrorOptions options)
        {
            _options = options;

            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_options.Title))
                this.Title = _options.Title;

            txtMessage.Text = _options.Explanation;
            btnContinue.Content = _options.ButtonContinueText;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (btnContinue.Equals(sender))
                this.Close();
        }
    }
}
