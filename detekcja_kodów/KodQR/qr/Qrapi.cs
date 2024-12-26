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

namespace KodQR.qr
{
    public class Qrapi
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

        public static List<string> DetectionQrBar(string img)
        {
            string filePath = ConvertBase64ToImage(img, "input_image.png");
            if(filePath == "Error")
            {
                return new List<string>();
            }

            List<string> list = new List<string>();
            void QR()
            {
                Image<Gray, byte> img = Binarization.Binarize(filePath);
                qrDetection qr = new qrDetection();
                List<Tuple<Punkt, Punkt, Punkt, Punkt, string>> qr_info = qr.qrDetect(img);

                foreach (var q in qr_info)
                {
                    if (q.Item5 == "")
                    {
                        continue;
                    }
                    list.Add(q.Item5);
                    Console.WriteLine("QR:" + q.Item5);
                }
            }

            void BAR()
            {
                Mat image = CvInvoke.Imread(filePath, ImreadModes.Color | ImreadModes.AnyDepth);
                barDetection bar = new barDetection(image.ToImage<Bgr, byte>());
                List<String> bar_info = bar.detectBAR();

                foreach (var q in bar_info)
                {
                    list.Add(q);
                    Console.WriteLine("EAN-13:" + q);
                }
            }

            Thread thread_qr = new Thread(new ThreadStart(QR));
            thread_qr.Name = "QR";
            thread_qr.Start();

            Thread thread_bar = new Thread(new ThreadStart(BAR));
            thread_bar.Name = "BAR";
            thread_bar.Start();

            thread_qr.Join();
            thread_bar.Join();

            return list;
        }

        public static List<String> Detection(string img1)
        {
            string filePath = ConvertBase64ToImage(img1, "input_image.png");
            List<string> list = new List<string>();
            Image<Gray, byte> img = Binarization.Binarize(filePath);
            qrDetection qr = new qrDetection();
            List<Tuple<Punkt, Punkt, Punkt, Punkt, string>> qr_info = qr.qrDetect(img);

            foreach (var q in qr_info)
            {
                if (q.Item5 == "")
                {
                    continue;
                }
                list.Add(q.Item5);
                Console.WriteLine("QR:" + q.Item5);
            }

            return list;
        }
    }
}
