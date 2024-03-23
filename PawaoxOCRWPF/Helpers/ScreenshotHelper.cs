using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Helpers
{
    public static class ScreenshotHelper
    {
        public static Bitmap CaptureScreenRegion(int left, int top, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(left, top, 0, 0, bitmap.Size);
            }

            return bitmap;
        }
    }
}
