using KS.GuessAthlete.Component.Logging.Implemetation;
using KS.GuessAthlete.Component.Logging.Interface;
using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.WPF.Utility;
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

namespace KS.GuessAthlete.WPF.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        protected ILogger Logger { get; set; }

        public LoginPage()
        {
            Logger = EnterpriseLogger.Instance;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!ValidateUI())
            {
                return;
            }

            Account account = new Account
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password
            };

            this.ShowWaitCursor(true);

            this.DoBackGroundWork(() =>
            {
                try
                {
                    MainWindow
                       .Client
                       .Authenticate(account)
                       .Wait();

                    LoginSucceeded();
                }
                catch (Exception ex)
                {
                    Logger.Error(this, ex);

                    this.ShowMessageDialogue("titLoginError",
                        "msgLoginError",
                        image: MessageBoxImage.Error);
                }
            });
        }

        private bool ValidateUI()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                return false;
            }

            return true;
        }

        private void LoginSucceeded()
        {
            this.ShowWaitCursor(false);

            Application.Current.Dispatcher.Invoke(() =>
            {
                btnLogin.Visibility = Visibility.Collapsed;

                Window mainWindow = Application.Current.MainWindow;
                MainWindow myWindow = (MainWindow)mainWindow;
                if (myWindow != null)
                {
                    myWindow.EnableNavigationBar(true);
                }

                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(new MainDashboardPage());
            });
        }
    }
}
