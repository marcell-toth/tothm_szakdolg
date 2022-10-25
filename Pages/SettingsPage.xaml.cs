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
            //gitTest
        }

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
    }
}
