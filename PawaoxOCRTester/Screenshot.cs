using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRTester
{
    public static class Screenshot
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
