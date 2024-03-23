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
    public partial class W_InputText : Window
    {
        WindowInputTextOptions _options;

        public string PrimaryInputText { get; set; } = "";
        public string SecondaryInputText { get; set; } = "";

        public bool DidContinue { get; set; } = false;

        public W_InputText(WindowInputTextOptions options)
        {
            _options = options;

            InitializeComponent();
        }

        void FocusOnInput()
        {
            if (_options != null)
            {
                if (_options.ModePasswordInput)
                {
                    txbPass.Focus();
                }
                else if (_options.ModeDoubleInput)
                {
                    FocusTextbox(txbDoubleInput1);
                }
                else
                {
                    FocusTextbox(txbInput);
                }
            }
        }

        private void FocusTextbox(TextBox box)
        {
            box.Focus();
            box.ScrollToEnd();
            box.CaretIndex = int.MaxValue;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_options.Title))
                this.Title = _options.Title;

            txtExplanation.Text = _options.Explanation;
            btnContinue.Content = _options.ButtonContinueText;
            btnClose.Content = _options.ButtonCloseText;

            if (_options.ModePasswordInput)
            {
                txbInput.Visibility = Visibility.Collapsed;
                txbPass.Visibility = Visibility.Visible;

                txbInput.IsEnabled = false;
                txbPass.IsEnabled = true;
            }
            else if (_options.ModeDoubleInput)
            {
                txbInput.Visibility = Visibility.Collapsed;
                stkDoubleInput.Visibility = Visibility.Visible;

                if (_options.IsTextArea)
                {
                    MakeTextarea(txbDoubleInput1);
                    MakeTextarea(txbDoubleInput2);
                }
            }
            else // Normal 1 Input Mode
            {
                if (_options.IsTextArea)
                    MakeTextarea(txbInput);
            }


            if (_options.MaximumLength > 0)
            {
                txbInput.MaxLength = _options.MaximumLength;
                txbPass.MaxLength = _options.MaximumLength;
                txbDoubleInput1.MaxLength = _options.MaximumLength;
                txbDoubleInput2.MaxLength = _options.MaximumLength;
            }

            if (!string.IsNullOrEmpty(_options.Watermark1))
            {
                txbInput.SetValue(TextBoxHelper.WatermarkProperty, _options.Watermark1);
                txbDoubleInput1.SetValue(TextBoxHelper.WatermarkProperty, _options.Watermark1);
            }
            if (!string.IsNullOrEmpty(_options.Watermark2))
                txbDoubleInput2.SetValue(TextBoxHelper.WatermarkProperty, _options.Watermark2);

            if (!string.IsNullOrEmpty(_options.DefaultText1))
            {
                txbInput.Text = _options.DefaultText1;
                txbDoubleInput1.Text = _options.DefaultText1;
            }

            if (!string.IsNullOrEmpty(_options.DefaultText2))
                txbDoubleInput2.SetValue(TextBoxHelper.WatermarkProperty, _options.Watermark2);

            FocusOnInput();
        }

        private void MakeTextarea(TextBox box)
        {
            box.AcceptsReturn = true;
            box.AcceptsTab = true;
            box.TextWrapping = TextWrapping.Wrap;
            box.Height = _options.TextAreaHeight;
            box.VerticalContentAlignment = VerticalAlignment.Top;
            box.HorizontalContentAlignment = HorizontalAlignment.Left;
            box.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private string GetErrorTextForInput(string input)
        {
            string error = "";

            if (input.Length < _options.MinimumLength)
                error = "You should write atleast " + _options.MinimumLength + " characters.";
            else if (_options.MaximumLength > 0 && input.Length > _options.MaximumLength)
                error = "Your input should not be more than " + _options.MaximumLength + " characters.";

            if (string.IsNullOrEmpty(error) && _options.ModePasswordInput)
                if (!string.IsNullOrEmpty(_options.ModePasswordPassword))
                    if (!_options.ModePasswordPassword.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                        error = "Invalid Password!";

            return error;
        }
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            DidContinue = false;

            if (btnContinue.Equals(sender))
            {
                txtError.Text = "";

                try
                {
                    string error = "";
                    string input1 = "", input2 = "";

                    if (_options.ModeDoubleInput)
                    {
                        input1 = txbDoubleInput1.Text?.Trim() ?? "";
                        input2 = txbDoubleInput2.Text?.Trim() ?? "";

                        error = GetErrorTextForInput(input1);
                        if (string.IsNullOrEmpty(error))
                            error = GetErrorTextForInput(input2);
                    }
                    else
                    {
                        if (_options.ModePasswordInput)
                            input1 = txbPass.Password.Trim();
                        else
                            input1 = txbInput.Text.Trim();

                        error = GetErrorTextForInput(input1);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        txtError.Text = error;
                        return;
                    }
                    else
                    {
                        DidContinue = true;
                        PrimaryInputText = input1;
                        SecondaryInputText = input2;
                        this.Close();

                    }
                }
                catch (Exception)
                {
                    txtError.Text = "Couldn't parse your input - it must be a valid date";
                    return;
                }

                if (!DidContinue)
                    FocusOnInput();
            }
            else if (btnClose.Equals(sender))
            {
                this.Close();
            }
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
