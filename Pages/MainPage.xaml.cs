using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using tothm_szak.ProcessMethods;
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
        BitmapImage bitmapSource = new BitmapImage();
        BitmapImage bitmapProc = new BitmapImage();
        
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
                    btPageUp.IsEnabled = true;
                    btPageDown.IsEnabled = true;
                    generateButtons(currentPage);
                } else
                {
                    // if there is not a selected directory
                    // current image is reset and controls are disabled
                    tbImgCounter.Text = "0";
                    btPrevImg.IsEnabled = false;
                    btNextImg.IsEnabled = false;
                    btPageUp.IsEnabled = false;
                    btPageDown.IsEnabled = false;
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

        /// <summary>
        /// loads the currently active image to the right side of the screen to be displayed
        /// </summary>
        /// <param name="num">
        /// the number of the image to be loaded, ordered from 0 to the total number of images
        /// </param>
        private Int64 loadImage(int num)
        {
            // loading the image into a Mat file
            Mat src = new Mat(images[num], ImreadModes.Unchanged);

            // converting Mat => Bitmap => BitmapImage
            bitmapSource = ImageProcUtility.Bitmap2BitmapImage(BitmapConverter.ToBitmap(src));

            // processing the source image and saving it to an OpenCV Mat variable
            // stopwatch to measure the time it takes to process an image
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Mat processedImage = selectProcess(src);
            watch.Stop();

            // reducing processed image output to 3 channels from 4
            Cv2.CvtColor(processedImage, processedImage, ColorConversionCodes.RGBA2RGB);

            // converting the output image from OpenCV Mat => Bitmap => BitmapImage
            bitmapProc = ImageProcUtility.Bitmap2BitmapImage(BitmapConverter
                .ToBitmap(processedImage, System.Drawing.Imaging.PixelFormat.Format24bppRgb));

            // loading the source and processed image to the 2 display elements
            loadImageOnly(bitmapSource, bitmapProc);

            Int64 elapsedMs = watch.ElapsedMilliseconds;

            // status of the stopwatch according to settings
            if (ConfigClass.testModes[ConfigClass.testType.singleTest])
            {
                lbExecTime.Content = "Time: " + elapsedMs + "ms";
            } else
            {
                lbExecTime.Content = "Time: - ms";
            }

            // updating the progress bar based on which image is loaded
            pbMultiTest.Value = ((double)(currentImage + 1) / (double)(numOfImages + 1) * 100);

            return elapsedMs;
        }

        /// <summary>
        /// loading the source and processed images to the 
        /// display elements on the right side of the screen
        /// </summary>
        /// <param name="imageBase"></param> source image
        /// <param name="imageProc"></param> processed image
        private void loadImageOnly(BitmapImage imageBase, BitmapImage imageProc)
        {
            // checking the state of the checkbox deciding if we are displaying
            // the source and the processed image or 
            // the source image twice
            if (cbProcessed.IsChecked == true)
            {
                testImg1.Source = bitmapSource;
                testImg2.Source = bitmapProc;
            }
            else
            {
                testImg1.Source = bitmapSource;
                testImg2.Source = bitmapSource;
            }
        }

        /// <summary>
        /// loading images based on their number
        /// handles cycling inputs, like 1 forward or 1 back
        /// updates the counter below the display elements
        /// </summary>
        /// <param name="dir">
        /// the number of the image to be displayed
        /// -2 means cycle back one
        /// -1 means cycle forward one
        /// dir >= 0 means the image with the given number is to be loaded
        /// </param>
        private long loadImageNum(int dir)
        {
            Int64 measuredTime = 0;
            switch (dir)
            {
                case -2:    //cycle back one
                    {
                        if (currentImage != 0) { currentImage--; }
                        else
                        {
                            currentImage = numOfImages - 1;
                        }
                                        
                        currentImage = currentImage % numOfImages;
                        tbImgCounter.Text = (currentImage + 1).ToString();
                        measuredTime = loadImage(currentImage);
                        break;
                    }
                case -1:    //cycle forward one
                    {
                        currentImage++;
                        currentImage = currentImage % numOfImages;

                        tbImgCounter.Text = (currentImage + 1).ToString();
                        measuredTime = loadImage(currentImage);
                        break;
                    }
                default:    // dir >= 0, image with the given number is loaded
                    loadImage(dir);
                    tbImgCounter.Text = (dir + 1).ToString();
                    break;
            }

            // checking if page cycling is needed
            cyclePage(currentImage);

            // measured time value returned, default 0ms
            return measuredTime;
        }

        /// <summary>
        /// gets the number of the currently loaded image and decides which page should be loaded
        /// pages are 4 x 4 grids containing 16 preview images
        /// currently rendered page is decided by the image's number
        /// numbers 0 - 15 (1 - 16 on display) are on page 0 (page 1 displayed)
        /// // numbers 16 - 31 (17 - 32 on display) are on page 1 (page 2 displayed) and so on
        /// </summary>
        /// <param name="currentImg"></param> given number representing the image
        private void cyclePage(int currentImg)
        {
            int cmpPage = (currentImg) / 16;

            // if page shoul be cycled according to the new image number
            // new page's buttons are generated and the page counter is updated
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

        // whenever the checkbox switching between source and processed view is clicked
        // the images are rerendered
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            loadImageOnly(bitmapSource, bitmapProc);
        }

        /// <summary>
        /// Process selection method
        /// </summary>
        /// <param name="src">
        /// the input source image
        /// </param>
        /// <returns>
        /// the processed image output as an OpenCV Mat type
        /// </returns>
        private Mat selectProcess(Mat src)
        {
            //OpenCV Mat to store the processed image output
            Mat processedImage = new Mat();

            // deciding between which method to use
            // most processing classes fit the IClassification interface
            // meaning they all must have a processAndReturnImage(Mat) method taking in the
            // source image as OpenCV Mat and returning the output as OpenCV Mat
            switch (ConfigClass.activeProcessMode)
            {
                case ConfigClass.processMode.None:
                    {
                        src.CopyTo(processedImage);
                        return processedImage;
                    }
                case ConfigClass.processMode.Grayscale:
                    {
                        Cv2.CvtColor(src, processedImage, ColorConversionCodes.BGR2GRAY);
                        return processedImage;
                    }
                case ConfigClass.processMode.Laplace:
                    {
                        laplacian lL = new();
                        processedImage =  lL.processAndReturnImage(src);
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
                        // default just returns the input
                        return src;
                    }
            }
        }

        // when a key is pressed, it's checked if its one of the 4 arrow keys
        // if it is, then the cycling method connected to the arrow key is called
        // -> next image
        // <- previous image
        // ↑  one page up (previous 16 images)
        // ↓ one page down (next 16 images)
        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (btPrevImg.IsEnabled)
                    {
                        btPrevImg_Click(sender, e);
                        e.Handled = true;
                    }
                    break;
                case Key.Right:
                    if (btNextImg.IsEnabled)
                    {
                        btNextImg_Click(sender, e);
                        e.Handled = true;
                    }
                    break;
                case Key.Up:
                    if (btPageUp.IsEnabled)
                    {
                        btPageUp_Click(sender, e);
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    if (btPageDown.IsEnabled)
                    {
                        btPageDown_Click(sender, e);
                        e.Handled = true;
                    }
                    break;

                // default, nothing happens
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles the page cycling
        /// </summary>
        /// <param name="dir">
        /// variable signaling the cycling direction
        /// 0 - page down
        /// 1 - page up
        /// </param>
        private void pageButtonHandling(int dir)
        {
            switch (dir)
            {
                // one page down
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

                // one page up
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

                // default - nothing happens
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

        /// <summary>
        /// Sets the IsEnabled property of the multiple test controls
        /// </summary>
        /// <param name="isenabled">
        /// bool deciding if the controls are enabled
        /// </param>
        private void setTestControls(bool isenabled)
        {
            btStartTest.IsEnabled = isenabled;
            pbMultiTest.IsEnabled = isenabled;
        }

        // starts the multiple image test
        // async, does not hold up the main thread, lets the UI render between image generations
        private async void btStartTest_Click(object sender, RoutedEventArgs e)
        {
            // disabled most controls 
            setControlEnabled(false);

            // resets the current image to the first in the directory
            currentImage = 0;

            // variables tracking the number of images checked and the elapsed time
            int totalCheckedImage = 0;
            Int64 elapsedMs = 0;

            // loop restarting the test on the whole directory,
            // by default runs once, value can be increased in settings
            for (int i = 0; i < ConfigClass.cycleNum; i++)
            {
                // Garbage collection cleanup
                GC.WaitForPendingFinalizers();
                GC.Collect();
                long measuredTime;
                // loop cycling through the directory
                do
                {
                    // loading the next image (-1 => next, -2 => previous) and measuring the load time
                    //var watch = System.Diagnostics.Stopwatch.StartNew();
                    measuredTime = loadImageNum(-1);
                    //watch.Stop();

                    // incrementing checked image counter and summarizing the elapsed load time
                    totalCheckedImage++;
                    //elapsedMs += watch.ElapsedMilliseconds;
                    elapsedMs += measuredTime;

                    // waits for the set amount of time(ms)
                    // if given value is 0, waits for 1ms
                    if (ConfigClass.waitNum > 0)
                    {
                        await Task.Delay(ConfigClass.waitNum);
                    }
                    else
                    {
                        await Task.Delay(1);
                    }
                } while (currentImage != 0);
            }

            // referencing the main window and displaying the gathered info after the test is complete
            MainWindow mainwin = (MainWindow)System.Windows.Window.GetWindow(this);
            mainwin.tbTestOutput.Text = "Time: " + elapsedMs + "ms\n"
                                        + "Képszám: " + totalCheckedImage + "db\n"
                                        + "Avg: " + elapsedMs / totalCheckedImage + "ms\n";
            mainwin.tbTestOutput.Visibility = Visibility.Visible;

            // re-enables most controls
            setControlEnabled(true);
        }

        /// <summary>
        /// Enables or disables most controls, 
        /// used to avoid unpredictable behaviour during the automatic test
        /// methods locking controls should call this method again to re-enable controls
        /// </summary>
        /// <param name="controlEnable">
        /// bool deciding if all the controls are to be disabled or enabled
        /// </param>
        private void setControlEnabled(bool controlEnable)
        {
            // targetting mainwindow elements
            MainWindow mainwin = (MainWindow)System.Windows.Window.GetWindow(this);

            // mainwindow controls
            mainwin.btMainPage.IsEnabled = controlEnable;
            mainwin.btSettings.IsEnabled = controlEnable;
            mainwin.btOpenDir.IsEnabled = controlEnable;

            // page controls
            btNextImg.IsEnabled = controlEnable;
            btPrevImg.IsEnabled = controlEnable;
            btPageUp.IsEnabled = controlEnable;
            btPageDown.IsEnabled = controlEnable;

            btRefresh.IsEnabled = controlEnable;
            btStartTest.IsEnabled = controlEnable;

            // preview grid controls
            InnerGrid.IsEnabled = controlEnable;
        }
    }
}
