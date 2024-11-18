using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCRWPF.Helpers
{
    public static class BitmapImageHelper
    {
        public static Bitmap ImplementSettings(Bitmap sourceBitmap, BitmapImageSettings settings)
        {
            Bitmap processedBitmap = new Bitmap(sourceBitmap);

            // Invert colors before processing, if required
            if (settings.InvertColorsPreProcess)
                InvertColors(ref processedBitmap);

            // Adjust brightness and contrast if specified
            if (settings.Brightness.HasValue || settings.Contrast.HasValue)
                AdjustBrightnessContrast(ref processedBitmap, settings.Brightness, settings.Contrast);

            // Invert colors after processing, if required
            if (settings.InvertColorsPostProcess)
                InvertColors(ref processedBitmap);

            if (settings.ResizePercentSize.HasValue)
            {
                double resizVal = settings.ResizePercentSize.Value;

                if (resizVal > 0.0 && resizVal != 100.0)
                    processedBitmap = CreateResizedBitmap(processedBitmap, resizVal);
            }

            return processedBitmap;
        }

        public static void AdjustBrightnessContrast(ref Bitmap bitmap, int? brightness, int? contrast)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color originalColor = bitmap.GetPixel(x, y);

                    float r = originalColor.R;
                    float g = originalColor.G;
                    float b = originalColor.B;
                    // Adjust brightness
                    if (brightness.HasValue)
                    {
                        float bValue = brightness.Value;
                        r += bValue;
                        g += bValue;
                        b += bValue;
                    }

                    // Adjust contrast
                    if (contrast.HasValue)
                    {
                        float contrastLevel = (100.0f + contrast.Value) / 100.0f;
                        contrastLevel *= contrastLevel;

                        r = ((((r / 255.0f) - 0.5f) * contrastLevel) + 0.5f) * 255.0f;
                        g = ((((g / 255.0f) - 0.5f) * contrastLevel) + 0.5f) * 255.0f;
                        b = ((((b / 255.0f) - 0.5f) * contrastLevel) + 0.5f) * 255.0f;
                    }


                    // Clamp values
                    r = Math.Min(Math.Max(r, 0), 255);
                    g = Math.Min(Math.Max(g, 0), 255);
                    b = Math.Min(Math.Max(b, 0), 255);

                    bitmap.SetPixel(x, y, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }
        }

        public static void InvertColors(ref Bitmap bitmap)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color originalColor = bitmap.GetPixel(x, y);

                    int r = 255 - originalColor.R;
                    int g = 255 - originalColor.G;
                    int b = 255 - originalColor.B;

                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }

        public static Bitmap CreateResizedBitmap(Bitmap bitmap, double percentage)
        {
            // Calculate new dimensions
            int newWidth = (int)(bitmap.Width * (percentage / 100));
            int newHeight = (int)(bitmap.Height * (percentage / 100));

            // Create a new bitmap with the new dimensions
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);

            // Use graphics to draw the resized image
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            }

            return resizedBitmap;
        }
    }
}
