using Microsoft.SolverFoundation.Services;
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
        private TeamService teamService;
        private MatchService matchService;

        public UserControl_Rankings(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
            competitionService = new CompetitionService();
            teamService = new TeamService();
            matchService = new MatchService();

            LoadFormRankings();           
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

        private void SolveRankings(int competitionId)
        {
            new Thread(() => 
            {
                UtilsNotification.StartLoadingAnimation();

                TeamSolver solverData = new TeamSolver();

                //Get Competition Teams
                var teams = teamService.GetCompetitionTeams(competitionId);

                if(teams != null && teams != null? teams.Count() > 0 : false)
                {
                    //Load teams into model
                    foreach (var t in teams)
                        solverData.Dados.Add(new TeamSolverData()
                        {
                            TeamId = t.TeamId,
                            TeamName = t.Name,
                            TeamDecision = new Decision(solverData.InputDomain, Utils.RemoveSpecialChars(t.Name))
                        });


                    //get Competition Matches
                    var matches = matchService.GetMatchesByCompetitionId(competitionId);

                    if (matches != null && matches != null ? matches.Count() > 0 : false)
                    {
                        //Load teams into model
                        foreach (var m in matches)
                            solverData.Jogos.Add(m);

                        //Calculate rankings using Microsoft.Solver
                        solverData = SolverHelper.SolverProblem(solverData);

                        //load model with data
                        ObservableCollection<RankingsModel> classificacoes = new ObservableCollection<RankingsModel>();

                        foreach(var cls in solverData.Dados)
                        {
                            classificacoes.Add(new RankingsModel()
                            {
                                TeamName = cls.TeamName,
                                TeamRatingValue = Math.Round(cls.TeamDecision.ToDouble(), 2),
                                TeamRating = Math.Round(cls.TeamDecision.ToDouble(), 2).ToString()
                            });
                        }

                        //checkRanks
                        int rnk = 1;
                        foreach (var r in classificacoes.OrderByDescending(x => x.TeamRatingValue))
                        {
                            r.TeamRankValue = rnk;
                            r.TeamRank = rnk.ToString();
                            rnk++;
                        }

                        DataGridRankings.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.ItemsSource = null; })); 
                        DataGridRankings.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.ItemsSource = classificacoes.OrderBy(x => x.TeamRankValue); }));
                        LabelHomeAdvantageValue.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantageValue.Text = Math.Round(solverData.HomeAdvantage.ToDouble(), 2).ToString(); }));

                        DataGridRankings.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Visible; }));
                        LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Visible; }));
                        LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantage.Visibility = Visibility.Visible; }));
                        LabelHomeAdvantageValue.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantageValue.Visibility = Visibility.Visible; }));

                        UtilsNotification.StopLoadingAnimation();
                    }
                    else
                    {
                        DataGridRankings.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Collapsed; }));
                        LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Collapsed; }));
                        LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantage.Visibility = Visibility.Collapsed; }));
                        LabelHomeAdvantageValue.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantageValue.Visibility = Visibility.Collapsed; }));

                        NotificationHelper.notifier.ShowCustomMessage("Não existem jogos registados na competição");
                        UtilsNotification.StopLoadingAnimation();
                    }

                }
                else
                {
                    DataGridRankings.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Collapsed; }));
                    LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Collapsed; }));
                    LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantage.Visibility = Visibility.Collapsed; }));
                    LabelHomeAdvantageValue.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantageValue.Visibility = Visibility.Collapsed; }));

                    NotificationHelper.notifier.ShowCustomMessage("Não existem equipas associadas à competição");
                    UtilsNotification.StopLoadingAnimation();
                }

            }).Start();
        }


        //BUTTONS
        private void ButtonShowRanks_Click(object sender, RoutedEventArgs e)
        {
            if ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue == null ||
                ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue != null ?
                ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId == 0 : true))
            {
                DataGridRankings.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Collapsed; }));
                LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { DataGridRankings.Visibility = Visibility.Collapsed; }));
                LabelHomeAdvantage.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantage.Visibility = Visibility.Collapsed; }));
                LabelHomeAdvantageValue.Dispatcher.BeginInvoke((Action)(() => { LabelHomeAdvantageValue.Visibility = Visibility.Collapsed; }));

                NotificationHelper.notifier.ShowCustomMessage("Falta escolher a competição!");
                return;
            }

            SolveRankings(((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId);
        }
    }
}
