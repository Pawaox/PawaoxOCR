using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PawaoxOCRWPF.Helpers
{
    public class BitmapImageSettings
    {
        public int? Brightness { get; set; } = null;
        public int? Contrast { get; set; } = null;

        public bool InvertColorsPreProcess { get; set; } = false;
        public bool InvertColorsPostProcess { get; set; } = false;

        /// <summary>
        /// <= 0 or exactly 100% = No change
        /// </summary>
        public double? ResizePercentSize { get; set; } = null;

    }
}
