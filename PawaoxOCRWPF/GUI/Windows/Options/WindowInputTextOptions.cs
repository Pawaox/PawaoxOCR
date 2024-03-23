using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.GUI.Views.Windows.Options
{
    public class WindowInputTextOptions : WindowOptionBase
    {
        public WindowInputTextOptions()
        {
            Title = "Input Text";
            Explanation = "";
            ButtonContinueText = "Continue";
            ModePasswordPassword = "";
            DefaultText1 = "";

            MinimumLength = 2;
            MaximumLength = 0;

            TextAreaHeight = 90;

            IsTextArea = true;
        }

        public bool IsTextArea { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }

        public int TextAreaHeight { get; set; }

        public string DefaultText1 { get; set; } = "";

        /// <summary>
        /// Default Text for the second input if ModeDoubleInput is true
        /// </summary>
        public string DefaultText2 { get; set; } = "";


        public string Watermark1 { get; set; } = "";
        /// <summary>
        /// Watermark for the second input if ModeDoubleInput is true
        /// </summary>
        public string Watermark2 { get; set; } = "";


        public string ModePasswordPassword { get; set; } = "";

        public bool ModePasswordInput { get; set; } = false;
        public bool ModeDoubleInput { get; set; } = false;

    }
}
