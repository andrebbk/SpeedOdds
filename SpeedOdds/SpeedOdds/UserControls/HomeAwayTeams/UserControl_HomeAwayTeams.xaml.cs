using SpeedOdds.Commons.Enums;
using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
using SpeedOdds.UserControls.DrawableMenu;
using SpeedOdds.UserControls.MainContent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SpeedOdds.UserControls.HomeAwayTeams
{
    /// <summary>
    /// Interaction logic for UserControl_HomeAwayTeams.xaml
    /// </summary>
    public partial class UserControl_HomeAwayTeams : System.Windows.Controls.UserControl
    {
        private UserControl_MainContent _mainContent;
        private TeamService teamService;
        private MatchService matchService;

        private bool IsHomeTeam;
        private bool IsMatchType;
        private bool IsDrawableMenuOpen;

        public UserControl_DrawableMenuTeams child;

        public UserControl_HomeAwayTeams(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                _mainContent = mainContent;                
                IsDrawableMenuOpen = false;

                IsMatchType = false;
                IsHomeTeam = true;
                ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));

                teamService = new TeamService();
                matchService = new MatchService();

                DrawableMenuContainer.Dispatcher.BeginInvoke((Action)(() =>
                    DrawableMenuContainer.Content = new UserControl_DrawableMenuTeams(_mainContent, this)));

                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }

        private void LoadObservableCollection(int compId, ObservableCollection<HomeAwayTeamViewModel> matchItems, List<Team> equipas)
        {
            if (!IsMatchType)
            {
                foreach (var item in equipas.OrderBy(x => x.Name))
                {
                    var teamResults = matchService.GetTeamResults(compId, item.TeamId, IsHomeTeam);
                    var teamGoals = matchService.GetTeamGoals(compId, item.TeamId, IsHomeTeam);

                    if (teamResults != null && teamGoals != null)
                    {
                        matchItems.Add(new HomeAwayTeamViewModel()
                        {
                            Team = teamService.GetTeamName(item.TeamId),
                            Matches = teamResults.Item4.ToString(),
                            Wins = teamResults.Item1.ToString(),
                            WinsP = UtilsOddOperations.GetPercentage(teamResults.Item1, teamResults.Item4),
                            WinsO = UtilsOddOperations.GetOdd(teamResults.Item1, teamResults.Item4),
                            Draws = teamResults.Item2.ToString(),
                            DrawsP = UtilsOddOperations.GetPercentage(teamResults.Item2, teamResults.Item4),
                            DrawsO = UtilsOddOperations.GetOdd(teamResults.Item2, teamResults.Item4),
                            Defeats = teamResults.Item3.ToString(),
                            DefeatsP = UtilsOddOperations.GetPercentage(teamResults.Item3, teamResults.Item4),
                            DefeatsO = UtilsOddOperations.GetOdd(teamResults.Item3, teamResults.Item4),
                            Forma = matchService.GetTeamForma(compId, item.TeamId, IsHomeTeam).ToString(),
                            GM = UtilsOddOperations.GetAverageNonPercentual(teamGoals.Item1, teamResults.Item4),
                            GS = UtilsOddOperations.GetAverageNonPercentual(teamGoals.Item2, teamResults.Item4),
                            GmGs = UtilsOddOperations.GetGMGS(teamGoals.Item1, teamGoals.Item2, teamResults.Item4),
                            Gm_Gs = UtilsOddOperations.GetGM_GS(teamGoals.Item1, teamGoals.Item2, teamResults.Item4),
                            FatorCasa = UtilsOddOperations.GetFatorCasa(teamGoals.Item1, teamGoals.Item2, teamResults.Item4),
                            Over25 = matchService.GetTeamOver25(compId, item.TeamId, IsHomeTeam).ToString(),
                            Over25P = UtilsOddOperations.GetPercentage(matchService.GetTeamOver25(compId, item.TeamId, IsHomeTeam), teamResults.Item4),
                            Over25O = UtilsOddOperations.GetOdd(matchService.GetTeamOver25(compId, item.TeamId, IsHomeTeam), teamResults.Item4),
                            Over15 = matchService.GetTeamOver15(compId, item.TeamId, IsHomeTeam).ToString(),
                            Over15P = UtilsOddOperations.GetPercentage(matchService.GetTeamOver15(compId, item.TeamId, IsHomeTeam), teamResults.Item4),
                            Over15O = UtilsOddOperations.GetOdd(matchService.GetTeamOver15(compId, item.TeamId, IsHomeTeam), teamResults.Item4),
                            Btts = matchService.GetTeamBTTS(compId, item.TeamId, IsHomeTeam).ToString(),
                            BttsP = UtilsOddOperations.GetPercentage(matchService.GetTeamBTTS(compId, item.TeamId, IsHomeTeam), teamResults.Item4),
                            BttsO = UtilsOddOperations.GetOdd(matchService.GetTeamBTTS(compId, item.TeamId, IsHomeTeam), teamResults.Item4),
                            P00 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 0, 0), teamResults.Item4),
                            P01 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 1, 0), teamResults.Item4),
                            P10 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 0, 1), teamResults.Item4),
                            P11 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 1, 1), teamResults.Item4),
                            P20 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 2, 0), teamResults.Item4),
                            P02 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 0, 2), teamResults.Item4),
                            P21 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 2, 1), teamResults.Item4),
                            P12 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 1, 2), teamResults.Item4),
                            P22 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 2, 2), teamResults.Item4),
                            P30 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 3, 0), teamResults.Item4),
                            P03 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 0, 3), teamResults.Item4),
                            P31 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 3, 1), teamResults.Item4),
                            P13 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 1, 3), teamResults.Item4),
                            P32 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 3, 2), teamResults.Item4),
                            P23 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 2, 3), teamResults.Item4),
                            P33 = UtilsOddOperations.GetPercentage(matchService.GetTeamResultsOccurrences(compId, item.TeamId, IsHomeTeam, 3, 3), teamResults.Item4)
                        });

                        //TODO: Remove this 4 lines (used for testing colors)
                        //matchItems[matchItems.Count() - 1].P01 = "0,3%";
                        //matchItems[matchItems.Count() - 1].P30 = "10,0%";
                        //matchItems[matchItems.Count() - 1].P13 = "64,4%";
                        //matchItems[matchItems.Count() - 1].P20 = "15,4%";
                    }
                }
            }
            else
            {
                //Match types
                foreach (MatchTypeValues mt in Enum.GetValues(typeof(MatchTypeValues)))
                {
                    var matchTypeResults = matchService.GetMatchTypeResults(compId, mt);

                    if (matchTypeResults != null)
                    {
                        matchItems.Add(new HomeAwayTeamViewModel()
                        {
                            Team = UtilsOddOperations.GetMatchTypeString(mt),
                            Matches = matchTypeResults.Item4.ToString(),
                            Wins = matchTypeResults.Item1.ToString(),
                            WinsP = UtilsOddOperations.GetPercentage(matchTypeResults.Item1, matchTypeResults.Item4),
                            WinsO = UtilsOddOperations.GetOdd(matchTypeResults.Item1, matchTypeResults.Item4),
                            Draws = matchTypeResults.Item2.ToString(),
                            DrawsP = UtilsOddOperations.GetPercentage(matchTypeResults.Item2, matchTypeResults.Item4),
                            DrawsO = UtilsOddOperations.GetOdd(matchTypeResults.Item2, matchTypeResults.Item4),
                            Defeats = matchTypeResults.Item3.ToString(),
                            DefeatsP = UtilsOddOperations.GetPercentage(matchTypeResults.Item3, matchTypeResults.Item4),
                            DefeatsO = UtilsOddOperations.GetOdd(matchTypeResults.Item3, matchTypeResults.Item4),                           
                            Over25 = matchService.GetMatchTypeOver25(compId, mt).ToString(),
                            Over25P = UtilsOddOperations.GetPercentage(matchService.GetMatchTypeOver25(compId, mt), matchTypeResults.Item4),
                            Over25O = UtilsOddOperations.GetOdd(matchService.GetMatchTypeOver25(compId, mt), matchTypeResults.Item4),
                            Over15 = matchService.GetMatchTypeOver15(compId, mt).ToString(),
                            Over15P = UtilsOddOperations.GetPercentage(matchService.GetMatchTypeOver15(compId, mt), matchTypeResults.Item4),
                            Over15O = UtilsOddOperations.GetOdd(matchService.GetMatchTypeOver15(compId, mt), matchTypeResults.Item4),
                            Btts = matchService.GetMatchTypeBTTS(compId, mt).ToString(),
                            BttsP = UtilsOddOperations.GetPercentage(matchService.GetMatchTypeBTTS(compId, mt), matchTypeResults.Item4),
                            BttsO = UtilsOddOperations.GetOdd(matchService.GetMatchTypeBTTS(compId, mt), matchTypeResults.Item4),                            
                        });
                    }
                }
            }
            
        }

        private void ReloadGridMatches()
        {
            if (DataGridTeams.Visibility == Visibility.Visible && DataGridTeams.ItemsSource != null)
            {
                if (child.GetCompetitionValue() != null)
                    LoadGridWithCalculatedMatchesData(
                        child.GetCompetitionValue(),
                        child.GetTeamValue());
            }
        }

        private void CloseDrawableMenu()
        {
            if (IsDrawableMenuOpen)
            {
                Storyboard sb = this.FindResource("CloseDrawableMenu") as Storyboard;
                sb.Begin();

                IsDrawableMenuOpen = false;

                ButtonOpenDrawableMenu.Visibility = Visibility.Visible;

                Canvas.SetZIndex(DataGridTeams, 0);
            }
        }

        private void CheckValuesInGrid(int nRows)
        {
            if (nRows < 1)
                return;
         
            //Load colors
            SolidColorBrush veryBadColor = new SolidColorBrush(Color.FromRgb(218, 66, 66));
            SolidColorBrush normalColor = new SolidColorBrush(Color.FromRgb(255, 255, 51));
            SolidColorBrush goodColor = new SolidColorBrush(Color.FromRgb(102, 204, 0));
            SolidColorBrush veryGoodColor = new SolidColorBrush(Color.FromRgb(0, 153, 0));

            for (int linha=0; linha < nRows; linha++)
            {
                //Começa na coluna 26
                for (int coluna = 26; coluna < 42; coluna++)
                {
                    var celula = UtilsDataGrid.GetCell(DataGridTeams, linha, coluna);
                    if (celula != null)
                    {
                        var texto = celula.Content as TextBlock;
                        if(texto != null && !String.IsNullOrWhiteSpace(texto.Text))
                        {
                            decimal val = -1;
                            string strVal = (texto.Text.Contains("%") ? texto.Text.Replace("%", "") : texto.Text).Trim();

                            if (Decimal.TryParse(texto.Text.Replace("%", ""), out val))
                            {
                                if (val < 1)
                                    celula.Background = veryBadColor;
                                else if (val < 15)
                                    celula.Background = goodColor;
                                else
                                    celula.Background = veryGoodColor;
                            }                            
                        }
                            
                    }
                }                
            }            
            
        }

        private void ArrangeColumnsGridVisibility()
        {
            DataGridTeams.Dispatcher.BeginInvoke((Action)(() => {

                if (IsMatchType)
                {
                    //VISIBILITY
                    DataGridTeams.Columns[11].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[12].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[13].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[14].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[15].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[16].Visibility = Visibility.Collapsed;

                    DataGridTeams.Columns[26].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[27].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[28].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[29].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[30].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[31].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[32].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[33].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[34].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[35].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[36].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[37].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[38].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[39].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[40].Visibility = Visibility.Collapsed;
                    DataGridTeams.Columns[41].Visibility = Visibility.Collapsed;
                }
                else
                {
                    //VISIBILITY
                    DataGridTeams.Columns[11].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[12].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[13].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[14].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[15].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[16].Visibility = Visibility.Visible;

                    DataGridTeams.Columns[26].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[27].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[28].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[29].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[30].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[31].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[32].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[33].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[34].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[35].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[36].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[37].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[38].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[39].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[40].Visibility = Visibility.Visible;
                    DataGridTeams.Columns[41].Visibility = Visibility.Visible;
                }
                
            }));
        }

        //PUBLIC
        public void LoadGridWithCalculatedMatchesData(int? compId, int? teamId)
        {
            if (!compId.HasValue || compId.Value == 0)
            {
                NotificationHelper.notifier.ShowCustomMessage("Selecione uma competição primeiro!");
                return;
            }

            //CloseMenu
            CloseDrawableMenu();

            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                //get team(s)
                List<Team> tList = null;
                if (teamId.HasValue)
                {
                    tList.Add(new Team()
                    {
                        TeamId = teamId.Value
                    });
                }
                else
                    tList = teamService.GetCompetitionTeams(compId.Value).ToList();

                if (tList != null && tList.Count() > 0)
                {
                    ObservableCollection<HomeAwayTeamViewModel> matchItems = new ObservableCollection<HomeAwayTeamViewModel>();
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = null));

                    LoadObservableCollection(compId.Value, matchItems, tList);

                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = matchItems));

                    ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Collapsed));
                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Collapsed));
                    LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Collapsed));

                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.Visibility = Visibility.Visible));

                    //VISIBILITY                  
                    ArrangeColumnsGridVisibility();

                    //Check values and change color dynamically
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => { CheckValuesInGrid(matchItems.Count()); }));                    
                }
                else
                {
                    if (DataGridTeams.Visibility != Visibility.Collapsed)
                    {
                        DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.Visibility = Visibility.Collapsed));

                        ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                        LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                        LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));
                    }
                }                            

                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }



        //BUTTONS
        private void ButtonOpenDrawableMenu_Click(object sender, RoutedEventArgs e)
        {
            IsDrawableMenuOpen = true;
            ButtonOpenDrawableMenu.Visibility = Visibility.Hidden;

            Canvas.SetZIndex(DataGridTeams, -1);
        }

        private void ButtonHomeTeams_Click(object sender, RoutedEventArgs e)
        {
            if (!IsHomeTeam || IsMatchType)
            {
                //Selection colors
                ButtonHomeTeams.Background = new SolidColorBrush(Color.FromRgb(170, 74, 59));
                ButtonAwayTeams.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonMatchType.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));

                IsHomeTeam = true;
                IsMatchType = false;

                ReloadGridMatches();
            }
        }

        private void ButtonAwayTeams_Click(object sender, RoutedEventArgs e)
        {
            if (IsHomeTeam || IsMatchType)
            {
                //Selection colors
                ButtonAwayTeams.Background = new SolidColorBrush(Color.FromRgb(170, 74, 59));
                ButtonHomeTeams.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonMatchType.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));

                IsHomeTeam = false;
                IsMatchType = false;

                ReloadGridMatches();
            }
        }

        private void ButtonMatchType_Click(object sender, RoutedEventArgs e)
        {
            if (!IsMatchType)
            {
                //Selection colors
                ButtonHomeTeams.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonAwayTeams.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonMatchType.Background = new SolidColorBrush(Color.FromRgb(170, 74, 59));

                IsMatchType = true;

                ReloadGridMatches();
            }
        }

        //EVENTS        
        private void GridBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Click on background

            if (IsDrawableMenuOpen)
            {
                Storyboard sb = this.FindResource("CloseDrawableMenu") as Storyboard;
                sb.Begin();

                IsDrawableMenuOpen = false;

                ButtonOpenDrawableMenu.Visibility = Visibility.Visible;

                Canvas.SetZIndex(DataGridTeams, 0);
            }
        }

    }
}
