using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using PipelinePattern.Entity;

namespace PipelinePattern
{
    public static class ImageLogic
    {
        private static int ammount = 0;
        public static ImageInfo LoadImage(string fname, string sourceDir, int count)
        {
            ImageInfo info = null;
            //Bitmap bitmap = new Bitmap(Path.Combine(sourceDir, fname));
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(sourceDir + fname, System.IO.FileMode.Open))
            {
                Image image = Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);

            }
            try
            {
                bitmap.Tag = fname;

                info = new ImageInfo(count, fname, bitmap);
                bitmap = null;
            }
            finally
            {
                bitmap?.Dispose();
            }
            return info;
        }

        public static void ScaleImage(ImageInfo info)
        {
            var orig = info.OriginalImage;
            info.OriginalImage = null;
            const int scale = 200;
            var isLandscape = (orig.Width > orig.Height);
            var newWidth = isLandscape ? scale : scale * orig.Width / orig.Height;
            var newHeight = !isLandscape ? scale : scale * orig.Height / orig.Width;
            Bitmap bitmap = new Bitmap(orig, newWidth, newHeight);
                try
                {
                    bitmap.Tag = orig.Tag;
                    info.ThumbnailImage = bitmap;
                    bitmap = null;
                }
                finally
                {
                    bitmap?.Dispose();
                    orig.Dispose();
                }
        }

        public static void FilterImage(ImageInfo info)
        {
            var sc = info.ThumbnailImage;
            info.ThumbnailImage = null;
            int rgb;
            Color c;

            for (int y = 0; y < sc.Height; y++)
            for (int x = 0; x < sc.Width; x++)
            {
                c = sc.GetPixel(x, y);
                rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                sc.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
            }

            try
            {
                sc.Tag = sc.Tag;
                info.FilteredImage = sc;

                sc = null;
            }
            finally
            {
                sc?.Dispose();
            }
        }

        public static void SaveImage(ImageInfo info, int count)
        {
            info.ImageCount = count;
            info.FilteredImage.Save("C:\\Users\\Kalpens\\source\\repos\\PipelinePattern\\PipelinePattern\\ImagesToSave\\img" + ammount + ".jpg", ImageFormat.Jpeg);
            ammount++;
            //info.FilteredImage.Save("C:\\Users\\Kalpens\\source\\repos\\PipelinePattern\\PipelinePattern\\ImagesToSave\\img" + count +".jpg", ImageFormat.Jpeg);
        }
    }
}
