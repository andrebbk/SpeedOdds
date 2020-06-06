using SpeedOdds.Commons.Helpers;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
using SpeedOdds.UserControls.MainContent;
using SpeedOdds.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            InitializeComponent();
            ComboBoxStartYear.IsEnabled = false;
            ComboBoxEndYear.IsEnabled = false;
            ButtonSaveSeason.IsEnabled = false;
            _mainContent = mainContent;

            seasonService = new SeasonService();
            LoadFormData();
        }

        private void LoadFormData()
        {
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

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

                //Enable Ui
                ComboBoxStartYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxStartYear.IsEnabled = true));
                ComboBoxEndYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxEndYear.IsEnabled = true));
                ButtonSaveSeason.Dispatcher.BeginInvoke((Action)(() => ButtonSaveSeason.IsEnabled = true));

                UtilsNotification.StopLoadingAnimation();

            }).Start();            
        }

        private void LoadSeasonsGrid()
        {
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

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

                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }

        //Buttons
        private void ButtonGoBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_Competitions(_mainContent);
        }

        private void ButtonSaveSeason_Click(object sender, RoutedEventArgs e)
        {
            if ((int)ComboBoxStartYear.SelectedValue > (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("Os anos da época são inválidos!");
                return;
            }

            if ((int)ComboBoxStartYear.SelectedValue == (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("Os anos da época são inválidos!");
                return;
            }

            if (seasonService.AlreadyExistsSeason((int)ComboBoxStartYear.SelectedValue, (int)ComboBoxEndYear.SelectedValue))
            {
                NotificationHelper.notifier.ShowCustomMessage("Época já criada!");
                return;
            }

            int sYear = (int)ComboBoxStartYear.SelectedValue;
            int eYear = (int)ComboBoxEndYear.SelectedValue;

            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();
                
                //Save new season
                if (seasonService.CreateSeason(sYear, eYear))
                {
                    NotificationHelper.notifier.ShowCustomMessage("Época criada com sucesso!");

                    //Reset UI
                    ComboBoxStartYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxStartYear.SelectedValue = DateTime.Now.Year));
                    ComboBoxEndYear.Dispatcher.BeginInvoke((Action)(() => ComboBoxEndYear.SelectedValue = DateTime.Now.Year));

                    LoadSeasonsGrid();
                }
                else
                    NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro ao  criar a Época!");

                UtilsNotification.StopLoadingAnimation();

            }).Start();          
        }

        private void ButtonRemoveSeason_Click(object sender, RoutedEventArgs e)
        {           
            SeasonDataModel item = (sender as Button).DataContext as SeasonDataModel;
            if(item != null)
            {
                ConfirmationWindow _popupConfirm = new ConfirmationWindow("Tens a certeza que pretendes remover a " + item.SeasonName + "?");
                if (_popupConfirm.ShowDialog() == true)
                {
                    new Thread(() =>
                    {
                        UtilsNotification.StartLoadingAnimation();

                        if (seasonService.CanDeleteById(item.SeasonId))
                        {
                            if (seasonService.RemoveSeason(item.SeasonId))
                            {
                                NotificationHelper.notifier.ShowCustomMessage("Época removida com sucesso!");
                                LoadSeasonsGrid();
                            }
                            else
                                NotificationHelper.notifier.ShowCustomMessage("Erro ao remover época!");
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("Não é possivel eliminar esta época!\nContacte o Admin do sistema...");

                        UtilsNotification.StopLoadingAnimation();

                    }).Start();
                    
                }
                
            }
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
