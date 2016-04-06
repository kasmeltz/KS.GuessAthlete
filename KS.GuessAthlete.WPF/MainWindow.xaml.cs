using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.WPF.Pages;
using System;
using System.Configuration;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace KS.GuessAthlete.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static WebApiClient Client { get; set; }

        public MainWindow()
        {
            SetLanguageDictionary();
            InitializeComponent();
            Client = new WebApiClient(ConfigurationManager.AppSettings["BaseApiUrl"], true);

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _mainFrame.Navigate(new LoginPage());
        }

        protected void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        #region RUTINAS                

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                    UriKind.Relative);
                    break;
                case "fr-CA":
                    dict.Source = new Uri("..\\Resources\\StringResources.fr-CA.xaml",
                                    UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                    UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(dict);
        }

        #endregion

        #region Menu

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Toolbar

        public void EnableNavigationBar(bool isEnabled)
        {
            foreach (Button b in stkNavigation.Children)
            {
                b.IsEnabled = isEnabled;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LoginPage());
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MainDashboardPage());
        }

        #endregion
    }
}
