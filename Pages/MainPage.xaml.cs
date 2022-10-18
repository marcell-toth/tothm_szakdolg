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
                numOfImages = imagesInDirectory.Count();
                tbNumberOfImages.Text = "Képek száma: " + numOfImages.ToString();
            }
        }

        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentImage = 0;
            numOfImages = 0;
            if (ConfigClass.folderPath != "")
            {
                getImages(ConfigClass.folderPath);
                loadImage(0);
                tbImgCounter.Text = "1";
                btPrevImg.IsEnabled = true;
                btNextImg.IsEnabled = true;
                generateButtons(numOfImages);
            }
        }

        private void generateButtons(int buttonCount)
        {
            int row = 0;
            for (int i = 0; i < buttonCount; i++)
            {


                Image imgSelect = new Image();
                Grid.SetColumn(imgSelect, i - row * 4);
                Grid.SetRow(imgSelect, row);

                imgSelect.HorizontalAlignment = HorizontalAlignment.Stretch;
                imgSelect.Height = imgSelect.Width;
                imgSelect.Stretch = Stretch.Uniform;
                imgSelect.StretchDirection = StretchDirection.Both;


                BitmapImage bimage = new BitmapImage();
                bimage.BeginInit();
                bimage.UriSource = new Uri(images[i], UriKind.Absolute);
                bimage.EndInit();
                imgSelect.Source = bimage;

                InnerGrid.Children.Add(imgSelect);
                if (i == 3) { row++; }
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
