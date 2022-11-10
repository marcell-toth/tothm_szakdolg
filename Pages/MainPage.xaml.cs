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
using OpenCvSharp.XFeatures2D;
//using System.Drawing;

namespace tothm_szak.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    /// MainWindow mainwin = (MainWindow)System.Windows.Window.GetWindow(this);

    public partial class MainPage : Page
    {
        // empty BitmapImages for the base image and the processed image
        BitmapImage biT = new BitmapImage();
        BitmapImage biTs = new BitmapImage();
        
        public MainPage()
        {
            InitializeComponent();
        }

        // properties storing the:
        // -currently displaying image's number
        // -currently displayed page's number (4x4 => 16 image per page)
        // -total number of suitable images in the folder
        public int currentImage { get; set; } = 0;
        public int currentPage { get; set; } = 0;
        public int numOfImages { get; set; } = 0;

        // list with all the image paths
        List<string> images = new List<string>();

        /// <summary>
        /// Gets the images from the given folder path fitting the given file extensions
        /// </summary>
        /// <param name="folderPath">
        /// string containing the path to the selected folder
        /// </param>
        /// <returns></returns>
        private bool getImages(string folderPath)
        {
            // list to store the selected file extensions as string
            List<String> allowedExtensions = new List<String>();
            allowedExtensions = allowedFileTypes(allowedExtensions);

            // checking if there is a given path and
            // if there are more than 0 allowed extensions selected
            if (ConfigClass.folderPath != "" && allowedExtensions.Count() > 0)
            {
                // gets the image paths in the directory matching the given extensions
                var imagesInDirectory = Directory
                .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(s => allowedExtensions.Contains(System.IO.Path.GetExtension(s)
                .TrimStart('.').ToLowerInvariant()));

                // sets the list containing all the image paths
                // and the total number of images
                images = imagesInDirectory.ToList();
                numOfImages = imagesInDirectory.Count();
                tbNumberOfImages.Text = "Képek száma: " + numOfImages.ToString();

                // false if there are no images fitting the given extensions in the folder
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

        // gets the allowed file extensions from the config class and
        // returns a string list with the allowed types each being a string
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

        // refresh button handling
        // reloads the path to the image directory and
        // sets the current image as the first image on the first page
        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            // sets the allowed test type
            setTestControls(ConfigClass.testModes[ConfigClass.testType.folderTest]);

            // resets tracked values and clears the preview grid
            currentImage = 0;
            numOfImages = 0;
            InnerGrid.Children.RemoveRange(0, InnerGrid.Children.Count);

            // check if there is a selected directory
            if (ConfigClass.folderPath != "")
            {
                // true if the images were loaded properly
                // false if no images could be loaded
                bool pathAndFileCheck = getImages(ConfigClass.folderPath);
                if (pathAndFileCheck)
                {              
                    // if there is a selected directory with images in it,
                    // loads the first image and preview page and enables controls
                    loadImage(0);
                    tbImgCounter.Text = "1";
                    btPrevImg.IsEnabled = true;
                    btNextImg.IsEnabled = true;
                    generateButtons(currentPage);
                } else
                {
                    // if there is not a selected directory
                    // current image is reset and controls are disabled
                    tbImgCounter.Text = "0";
                    btPrevImg.IsEnabled = false;
                    btNextImg.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// generates the 4 x 4 preview page
        /// </summary>
        /// <param name="previewPage">
        /// the number of the currently generated page
        /// a page contains 16 preview images, so the images 
        /// are cut into 16 image blocks
        /// </param>
        private void generateButtons(int previewPage)
        {
            // clears previous 16 images from preview
            InnerGrid.Children.RemoveRange(0, InnerGrid.Children.Count);
            
            // calculates which should be the first of the 16 images generated,
            // moving in 16 block chunks, so suitable images to start a page at is 1, 17, 33 etc
            int renderStart = (0 + previewPage) * 16;
            int renderStop = numOfImages - renderStart;
            if (renderStop > 16) { renderStop = 16; }
            renderStop = renderStop + renderStart;

            // counts to 16 through the 4 rows and columns
            int count = renderStart;
            for (int row = 0; row < 4; row++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (count != renderStop)
                    {
                        // declares button and image
                        System.Windows.Controls.Image imgSelect = new System.Windows.Controls.Image();
                        System.Windows.Controls.Button imgButton = new System.Windows.Controls.Button();

                        // image is set as content of button
                        // button is assigned its position in the 4 x 4 grid
                        imgButton.Content = imgSelect;
                        Grid.SetColumn(imgButton, i);
                        Grid.SetRow(imgButton, row);

                        // image parameters
                        imgSelect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        imgSelect.Height = imgSelect.Width;
                        imgSelect.Stretch = Stretch.Uniform;
                        imgSelect.StretchDirection = StretchDirection.Both;

                        // click event added to the generated button
                        int currentNum = count;
                        imgButton.Click += delegate (object sender, RoutedEventArgs e) { imgButton_Click(sender, e, currentNum); };

                        // selected image loaded
                        BitmapImage bimage = new BitmapImage();
                        bimage.BeginInit();
                        bimage.UriSource = new Uri(images[count], UriKind.Absolute);
                        bimage.EndInit();
                        imgSelect.Source = bimage;

                        // button added to the grid
                        InnerGrid.Children.Add(imgButton);
                        count++;
                    }
                    else { break; }
                }
            }
            // page counter, moving in 16 image blocks
            // one page contains 16 images
            tbCurrentPage.Text = "Oldal: " + (currentPage + 1);
        }

        // event added to the generated buttons, cycles to the image clicked on
        void imgButton_Click(object sender, RoutedEventArgs e, int imgNum)
        {
            currentImage = imgNum;
            loadImage(imgNum);
            loadImageNum(imgNum);
        }

        // loads the currently active image to
        // the right side of the screen to be displayed
        private void loadImage(int num)
        {

            Mat src = new Mat(images[num], ImreadModes.Unchanged);
            Mat srcGray = new Mat(images[num], ImreadModes.Grayscale);

            Bitmap bT = BitmapConverter.ToBitmap(src);
            biT = ImageProcUtility.Bitmap2BitmapImage(bT);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Mat processedImage = selectProcess(src, srcGray);
            Cv2.CvtColor(processedImage, processedImage, ColorConversionCodes.RGBA2RGB);
            Bitmap bTs = BitmapConverter.ToBitmap(processedImage, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            biTs = ImageProcUtility.Bitmap2BitmapImage(bTs);

            loadImageOnly(biT, biTs);

            watch.Stop();
            if (ConfigClass.testModes[ConfigClass.testType.singleTest])
            {
                var elapsedMs = watch.ElapsedMilliseconds;
                lbExecTime.Content = "Time: " + elapsedMs + "ms";
            } else
            {
                lbExecTime.Content = "Time: - ms";
            }

            pbMultiTest.Value = ((double)(currentImage + 1) / (double)(numOfImages + 1) * 100);
        }
        private void loadImageOnly(BitmapImage imageBase, BitmapImage imageProc)
        {
            if (cbProcessed.IsChecked == true)
            {
                testImg1.Source = biT;
                testImg2.Source = biTs;
            }
            else
            {
                testImg1.Source = biT;
                testImg2.Source = biT;
            }
        }
        private void loadImageNum(int dir)
        {
            switch (dir)
            {
                case -2:    //cycle back
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
                case -1:    //cycle forward
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
            cyclePage(currentImage);
        }
        private void cyclePage(int currentImg)
        {
            int cmpPage = (currentImg) / 16;
            if (cmpPage != currentPage)
            {
                currentPage = cmpPage;
                generateButtons(cmpPage);
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
                        processedImage =  lL.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.SelectiveSearch:
                    {
                        selectiveSearch sS = new();
                        processedImage = sS.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.Contour:
                    {
                        findContours fC = new();
                        processedImage = fC.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.SimpleThreshold:
                    {
                        simpleThreshold sT = new();
                        processedImage = sT.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.OtsuThreshold:
                    {
                        otsuThreshold oT = new();
                        processedImage = oT.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.AdaptiveThreshold:
                    {
                        adaptiveThreshold aT = new();
                        processedImage = aT.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.BradleyThreshold:
                    {
                        bradleyThreshold bT = new();
                        processedImage = bT.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.Canny:
                    {
                        cannyEdgeDetection cED = new();
                        processedImage = cED.processAndReturnImage(src);
                        return processedImage;
                    }
                case ConfigClass.processMode.KMeans:
                    {
                        kMeans kM = new();
                        processedImage = kM.processAndReturnImage(src);
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
                case Key.Up:
                    btPageUp_Click(sender, e);
                    e.Handled = true;
                    break;
                case Key.Down:
                    btPageDown_Click(sender, e);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void pageButtonHandling(int dir)
        {
            switch (dir)
            {
                case 0:
                    {
                        if ((currentPage + 1) * 16 >= numOfImages)
                        {
                            currentPage = 0;
                        }
                        else
                        {
                            currentPage++;
                        }
                        currentImage = currentPage * 16;
                        generateButtons(currentPage);
                        loadImage(currentImage);
                        tbImgCounter.Text = (currentImage + 1).ToString();
                        break;
                    }
                case 1:
                    {
                        if (currentPage == 0)
                        {
                            currentPage = numOfImages / 16;
                            if (numOfImages % 16 == 0) { currentPage--; }
                        } else
                        {
                            currentPage--;
                        }
                        currentImage = currentPage * 16;
                        generateButtons(currentPage);
                        loadImage(currentImage);
                        tbImgCounter.Text = (currentImage + 1).ToString();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void btPageDown_Click(object sender, RoutedEventArgs e)
        {
            pageButtonHandling(0);
        }

        private void btPageUp_Click(object sender, RoutedEventArgs e)
        {
            pageButtonHandling(1);
        }

        private void setTestControls(bool isenabled)
        {
            btStartTest.IsEnabled = isenabled;
            pbMultiTest.IsEnabled = isenabled;
        }

        private async void btStartTest_Click(object sender, RoutedEventArgs e)
        {
            setControlEnabled(false);

            currentImage = 0;
            int totalCheckedImage = 0;
            Int64 elapsedMs = 0;
            for (int i = 0; i < ConfigClass.cycleNum; i++)
            {
                do
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    loadImageNum(-1);
                    watch.Stop();
                    totalCheckedImage++;
                    elapsedMs += watch.ElapsedMilliseconds;
                    int delay = ConfigClass.waitNum;

                    if (ConfigClass.waitNum > 0)
                    {
                        await Task.Delay(delay);
                    }
                    else
                    {
                        await Task.Delay(1);
                    }
                } while (currentImage != 0);
            }

            MainWindow mainwin = (MainWindow)System.Windows.Window.GetWindow(this);
            mainwin.tbTestOutput.Text = "Time: " + elapsedMs + "ms\n"
                                        + "Képszám: " + totalCheckedImage + "db\n"
                                        + "Avg: " + elapsedMs / totalCheckedImage + "ms\n";
            mainwin.tbTestOutput.Visibility = Visibility.Visible;

            setControlEnabled(true);
        }

        private void setControlEnabled(bool controlEnable)
        {
            MainWindow mainwin = (MainWindow)System.Windows.Window.GetWindow(this);

            mainwin.btMainPage.IsEnabled = controlEnable;
            mainwin.btSettings.IsEnabled = controlEnable;
            mainwin.btOpenDir.IsEnabled = controlEnable;

            btNextImg.IsEnabled = controlEnable;
            btPrevImg.IsEnabled = controlEnable;
            btPageUp.IsEnabled = controlEnable;
            btPageDown.IsEnabled = controlEnable;

            btRefresh.IsEnabled = controlEnable;
            btStartTest.IsEnabled = controlEnable;

            InnerGrid.IsEnabled = controlEnable;
        }
    }
}
