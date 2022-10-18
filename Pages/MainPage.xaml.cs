using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tothm_szak.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public int currentImage { get; set; } = 0;
        public int numOfImages { get; set; } = 0;

        List<string> images = new List<string>();

        private void getImages(string folderPath)
        {
            var allowedExtensions = new[] { "png", "jpg" };
            if (ConfigClass.folderPath != "")
            {
                var imagesInDirectory = Directory
                .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(s => allowedExtensions.Contains(System.IO.Path.GetExtension(s)
                .TrimStart('.').ToLowerInvariant()));

                images = imagesInDirectory.ToList();

                ConfigClass.ImgPath = imagesInDirectory;

                numOfImages = imagesInDirectory.Count();

                tbNumberOfImages.Text = "Képek száma: " + numOfImages.ToString();
            }
            /*
             * BACKUP, NEEDS FIXING
             * 
            List<string> imagesInDirectory = new List<string>();
                Directory.EnumerateFiles(folderPath, allowedExtensions));
            foreach (string extension in allowedExtensions)
            {
                string m = "";
                imagesInDirectory.Add(Directory.EnumerateFiles(folderPath, extension));
                Directory.EnumerateFiles(folderPath, extension);
            }
            
            string[] imagesInDirectory = Directory.GetFiles(folderPath);
            int szam = imagesInDirectory.Length;
            tbDirPath.Text = Int32.Parse(imagesInDirectory.Length);
            */

        }

        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (ConfigClass.folderPath != "")
            {
                getImages(ConfigClass.folderPath);
                loadImage(0);
                tbImgCounter.Text = "1";
                btPrevImg.IsEnabled = true;
                btNextImg.IsEnabled = true;
            }
            
        }

        private void loadImage(int num)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(images[num], UriKind.Absolute);
            bimage.EndInit();
            testImg.Source = bimage;
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
    }
}
