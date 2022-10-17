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

        /*
        public string FoundImageNumber
        {
            get { return tbNumberOfImages.Text; }
            set { tbNumberOfImages.Text = value; }
        }
        */


        private void getImages(string folderPath)
        {
            var allowedExtensions = new[] { "png", "jpg" };

            var imagesInDirectory = Directory
                .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(s => allowedExtensions.Contains(System.IO.Path.GetExtension(s)
                .TrimStart('.').ToLowerInvariant()));

            ConfigClass.ImgPath = imagesInDirectory;
            
            int imageNum = imagesInDirectory.Count();

            tbNumberOfImages.Text = "Képek száma: " + imageNum.ToString();

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
            foreach (var img in ConfigClass.ImgPath)
            {

                BitmapImage bimage = new BitmapImage();
                bimage.BeginInit();
                bimage.UriSource = new Uri(img, UriKind.Absolute);
                bimage.EndInit();
                testImg.Source = bimage;
                Thread.Sleep(500);
            }
        }

        private void loadImage(int dir)
        {
            if (dir == 0) {
                int s = Int32.Parse(tbImgCounter.Text);
                tbImgCounter.Text = (s--).ToString();
            }

            if (dir == 1)
            {
                int s = Int32.Parse(tbImgCounter.Text);
                tbImgCounter.Text = (s++).ToString();
            }
        }

        private void btPrevImg_Click(object sender, RoutedEventArgs e)
        {
            loadImage(0);
        }

        private void btNextImg_Click(object sender, RoutedEventArgs e)
        {
            loadImage(1);
        }
    }
}
