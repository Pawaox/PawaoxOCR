using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawaoxOCR;

namespace PawaoxOCRTester
{
    internal class Program
    {
        PawaoxTesseractEngine _ocr;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ENTER to start");
                Console.ReadLine();

                using (var ocr = new PawaoxTesseractEngine("C://Temp//tesseractData//", PawaoxOCREngineLanguage.JAPANESE))
                {
                    int tries = 5;
                    int currentTry = 0;
                    ProcessResult bestResult = null;
                    while (currentTry < tries)
                    {
                        Bitmap image = Screenshot.CaptureScreenRegion(0, 0, 1920, 1080);

                        byte[] bytes = ImageToByteArray(image);

                        if (bytes != null && bytes.Length > 0)
                        {
                            ProcessResult result = ocr.Process(bytes);

                            if (result != null && result.Success)
                            {
                                Console.WriteLine($"Confidence For #{(currentTry + 1)}: {result.MeanConfidence}");

                                if (bestResult == null)
                                    bestResult = result;
                                else if (result.MeanConfidence > bestResult.MeanConfidence)
                                    bestResult = result;
                            }
                        }

                        currentTry++;
                    }

                    if (bestResult != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Confidence: " + bestResult.MeanConfidence);

                        List<string> lines = bestResult.GetAllLines();

                        foreach (string line in lines)
                            Console.WriteLine(line);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("<<<<<FINISHED>>>>>");
            Console.ReadLine();
        }

        static byte[] ImageToByteArray(Image image)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg); // Change ImageFormat as per your requirement
                return stream.ToArray();
            }
        }
    }
}
