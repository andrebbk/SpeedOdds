using BlurMessageBox;
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
                NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Os anos da época são inválidos!");
                return;
            }

            if((int)ComboBoxStartYear.SelectedValue == (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Os anos da época são inválidos!");
                return;
            }

            if (seasonService.AlreadyExistsSeason((int)ComboBoxStartYear.SelectedValue, (int)ComboBoxEndYear.SelectedValue))
            {
                NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Época já criada!");
                return;
            }

            //Save new season
            if(seasonService.CreateSeason((int)ComboBoxStartYear.SelectedValue, (int)ComboBoxEndYear.SelectedValue))
            {
                NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Época criada com sucesso!");

                //Reset UI
                ComboBoxStartYear.SelectedValue = DateTime.Now.Year;
                ComboBoxEndYear.SelectedValue = DateTime.Now.Year;

                LoadSeasonsGrid();
            }
            else
                NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Ocorreu um erro ao  criar a Época!");

        }

        private void ButtonRemoveSeason_Click(object sender, RoutedEventArgs e)
        {           
            SeasonDataModel item = (sender as Button).DataContext as SeasonDataModel;
            if(item != null)
            {
                if (this.MessageBoxShow("Tens a certeza que pretendes remover a " + item.SeasonName + "?", "Speed Odds",
                Buttons.YesNo, Icons.Warning, AnimateStyle.FadeIn) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (seasonService.CanDeleteById(item.SeasonId))
                    {
                        if (seasonService.RemoveSeason(item.SeasonId))
                        {
                            NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Época removida com sucesso!");
                            LoadSeasonsGrid();
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Erro ao remover época!");
                    }
                    else
                        NotificationHelper.notifier.ShowCustomMessage("Speed Odds", "Não é possivel eliminar esta época!\nContacte o Admin do sistema...");
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
