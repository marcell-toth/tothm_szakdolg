using System;
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
using System.Windows.Forms;
using tothm_szak.ProcessMethods;
//using System.Drawing;

namespace tothm_szak.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    /// MainWindow mainwin = (MainWindow)System.Windows.Window.GetWindow(this);
    /// Definitely check out Accord + Accord.Imaging NuGet packages
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
            if (ConfigClass.isAllowedPng) { allowedExtensions.Add("png"); }  else
            {
                allowedExtensions.RemoveAll(p => p == "png");
            }
            if (ConfigClass.isAllowedJpg) { allowedExtensions.Add("jpg"); } else
            {
                allowedExtensions.RemoveAll(p => p == "jpg");
            }
            if (ConfigClass.isAllowedJpeg) { allowedExtensions.Add("jpeg"); } else
            {
                allowedExtensions.RemoveAll(p => p == "jpeg");
            }
            return allowedExtensions;
        }
        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentImage = 0;
            numOfImages = 0;
            InnerGrid.Children.RemoveRange(0, InnerGrid.Children.Count);
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
            int count = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (count != numOfImages) 
                    {  
                        System.Windows.Controls.Image imgSelect = new System.Windows.Controls.Image();
                        System.Windows.Controls.Button imgButton = new System.Windows.Controls.Button();

                        imgButton.Content = imgSelect;
                        Grid.SetColumn(imgButton, i);
                        Grid.SetRow(imgButton, row);

                        imgSelect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        imgSelect.Height = imgSelect.Width;
                        imgSelect.Stretch = Stretch.Uniform;
                        imgSelect.StretchDirection = StretchDirection.Both;

                        int currentNum = count;
                        imgButton.Click += delegate(object sender, RoutedEventArgs e) { imgButton_Click(sender, e, currentNum); };

                        BitmapImage bimage = new BitmapImage();
                        bimage.BeginInit();
                        bimage.UriSource = new Uri(images[count], UriKind.Absolute);
                        bimage.EndInit();
                        imgSelect.Source = bimage;
                        

                        InnerGrid.Children.Add(imgButton);
                        count++;
                    }
                }
            }
        }

        void imgButton_Click(object sender, RoutedEventArgs e, int imgNum)
        {
            currentImage = imgNum;
            loadImage(imgNum);
            loadImageNum(imgNum);
        }
        private void loadImage(int num)
        {
            //Mat src = Cv2.ImRead(images[num], ImreadModes.Grayscale);
            Mat src = new Mat(images[num], ImreadModes.Unchanged);
            Mat srcGray = new Mat(images[num], ImreadModes.Grayscale);

            Bitmap bT = BitmapConverter.ToBitmap(src);
            

            biT = ImageProcUtility.Bitmap2BitmapImage(bT);


            Mat processedImage = selectProcess(src, srcGray);
            Cv2.CvtColor(processedImage, processedImage, ColorConversionCodes.RGBA2RGB);
            Bitmap bTs = BitmapConverter.ToBitmap(processedImage, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            biTs = ImageProcUtility.Bitmap2BitmapImage(bTs);

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
            switch (dir)
            {
                case -2:
                    {
                        
                        if (currentImage != 0) { currentImage--; }
                        else
                        {
                            currentImage = numOfImages - 1;
                        }
                        
                        currentImage = currentImage % numOfImages;
                        tbImgCounter.Text = (currentImage + 1).ToString();
                        loadImage(currentImage);
                        break;
                    }
                case -1:
                    {
                        currentImage++;
                        currentImage = currentImage % numOfImages;
                        tbImgCounter.Text = (currentImage + 1).ToString();
                        loadImage(currentImage);
                        break;
                    }
                default:
                    loadImage(dir);
                    tbImgCounter.Text = (dir + 1).ToString();
                    break;
            }
            
        }
        private void btPrevImg_Click(object sender, RoutedEventArgs e)
        {
            //back one
            loadImageNum(-2);
        }
        private void btNextImg_Click(object sender, RoutedEventArgs e)
        {
            //forward one
            loadImageNum(-1);
        }
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            loadImageOnly(biT, biTs);
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
                        laplacian lL = new();
                        processedImage =  lL.gradLaplacianImg(srcGray);
                        return processedImage;
                    }
                case ConfigClass.processMode.SelectiveSearch:
                    {
                        selectiveSearch sS = new();
                        processedImage = sS.searchSegmentImg(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.Contour:
                    {
                        findContours fC = new();
                        processedImage = fC.findContImg(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.SimpleThreshold:
                    {
                        simpleThreshold sT = new();
                        processedImage = sT.simpleThresholdImg(srcGray);
                        return processedImage;
                    }
                case ConfigClass.processMode.OtsuThreshold:
                    {
                        otsuThreshold oT = new();
                        processedImage = oT.otsuThresholdImg(srcGray);
                        return processedImage;
                    }
                case ConfigClass.processMode.AdaptiveThreshold:
                    {
                        adaptiveThreshold aT = new();
                        processedImage = aT.adaptiveThresholdImg(srcGray);
                        return processedImage;
                    }
                case ConfigClass.processMode.BradleyThreshold:
                    {
                        bradleyThreshold bT = new();

                        return processedImage;
                    }
                default:
                    { 
                        return src;
                    }
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    btPrevImg_Click(sender, e);
                    e.Handled = true;
                    break;
                case Key.Right:
                    btNextImg_Click(sender, e);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
