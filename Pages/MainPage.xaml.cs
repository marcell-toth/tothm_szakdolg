﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Media;
using System.Drawing.Imaging;
using OpenCvSharp.XImgProc.Segmentation;
using System.Collections;
using OpenCvSharp.ML;
//using System.Drawing;

namespace tothm_szak.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        BitmapImage biT = new BitmapImage();
        BitmapImage biTs = new BitmapImage();
        public MainPage()
        {
            InitializeComponent();
        }

        public int currentImage { get; set; } = 0;
        public int numOfImages { get; set; } = 0;

        List<string> images = new List<string>();

        private bool getImages(string folderPath)
        {
            List<String> allowedExtensions = new List<String>();
            allowedExtensions = allowedFileTypes(allowedExtensions);
            if (ConfigClass.folderPath != "" && allowedExtensions.Count() > 0)
            {
                var imagesInDirectory = Directory
                .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(s => allowedExtensions.Contains(System.IO.Path.GetExtension(s)
                .TrimStart('.').ToLowerInvariant()));

                images = imagesInDirectory.ToList();
                numOfImages = imagesInDirectory.Count();
                tbNumberOfImages.Text = "Képek száma: " + numOfImages.ToString();
                if (numOfImages == 0) return false;
                return true;
            } else
            {
                tbImgCounter.Text = "0";
                btPrevImg.IsEnabled = false;
                btNextImg.IsEnabled = false;
                return false;
            }
        }
        private List<string> allowedFileTypes(List<string> allowedExtensions)
        {
            //TODO FIX SELECTION
            if (ConfigClass.isAllowedPng) { allowedExtensions.Add("png"); } 
            if (ConfigClass.isAllowedJpg) { allowedExtensions.Add("jpg"); }
            if (ConfigClass.isAllowedJpeg) { allowedExtensions.Add("jpeg"); }
            return allowedExtensions;
        }
        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentImage = 0;
            numOfImages = 0;
            if (ConfigClass.folderPath != "")
            {
                bool pathAndFileCheck = getImages(ConfigClass.folderPath);
                if (pathAndFileCheck)
                {
                    loadImage(0);
                    tbImgCounter.Text = "1";
                    btPrevImg.IsEnabled = true;
                    btNextImg.IsEnabled = true;
                    generateButtons(numOfImages);
                } else
                {
                    tbImgCounter.Text = "0";
                    btPrevImg.IsEnabled = false;
                    btNextImg.IsEnabled = false;
                }
            }
        }
        private void generateButtons(int buttonCount)
        {
            //10 -> 2 in sort
            int count = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (count != numOfImages) 
                    {  
                        System.Windows.Controls.Image imgSelect = new System.Windows.Controls.Image();
                        Grid.SetColumn(imgSelect, i);
                        Grid.SetRow(imgSelect, row);

                        imgSelect.HorizontalAlignment = HorizontalAlignment.Stretch;
                        imgSelect.Height = imgSelect.Width;
                        imgSelect.Stretch = Stretch.Uniform;
                        imgSelect.StretchDirection = StretchDirection.Both;


                        BitmapImage bimage = new BitmapImage();
                        bimage.BeginInit();
                        bimage.UriSource = new Uri(images[count], UriKind.Absolute);
                        bimage.EndInit();
                        imgSelect.Source = bimage;

                        InnerGrid.Children.Add(imgSelect);
                        count++;
                    }
                }
            }
        }
        private void loadImage(int num)
        {
            //Mat src = Cv2.ImRead(images[num], ImreadModes.Grayscale);
            Mat src = new Mat(images[num], ImreadModes.Unchanged);
            Mat srcGray = new Mat(images[num], ImreadModes.Grayscale);

            Bitmap bT = BitmapConverter.ToBitmap(src);

            biT = Bitmap2BitmapImage(bT);



            Mat processedImage = selectProcess(src, srcGray);

            Bitmap bTs = BitmapConverter.ToBitmap(processedImage);
            biTs = Bitmap2BitmapImage(bTs);

            loadImageOnly(biT, biTs);
        }
        private void loadImageOnly(BitmapImage imageBase, BitmapImage imageProc)
        {
            if (cbProcessed.IsChecked == true)
            {
                testImg.Source = biTs;
            }
            else
            {
                testImg.Source = biT;
            }
        }
        private void loadImageNum(int dir)
        {
            if (dir == 0) 
            {
                if (currentImage != 0) { currentImage--; } else
                {
                    currentImage = numOfImages - 1;
                }
                
                currentImage = currentImage % numOfImages;
                tbImgCounter.Text = (currentImage + 1).ToString();
            }

            if (dir == 1)
            {
                currentImage++;
                currentImage = currentImage % numOfImages;
                tbImgCounter.Text = (currentImage + 1).ToString();
            }
            loadImage(currentImage);
        }
        
        private Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);

            }
            return bitmap;
        }
        private BitmapImage Bitmap2BitmapImage(Bitmap inputBitmap)
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
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            //BitmapImage bitmapImageT = new BitmapImage(new Uri(images[0], UriKind.Absolute));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        private void btPrevImg_Click(object sender, RoutedEventArgs e)
        {
            //back one
            loadImageNum(0);
        }
        private void btNextImg_Click(object sender, RoutedEventArgs e)
        {
            //forward one
            loadImageNum(1);
        }
        
        private Mat selectProcess(Mat src, Mat srcGray)
        {
            Mat processedImage = new Mat();
            switch (ConfigClass.activeProcessMode)
            {
                case ConfigClass.processMode.None:
                    {
                        src.CopyTo(processedImage);
                        return processedImage;
                    }
                case ConfigClass.processMode.Grayscale:
                    {
                        srcGray.CopyTo(processedImage);
                        return processedImage;
                    }
                case ConfigClass.processMode.Laplace:
                    {
                        processedImage =  gradLaplacian(srcGray);
                        return processedImage;
                    }
                case ConfigClass.processMode.SelectiveSearch:
                    {
                        processedImage = searchSegment(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.Contour:
                    {
                        processedImage = findCont(srcGray);
                        return processedImage;
                    }
                case ConfigClass.processMode.SimpleTreshold:
                    {
                        processedImage = simpleTreshold(srcGray);
                        return processedImage;
                    }

                default:
                    { 
                        return src;
                    }
            }
        }
        private Mat searchSegment(Mat src)
        {
            var ss = SelectiveSearchSegmentation.Create();
            ss.SetBaseImage(src);

            ss.SwitchToSelectiveSearchFast(300, 500, 0.8F);
            //List<OpenCvSharp.Rect> rl = new List<OpenCvSharp.Rect>();
            //rl.ToArray();

            OpenCvSharp.Rect[] rl;
            ss.Process(out rl);
            Trace.WriteLine(rl.Length);

            Scalar sc = new Scalar(0,0,255);
            foreach (OpenCvSharp.Rect r in rl)
            {
                Cv2.Rectangle(src, r.TopLeft, r.BottomRight, sc, 1, LineTypes.Link4, 0);
            }
            return src;
        }
        private Mat findCont(Mat src)
        {
            Mat dst = src.Clone();
            OpenCvSharp.Point[][] pl;
            HierarchyIndex[] hi;
            Scalar sc = new Scalar(255, 0, 0);
            Cv2.FindContours(src, out pl, out hi, RetrievalModes.List, ContourApproximationModes.ApproxNone, null);

            var contourIndex = 0;
            while ((contourIndex >= 0))
            {
                if (pl[contourIndex].Length < 250)
                {
                    foreach (OpenCvSharp.Point pt in pl[contourIndex])
                    {
                        Cv2.Circle(dst, pt.X, pt.Y, 0, sc, -1);
                    }
                    
                }
                contourIndex = hi[contourIndex].Next;
            }
            return dst;
        }
        private Mat simpleTreshold(Mat src)
        {
            Mat dst = src.Clone();
            dst = dst.MedianBlur(5);
            Cv2.AdaptiveThreshold(dst, dst, 256, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 13, 0);
            return dst;
        }
        private Mat gradLaplacian(Mat src)
        {
            Cv2.Laplacian(src, src, MatType.CV_8UC3, 3, 1, 0, BorderTypes.Default);
            return src;
        }
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            loadImageOnly(biT, biTs);
        }
    }
}
