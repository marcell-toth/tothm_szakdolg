using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// button click events setting the currently selected processing method
        /// </summary>
        private void btGrayscale_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.Grayscale;
        }

        private void btLaplace_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.Laplace;
        }

        private void btContour_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.Contour;
        }

        private void btSearchSegment_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.SelectiveSearch;
        }

        private void btSimThreshold_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.SimpleThreshold;
        }

        private void btAdpThreshold_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.AdaptiveThreshold;
        }

        private void btNone_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode= ConfigClass.processMode.None;
        }
        private void btOtsuThreshold_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.OtsuThreshold;
        }
        private void btBradleyThreshold_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.BradleyThreshold;
        }
        private void btCannyEdgeDet_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.Canny;
        }
        private void btKMeans_Checked(object sender, RoutedEventArgs e)
        {
            ConfigClass.activeProcessMode = ConfigClass.processMode.KMeans;
        }

        /// <summary>
        /// Checking which file extensions are allowed accorfing to settings given
        /// </summary>
        private void FileTypeCheck(object sender, RoutedEventArgs e)
        {
            if (settingsGrid.IsInitialized == true)
            {

                if (cbFilePng.IsChecked == true)
                {
                    ConfigClass.isAllowedPng = true;
                }
                else { ConfigClass.isAllowedPng = false; }

                if (cbFileJpg.IsChecked == true)
                {
                    ConfigClass.isAllowedJpg = true;
                }
                else { ConfigClass.isAllowedJpg = false; }

                if (cbFileJpeg.IsChecked == true)
                {
                    ConfigClass.isAllowedJpeg = true;
                }
                else { ConfigClass.isAllowedJpeg = false; }
            }
        }

        /// <summary>
        /// Checking which file extensions are allowed according to settings given
        /// </summary>
        private void TestModeCheck(object sender, RoutedEventArgs e)
        {
            if (settingsGrid.IsInitialized == true)
            {
                if (cbSingleTest.IsChecked == true)
                {
                    ConfigClass.testModes[ConfigClass.testType.singleTest] = true;
                }
                else { ConfigClass.testModes[ConfigClass.testType.singleTest] = false; }

                if (cbMultiTest.IsChecked == true)
                {
                    ConfigClass.testModes[ConfigClass.testType.folderTest] = true;
                    tbCycleNum.IsEnabled = true;  

                    tbWaitNum.IsEnabled = true;    
                }
                else 
                { 
                    ConfigClass.testModes[ConfigClass.testType.folderTest] = false;
                    tbCycleNum.IsEnabled = false;
                    tbCycleNum.Text = "1";

                    tbWaitNum.IsEnabled = false;
                    tbWaitNum.Text = "0";
                }
            }
        }

        /// <summary>
        /// Button setting the prameters given for the multiple image test
        /// Sets the number of cycles done through the directory, min: 1
        /// Sets the wait time between image generations, min: 0 (treated as 1)
        /// </summary>
        private void btSetMultitestConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbCycleNum.Text, out ConfigClass.cycleNum) || int.Parse(tbCycleNum.Text) < 1)
            {
                ConfigClass.cycleNum = 1;
            }

            if (!int.TryParse(tbWaitNum.Text, out ConfigClass.waitNum) || int.Parse(tbWaitNum.Text) < 1)
            {
                ConfigClass.waitNum = 0;
            }
        }
    }
}
