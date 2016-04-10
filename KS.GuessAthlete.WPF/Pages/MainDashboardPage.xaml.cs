using KS.GuessAthlete.Component.Caching.Implementation;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.Data.DataAccess.Repository.Implementation;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Logic.Importers;
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
    /// Interaction logic for MainDashboardPage.xaml
    /// </summary>
    public partial class MainDashboardPage : Page
    {
        public MainDashboardPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ICacheProvider cacheProvider = MemoryCacheProvider
                .Instance;
            IRepositoryCollection repository = 
                new DapperRepositoryCollection(cacheProvider);
            IRestfulClient apiClient = MainWindow.Client;
            HockeyReferenceImporter importer = 
                new HockeyReferenceImporter(apiClient);
            await importer.ImportAthletes();
        }
    }
}
