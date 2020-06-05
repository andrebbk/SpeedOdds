using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models.Shared;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
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

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_AddMatch_StartTime.xaml
    /// </summary>
    public partial class UserControl_AddMatch_StartTime : UserControl
    {
        private CompetitionService competitionService;
        private TeamService teamService;

        //properties

        public UserControl_AddMatch_StartTime()
        {
            InitializeComponent();
            competitionService = new CompetitionService();
            teamService = new TeamService();

            IniForm();
        }

        private void IniForm()
        {
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();
               
                //Load competitions
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

                ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.ItemsSource = compsBox));
                ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.SelectedValue = compsBox.FirstOrDefault()));

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private bool IsMatchesListValid()
        {
            int nRows = 0;
            DataGridTeams.Dispatcher.BeginInvoke((Action)(() => nRows = DataGridTeams.ItemsSource.OfType<object>().Count()));
            if(nRows < 1)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Adiciona jogos para procederes com as odds!");
                return false;
            }

            Tuple<int, string> matches = null;
            DataGridTeams.Dispatcher.BeginInvoke((Action)(() =>
            {
                foreach (var item in DataGridTeams.ItemsSource)
                    if (((MatchesModel)item).HomeTeamId == 0)
                    {
                        matches = new Tuple<int, string>(((MatchesModel)item).MatchViewId, "casa");
                    }
                    else if(((MatchesModel)item).AwayTeamId == 0)
                    {
                        matches = new Tuple<int, string>(((MatchesModel)item).MatchViewId, "fora");
                    }
            }));
            if(matches != null)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "No jogo nº " + matches.Item1.ToString() + 
                    " falta selecionar a equipa de " + matches.Item2);
                return false;
            }

            return true;
        }

        //BUTTONS
        private void ButtonLoadForm_Click(object sender, RoutedEventArgs e)
        {
            Visibility auxGridVisibility = DataGridTeams.Visibility;
            int compId = ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId;

            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                //GRID
                DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = null));
                ObservableCollection<MatchesModel> matchItems = new ObservableCollection<MatchesModel>();

                //GetCompTeams
                var tList = teamService.GetCompetitionTeams(compId);
                ObservableCollection<TeamComboModel> auxTeams = new ObservableCollection<TeamComboModel>();

                for (int i = 1; i < 11; i++)
                {
                    MatchesModel newMatch = new MatchesModel()
                    {
                        MatchViewId = i,
                        HomeGoals = 4,
                        AwayGoals = 1,
                        OddsDraw = (decimal)1.5
                    };

                    if (tList != null && tList.Count() > 0)
                        foreach (var item in tList) newMatch.TeamsList.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                    matchItems.Add(newMatch);
                }

                if(auxGridVisibility != Visibility.Visible)
                {
                    //HIDE INITIAL INFO 
                    ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Collapsed));
                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Collapsed));
                }                

                //BINDING
                DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = matchItems));

                if (auxGridVisibility != Visibility.Visible)
                {
                    //SHOW GRID
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.Visibility = Visibility.Visible));
                    ButtonSaveAllMatches.Dispatcher.BeginInvoke((Action)(() => ButtonSaveAllMatches.Visibility = Visibility.Visible));
                    ButtonAddNewGame.Dispatcher.BeginInvoke((Action)(() => ButtonAddNewGame.Visibility = Visibility.Visible));
                }

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private void ButtonSaveSingleMatch_Click(object sender, RoutedEventArgs e)
        {
            MatchesModel item = (sender as Button).DataContext as MatchesModel;
            if (item != null)
            {

            }
        }

        private void ButtonSaveAllMatches_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                if (IsMatchesListValid())
                {

                }
            }).Start();
        }

        private void ButtonAddNewGame_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class CompetitionComboModel
    {
        public int CompetitionId { get; set; }

        public string CompetitionName { get; set; }
    }

    public class TeamComboModel
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }
    }

    public class MatchesModel: ObservableObject
    {
        public MatchesModel()
        {
            homeTeamId = 0;
            awayTeamId = 0;

            teamsList = new ObservableCollection<TeamComboModel>();
            teamsList.Add(new TeamComboModel()
            {
                TeamId = 0,
                TeamName = "Selecionar Equipa"
            });

            homeGoals = 0;
            awayGoals = 0;

            oddsHome = 0;
            oddsDraw = 0;
            oddsAway = 0;
        }

        private int matchViewId;
        public int MatchViewId
        {
            get { return matchViewId; }
            set
            {
                matchViewId = value;
                OnPropertyChanged("MatchViewId");
            }
        }

        private int homeTeamId;
        public int HomeTeamId
        {
            get { return homeTeamId; }
            set
            {
                homeTeamId = value;
                OnPropertyChanged("HomeTeamId");
            }
        }

        private int awayTeamId;
        public int AwayTeamId
        {
            get { return awayTeamId; }
            set
            {
                awayTeamId = value;
                OnPropertyChanged("AwayTeamId");
            }
        }

        private ObservableCollection<TeamComboModel> teamsList;
        public ObservableCollection<TeamComboModel> TeamsList
        {
            get { return teamsList; }
            set
            {
                teamsList = value;
                OnPropertyChanged("TeamsList");
            }
        }

        private int homeGoals;
        public int HomeGoals
        {
            get { return homeGoals; }
            set
            {
                homeGoals = value;
                OnPropertyChanged("HomeGoals");
            }
        }

        private int awayGoals;
        public int AwayGoals
        {
            get { return awayGoals; }
            set
            {
                awayGoals = value;
                OnPropertyChanged("AwayGoals");
            }
        }

        private decimal oddsHome;
        public decimal OddsHome
        {
            get { return oddsHome; }
            set
            {
                oddsHome = value;
                OnPropertyChanged("OddsHome");
            }
        }

        private decimal oddsDraw;
        public decimal OddsDraw
        {
            get { return oddsDraw; }
            set
            {
                oddsDraw = value;
                OnPropertyChanged("OddsDraw");
            }
        }

        private decimal oddsAway;
        public decimal OddsAway
        {
            get { return oddsAway; }
            set
            {
                oddsAway = value;
                OnPropertyChanged("OddsAway");
            }
        }
    }
}
