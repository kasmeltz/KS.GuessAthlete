using KS.GuessAthlete.Component.Caching.Implementation;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Component.WebService;
using KS.GuessAthlete.Data.DataAccess.Repository.Implementation;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using KS.GuessAthlete.Logic.Importers;
using System;
using System.Windows;
using System.Windows.Controls;

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

            Action<Athlete> cb = (athlete) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    txtAthleteName.Text = athlete.Name;
                });
            };

            await importer.ImportAthletes(cb);
        }

        private async void PickAthlete_Click(object sender, RoutedEventArgs e)
        {
            string url = "api/pickAthlete";
            url += "?skaterGamesPlayed=1500";
            url += "&skaterPoints=2500";
            url += "&skaterPPG=1.5";
            url += "&goalieGamesPlayed=10000";
            url += "&goalieWins=10000";
            url += "&startYear=1960";

            Athlete athlete = await MainWindow.Client.Get<Athlete>(url);
        }
    }
}
