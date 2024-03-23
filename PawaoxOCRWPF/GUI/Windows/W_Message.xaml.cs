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
    public partial class W_Message : MetroWindow
    {
        WindowMessageOptions _options;

        public W_Message(WindowMessageOptions options)
        {
            _options = options;

            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_options.Title))
                this.Title = _options.Title;

            txtMessage.Text = _options.Explanation;
            btnClose.Content = _options.ButtonCloseText;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (btnClose.Equals(sender))
                this.Close();
        }
    }
}
