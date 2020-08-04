using SpeedOdds.Commons.Enums;
using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models;
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

namespace SpeedOdds.UserControls.TimeGoals
{
    public partial class UserControl_TimeGoals : UserControl
    {
        private UserControl_MainContent _mainContent;
        private CompetitionService competitionService;
        private TeamService teamService;
        private TimeGoalsService timeGoalsService;
        private ObservableCollection<TimeGoalsModel> matchItems;

        public UserControl_TimeGoals(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
            competitionService = new CompetitionService();
            teamService = new TeamService();
            timeGoalsService = new TimeGoalsService();

            LoadFormTimeGoals();
        }

        private void LoadFormTimeGoals()
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

                ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));                

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private void LoadModelListWithCompetitionTeams(int competitionId)
        {
            UtilsNotification.StartLoadingAnimation();

            var compTeams = teamService.GetCompetitionTeams(competitionId);
            
            if(compTeams != null && compTeams != null? compTeams.Count() > 0 : false)
            {
                matchItems = new ObservableCollection<TimeGoalsModel>();
                DataGridTimeGoals.Dispatcher.BeginInvoke((Action)(() => DataGridTimeGoals.ItemsSource = null));

                LoadTimeGoalsFromCompetitionTeams(competitionId, compTeams, matchItems);

                DataGridTimeGoals.Dispatcher.BeginInvoke((Action)(() => DataGridTimeGoals.ItemsSource = matchItems));

                ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Collapsed));
                LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Collapsed));
                LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Collapsed));
                
                DataGridTimeGoals.Dispatcher.BeginInvoke((Action)(() => DataGridTimeGoals.CellEditEnding += DataGridTimeGoals_CellEditEnding));
                DataGridTimeGoals.Dispatcher.BeginInvoke((Action)(() => DataGridTimeGoals.Visibility = Visibility.Visible));
            }
            else
            {
                if (DataGridTimeGoals.Visibility != Visibility.Collapsed)
                {
                    DataGridTimeGoals.Dispatcher.BeginInvoke((Action)(() => DataGridTimeGoals.Visibility = Visibility.Collapsed));

                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => 
                        LabelInfo.Content = "Para visualizar informações de golos é necessário inserir as equipas da competição primeiro"));

                    ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                    LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));
                }
            }

            UtilsNotification.StopLoadingAnimation();
        }

        private void LoadTimeGoalsFromCompetitionTeams(int competitionId, IEnumerable<Team> compTeams, ObservableCollection<TimeGoalsModel> matchItems)
        {
            //Init 
            matchItems.Clear();

            var savedTimeGoals = timeGoalsService.GetTimeGoalsByCompetition(competitionId);
            
            foreach(var team in compTeams.OrderBy(t => t.Name))
            {
                if(savedTimeGoals != null && savedTimeGoals.Where(t => t.TeamId == team.TeamId).FirstOrDefault() != null)
                {
                    var teamTimeGoals = savedTimeGoals.Where(t => t.TeamId == team.TeamId).FirstOrDefault();
                    matchItems.Add(new TimeGoalsModel()
                    {
                        TimeGoalsId = teamTimeGoals.TimeGoalsId,
                        CompetitionId = teamTimeGoals.CompetitionId,
                        TeamId = teamTimeGoals.TeamId,
                        TeamName = team.Name,
                        TotalGoals = (teamTimeGoals.Goal15 + teamTimeGoals.Goal30 + teamTimeGoals.Goal45 + teamTimeGoals.Goal60 +
                            teamTimeGoals.Goal75 + teamTimeGoals.Goal90) ?? 0,
                        Goal15 = teamTimeGoals.Goal15 ?? 0,
                        Goal15P = timeGoalsService.GetPercentageOfTimeGoal(TimeGoalsTypeValues.Goal15, teamTimeGoals),
                        Goal30 = teamTimeGoals.Goal30 ?? 0,
                        Goal30P = timeGoalsService.GetPercentageOfTimeGoal(TimeGoalsTypeValues.Goal30, teamTimeGoals),
                        Goal45 = teamTimeGoals.Goal45 ?? 0,
                        Goal45P = timeGoalsService.GetPercentageOfTimeGoal(TimeGoalsTypeValues.Goal45, teamTimeGoals),
                        Goal60 = teamTimeGoals.Goal60 ?? 0,
                        Goal60P = timeGoalsService.GetPercentageOfTimeGoal(TimeGoalsTypeValues.Goal60, teamTimeGoals),
                        Goal75 = teamTimeGoals.Goal75 ?? 0,
                        Goal75P = timeGoalsService.GetPercentageOfTimeGoal(TimeGoalsTypeValues.Goal75, teamTimeGoals),
                        Goal90 = teamTimeGoals.Goal90 ?? 0,
                        Goal90P = timeGoalsService.GetPercentageOfTimeGoal(TimeGoalsTypeValues.Goal90, teamTimeGoals),
                    });
                }
                else
                {
                    matchItems.Add(new TimeGoalsModel()
                    {
                        CompetitionId = competitionId,
                        TeamId = team.TeamId,
                        TeamName = team.Name,
                        TotalGoals = 0,
                        Goal15 = 0,
                        Goal15P = "100%",
                        Goal30 = 0,
                        Goal30P = "100%",
                        Goal45 = 0,
                        Goal45P = "100%",
                        Goal60 = 0,
                        Goal60P = "100%",
                        Goal75 = 0,
                        Goal75P = "100%",
                        Goal90 = 0,
                        Goal90P = "100%",
                    });
                }
            }
        }

        private void ReCalculateDataFromDataGrid()
        {
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                if (matchItems != null)
                {
                    foreach (var timeGoal in matchItems)
                    {
                        timeGoal.TotalGoals = timeGoal.Goal15 + timeGoal.Goal30 + timeGoal.Goal45 + timeGoal.Goal60 +
                                timeGoal.Goal75 + timeGoal.Goal90;
                        timeGoal.Goal15P = timeGoalsService.GetPercentageOfTimeGoalByValue(timeGoal.Goal15, timeGoal.TotalGoals);
                        timeGoal.Goal30P = timeGoalsService.GetPercentageOfTimeGoalByValue(timeGoal.Goal30, timeGoal.TotalGoals);
                        timeGoal.Goal45P = timeGoalsService.GetPercentageOfTimeGoalByValue(timeGoal.Goal45, timeGoal.TotalGoals);
                        timeGoal.Goal60P = timeGoalsService.GetPercentageOfTimeGoalByValue(timeGoal.Goal60, timeGoal.TotalGoals);
                        timeGoal.Goal75P = timeGoalsService.GetPercentageOfTimeGoalByValue(timeGoal.Goal75, timeGoal.TotalGoals);
                        timeGoal.Goal90P = timeGoalsService.GetPercentageOfTimeGoalByValue(timeGoal.Goal90, timeGoal.TotalGoals);
                    }
                }

                UtilsNotification.StopLoadingAnimation();

            }).Start();           
            
        }

        //EVENTS
        private void DataGridTimeGoals_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                ReCalculateDataFromDataGrid();
            }
        }


        //BUTTONS
        private void ButtonLoadTimeGoals_Click(object sender, RoutedEventArgs e)
        {
            if ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue == null ||
                ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue != null? 
                ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId == 0 : true))
            {
                NotificationHelper.notifier.ShowCustomMessage("Falta escolher a competição!");
                return;
            }

            int auxComp = ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId;
            new Thread(() =>
            {
                //Load list
                LoadModelListWithCompetitionTeams(auxComp);

            }).Start();
            
        }

        private void ButtonSaveTimeGoals_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridTimeGoals_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ReCalculateDataFromDataGrid();
        }
    }
}
