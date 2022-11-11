using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tothm_szak
{
    /// <summary>
    /// Static utility class for commonly used methods
    /// Contains conversion methods
    /// </summary>
    public static class ImageProcUtility
    {
        // currently unused
        public static Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);
            }
            return bitmap;
        }


        /// <summary>
        /// Converts Bitmap type to BitmapImage
        /// </summary>
        /// <param name="inputBitmap">
        /// Input image as type Bitmap
        /// </param>
        /// <returns>
        /// Output image as type BitmapImage
        /// </returns>
        public static BitmapImage Bitmap2BitmapImage(Bitmap inputBitmap)
        {
            using (var memory = new MemoryStream())
            {
                inputBitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        /// <summary>
        /// Converts BitmapImage type to Bitmap
        /// </summary>
        /// <param name="bitmapImage">
        /// Input image as type BitmapImage
        /// </param>
        /// <returns>
        /// Output image as type Bitmap
        /// </returns>
        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            //BitmapImage bitmapImageT = new BitmapImage(new Uri(images[0], UriKind.Absolute));
            //use Mat to load the img

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
