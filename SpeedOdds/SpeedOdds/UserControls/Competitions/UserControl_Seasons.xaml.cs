using SpeedOdds.Commons.Helpers;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
using SpeedOdds.UserControls.Loading;
using SpeedOdds.UserControls.MainContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SpeedOdds.UserControls.Competitions
{
    /// <summary>
    /// Interaction logic for UserControl_Seasons.xaml
    /// </summary>
    public partial class UserControl_Seasons : UserControl
    {
        private UserControl_MainContent _mainContent;
        private SeasonService seasonService;

        public UserControl_Seasons(UserControl_MainContent mainContent)
        {
            mainContent.WFAPContentContainer.Content = new UserControl_Loading();

            InitializeComponent();
            _mainContent = mainContent;

            seasonService = new SeasonService();
            LoadFormData();
        }

        private void LoadFormData()
        {
            new Thread(() =>
            {
                //Get seasons
                var seasonList = seasonService.GetSeasons();

                DataGridSeasons.Dispatcher.BeginInvoke((Action)(() => DataGridSeasons.ItemsSource = null));
                ObservableCollection<SeasonDataModel> seasonItems = new ObservableCollection<SeasonDataModel>();

                if (seasonList != null && seasonList.Count() > 0)
                {
                    foreach (var item in seasonList)
                        seasonItems.Add(new SeasonDataModel()
                        {
                            SeasonId = item.SeasonId,
                            SeasonName = item.Name,
                            StartYear = item.StartYear,
                            EndYear = item.EndYear,
                            CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                        });

                    //BINDING
                    DataGridSeasons.Dispatcher.BeginInvoke((Action)(() => DataGridSeasons.ItemsSource = seasonItems));                    
                }


                //Load other data
                ObservableCollection<int> yearsList = new ObservableCollection<int>();
                int currentYear = DateTime.Now.Year;

                //10 years before
                for (int i = currentYear - 10; i < currentYear; i++)
                    yearsList.Add(i);

                yearsList.Add(currentYear);

                //10 years after
                for (int i = currentYear + 1; i < currentYear + 10; i++)
                    yearsList.Add(i);

                ComboBoxStartYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxStartYear.ItemsSource = yearsList));
                ComboBoxStartYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxStartYear.SelectedValue = currentYear));
                ComboBoxEndYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxEndYear.ItemsSource = yearsList));
                ComboBoxEndYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxEndYear.SelectedValue = currentYear));

                _mainContent.Dispatcher.BeginInvoke((Action)(() => _mainContent.WFAPContentContainer.Content = this)); 

            }).Start();            
        }

        private void LoadSeasonsGrid()
        {
            new Thread(() =>
            {
                //Get seasons
                var seasonList = seasonService.GetSeasons();

                DataGridSeasons.Dispatcher.BeginInvoke((Action)(() => DataGridSeasons.ItemsSource = null));
                ObservableCollection<SeasonDataModel> seasonItems = new ObservableCollection<SeasonDataModel>();

                if (seasonList != null && seasonList.Count() > 0)
                {
                    foreach (var item in seasonList)
                        seasonItems.Add(new SeasonDataModel()
                        {
                            SeasonId = item.SeasonId,
                            SeasonName = item.Name,
                            StartYear = item.StartYear,
                            EndYear = item.EndYear,
                            CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                        });

                    //BINDING
                    DataGridSeasons.Dispatcher.BeginInvoke((Action)(() => DataGridSeasons.ItemsSource = seasonItems));
                }                

            }).Start();
        }

        //Buttons
        private void ButtonGoBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_Competitions(_mainContent);
        }

        private void ButtonSaveSeason_Click(object sender, RoutedEventArgs e)
        {
            if((int)ComboBoxStartYear.SelectedValue > (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Os anos da época são inválidos!");
                return;
            }

            if((int)ComboBoxStartYear.SelectedValue == (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Os anos da época são inválidos!");
                return;
            }

            if (seasonService.AlreadyExistsSeason((int)ComboBoxStartYear.SelectedValue, (int)ComboBoxEndYear.SelectedValue))
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Época já criada!");
                return;
            }

            //Save new season
            if(seasonService.CreateSeason((int)ComboBoxStartYear.SelectedValue, (int)ComboBoxEndYear.SelectedValue))
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Época criada com sucesso!");

                //Reset UI
                ComboBoxStartYear.SelectedValue = DateTime.Now.Year;
                ComboBoxEndYear.SelectedValue = DateTime.Now.Year;

                LoadSeasonsGrid();
            }
            else
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Ocorreu um erro a criar a Época!");

        }
    }

    class SeasonDataModel
    {
        public int SeasonId { get; set; }

        public string SeasonName { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public string CreateDate { get; set; }
    }
}
