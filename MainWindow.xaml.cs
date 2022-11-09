﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
using tothm_szak.Pages;
using static System.Net.Mime.MediaTypeNames;

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

        // tesztelési info lekérése
        // aktív tesztelési módok kiírása
        private void SetTestInfo()
        {
            string enabledTestTypes = "";
            if (ConfigClass.testModes[ConfigClass.elemTeszt.singleTest])
            {
                enabledTestTypes += "Egyéni teszt\n";
            }
            if (ConfigClass.testModes[ConfigClass.elemTeszt.folderTest])
            {
                enabledTestTypes += "Mappa teszt\n";
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
