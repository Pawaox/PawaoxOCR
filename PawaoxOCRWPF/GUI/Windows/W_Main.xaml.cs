using MahApps.Metro.Controls;
using PawaoxOCRWPF.GUI.Views;
using System;
using System.Collections.Generic;
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

namespace PawaoxOCRWPF
{
    public partial class W_Main : MetroWindow
    {
        public W_Main()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            root.Content = new UC_System();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string prefix = $"({e.NewSize.Width},{e.NewSize.Height})";
            this.Title = prefix + " Paw OCR";
        }
    }
}
