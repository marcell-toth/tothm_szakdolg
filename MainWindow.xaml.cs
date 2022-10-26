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

        Page mainpage;
        Page setpage;
        public MainWindow()
        {
            InitializeComponent();
            
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.7);
            this.Height = (this.Width*0.65);

            mainpage = new MainPage();
            setpage = new SettingsPage();
        }
        
        public void SetVisibilityOnButton()
        {
            {
                mainFrame.Visibility = Visibility.Visible;
            }
        }
        private void btPage1_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = mainpage;
            enableSideInfo();
            SetVisibilityOnButton();
        }

        private void btSettings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = setpage;
            SetVisibilityOnButton();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btOpenDir_Click(object sender, RoutedEventArgs e)
        {


            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                ConfigClass.folderPath = folderBrowserDialog1.SelectedPath;
                tbDirPath.Text = ConfigClass.folderPath;
            }
        }

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

    }
}
