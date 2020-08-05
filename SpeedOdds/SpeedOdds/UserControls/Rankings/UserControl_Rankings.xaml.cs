using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models.Shared;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
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

namespace SpeedOdds.UserControls.Rankings
{
    /// <summary>
    /// Interaction logic for UserControl_Rankings.xaml
    /// </summary>
    public partial class UserControl_Rankings : UserControl
    {
        private UserControl_MainContent _mainContent;
        private CompetitionService competitionService;

        public UserControl_Rankings(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
            competitionService = new CompetitionService();

            LoadFormRankings();

            //LottieAnimationRanksView.Dispatcher.BeginInvoke((Action)(() => LottieAnimationRanksView.PlayAnimation()));
            //LottieAnimationRanksView.Dispatcher.BeginInvoke((Action)(() => LottieAnimationRanksView.Visibility = Visibility.Visible));

            ObservableCollection<RankingsModel> data = new ObservableCollection<RankingsModel>()
            {
                new RankingsModel(){ TeamName = "F.C. Porto", TeamRating = "1.84", TeamRank = "1"},
                new RankingsModel(){ TeamName = "S.L. Benfica", TeamRating = "1.53", TeamRank = "2"},
                new RankingsModel(){ TeamName = "Braga S.C.", TeamRating = "1.31", TeamRank = "3"},
                new RankingsModel(){ TeamName = "Sporting C.P.", TeamRating = "1.22", TeamRank = "4"},
                new RankingsModel(){ TeamName = "Famalicão", TeamRating = "1.04", TeamRank = "5"},
                new RankingsModel(){ TeamName = "Vitória S.C.", TeamRating = "0.83", TeamRank = "6"},
                new RankingsModel(){ TeamName = "Marítimo", TeamRating = "0.49", TeamRank = "7"},
            };

            DataGridRankings.ItemsSource = null;
            DataGridRankings.ItemsSource = data;
        }


        private void LoadFormRankings()
        {
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                //Load competition data
                var competitionList = competitionService.GetCompetitions();

                ObservableCollection<CompetitionComboModel> compsBox = new ObservableCollection<CompetitionComboModel>();
                compsBox.Add(new CompetitionComboModel()
                {
                    CompetitionId = 0,
                    CompetitionName = "Selecionar competição"
                });

                foreach (var item in competitionList)
                    compsBox.Add(new CompetitionComboModel()
                    {
                        CompetitionId = item.CompetitionId,
                        CompetitionName = item.Name + " - " + competitionService.GetCompetitionSeasonName(item.CompetitionId)
                    });

                ComboBoxFilterCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxFilterCompetition.ItemsSource = compsBox));
                ComboBoxFilterCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxFilterCompetition.SelectedValue = compsBox.FirstOrDefault()));

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private void CheckAndShowTeamRankings()
        {
            new Thread(() => 
            {
                UtilsNotification.StartLoadingAnimation();


                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }


        //BUTTONS
        private void ButtonShowRanks_Click(object sender, RoutedEventArgs e)
        {
            if ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue == null ||
                ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue != null ?
                ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId == 0 : true))
            {
                NotificationHelper.notifier.ShowCustomMessage("Falta escolher a competição!");
                return;
            }
        }
    }
}
