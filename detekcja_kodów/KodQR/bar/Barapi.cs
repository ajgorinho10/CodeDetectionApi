using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using static FindPatterns;
using System.Drawing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using KodQR.bar;
using System.Collections.Generic;

namespace KodQR.bar
{
    public class Barapi
    {
        public static string ConvertBase64ToImage(string base64String, string outputPath)
        {
            try
            {
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                byte[] imageBytes = Convert.FromBase64String(base64String);

                File.WriteAllBytes(outputPath, imageBytes);

                //Console.WriteLine($"Image successfully saved to {outputPath}");
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting Base64 to image: {ex.Message}");
                return "Error";
            }
        }
        public static List<String> Detection(string img)
        {
            string filePath = ConvertBase64ToImage(img, "input_image.png");

            Mat image = CvInvoke.Imread(filePath, ImreadModes.Color | ImreadModes.AnyDepth);
            barDetection bar = new barDetection(image.ToImage<Bgr, byte>());
            List<String> bar_info = bar.detectBAR();
            return bar_info;
        }
    }
}
