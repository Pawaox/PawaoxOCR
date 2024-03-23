using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PawaoxOCRWPF.Helpers
{
    public static class GUIHelper
    {
        public static void ElementMouseEnter(object obj, double opacity = 0.5)
        {
            if (obj is FrameworkElement)
            {
                FrameworkElement fme = (FrameworkElement)obj;

                fme.Opacity = opacity;
            }
        }
        public static void ElementMouseLeave(object obj, double opacity = 1)
        {
            if (obj is FrameworkElement)
            {
                FrameworkElement fme = (FrameworkElement)obj;

                fme.Opacity = opacity;
            }
        }

        public static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg); // Change ImageFormat as per your requirement
                return stream.ToArray();
            }
        }
    }
}
