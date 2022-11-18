using System.Windows;
using System.Windows.Controls;
using tothm_szak.Pages;

namespace tothm_szak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// Initializing the two used pages
        /// so the pages can be called multiple times
        /// without re-rendering
        Page mainpage;
        Page setpage;

        public MainWindow()
        {
            InitializeComponent();
            
            // Width is based on the screen width
            // Height is set as a fraction of with
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.8);
            this.Height = (this.Width * 0.45);

            mainpage = new MainPage();
            setpage = new SettingsPage();
        }
        
        // Main frame holding content pages starts out as invisible
        // Visibility is set to visible after a page is called
        public void SetVisibilityOnButton()
        {
            {
                mainFrame.Visibility = Visibility.Visible;
            }
        }

        // Calls the main image preview page
        // also enables settings info on the left side
        private void btPage1_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = mainpage;
            enableSideInfo();
            SetTestInfo();
            SetVisibilityOnButton();
        }

        // Calls the settings page
        private void btSettings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = setpage;
            SetVisibilityOnButton();
        }

        // Closes the application
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        // Opens a folder browse window
        // Sets chosen folder's path
        private void btOpenDir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigClass.folderPath = folderBrowserDialog.SelectedPath;
                tbDirPath.Text = ConfigClass.folderPath;
            }
        }

        // Enables settings info on the left side
        // Displays the currently active processing method and the allowed file extensions
        private void enableSideInfo()
        {
            tbProcessMode.Text = "Feldolgozás:\n" + ConfigClass.activeProcessMode.ToString();
            tbAllowedFiles.Text = "Képfájlok:\n";
            if (ConfigClass.isAllowedPng) tbAllowedFiles.Text = tbAllowedFiles.Text + "PNG ";
            if (ConfigClass.isAllowedJpg) tbAllowedFiles.Text = tbAllowedFiles.Text + "JPG ";
            if (ConfigClass.isAllowedJpeg) tbAllowedFiles.Text = tbAllowedFiles.Text + "JPEG ";
            tbAllowedFiles.Visibility = Visibility.Visible;
            tbProcessMode.Visibility = Visibility.Visible;
        }

        // Gets test config
        // displays currently selected test configs
        private void SetTestInfo()
        {
            string enabledTestTypes = "";
            if (ConfigClass.testModes[ConfigClass.testType.singleTest])
            {
                enabledTestTypes += "Egyéni teszt\n";
            }
            if (ConfigClass.testModes[ConfigClass.testType.folderTest])
            {
                enabledTestTypes += "Mappa teszt\n" + "Kör: " + ConfigClass.cycleNum + "\nVár: " + ConfigClass.waitNum + "(ms)";
            }

            if (enabledTestTypes != "")
            {
                tbTestMode.Text = "Tesztek:\n" + enabledTestTypes;
            } else
            {
                tbTestMode.Text = "Tesztek:\n" + "Semmi\n";
            }
            tbTestMode.Visibility = Visibility.Visible;
        }
    }
}
