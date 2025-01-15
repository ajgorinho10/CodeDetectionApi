using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using static FindPatterns;
using System.Drawing;
using System.Collections.Concurrent;
using System.Diagnostics;
using KodQR.qr;
using ImageProcessor.Processors;

namespace KodQR.bar
{
    public class barDetection
    {
        public Image<Bgr, Byte> img;

        public barDetection(Image<Bgr, byte> img)
        {
            this.img = img;
        }

        public List<String> detectBAR()
        {
            int x = img.Width;
            int y = img.Height;
            double ratio = (double)this.img.Height / (double)this.img.Width;
            if(this.img.Width > 1200 || this.img.Height > 1200)
            {
                x = 1200;
                y = (int)(x*ratio);
                CvInvoke.Resize(this.img, this.img, new Size(x, y));
                //Console.WriteLine("ok");
            }


            //CvInvoke.GaussianBlur(this.img, this.img, new Size(1, 1), 10.0);

            CvInvoke.Normalize(this.img, this.img, 0,180, NormType.MinMax);

            FindBar fBar = new FindBar(this.img.Convert<Gray,Byte>(), this.img.Convert<Bgr, Byte>());
            List<Image<Gray, Byte>> barImages = fBar.find();

            CvInvoke.Resize(fBar.img_codes, fBar.img_codes, new Size(800, 600));
            //CvInvoke.Imshow("orginalBar", fBar.img_codes);
            //CvInvoke.WaitKey(0);
            int i = 1;
            List<String> msg = new List<String>();
            Parallel.ForEach(barImages, img => {
            
                //CvInvoke.Imshow($"img: {i}", img);
                //CvInvoke.WaitKey(0);
                projection p = new projection(img);
                p.Image_projection();

                if (p.barInTab != null)
                {
                    //CvInvoke.Imshow($"img1: {i}", img);
                    //CvInvoke.Imshow($"img2: {i}", p.imBar);
                    Decoding dec = new Decoding(p.barInTab, p.imBar, p.y_f);
                    String odp = dec.decode();
                    if (odp != "Brak")
                    {
                        msg.Add(odp);
                    }
                    CvInvoke.WaitKey(0);
                }
                i++;

            });

            return msg;
        }
    }
}
