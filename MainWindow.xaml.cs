using System;
using System.Collections.Generic;
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

namespace tothm_szak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.7);
            this.Height = (this.Width*0.65);

        }
        
        private void btPage1_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new MainPage();
            
        }

        private void btSettings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new SettingsPage();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
