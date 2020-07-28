using SpeedOdds.Commons.Enums;
using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models;
using SpeedOdds.Models.Shared;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
using SpeedOdds.UserControls.DrawableMenu;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_Matches.xaml
    /// </summary>
    public partial class UserControl_Matches : UserControl
    {
        private UserControl_MainContent _mainContent;
        private TeamService teamService;
        private MatchService matchService;

        private MatchViewTypeValues typeMatchView; 
        private bool IsDrawableMenuOpen;

        public UserControl_DrawableMenuMatches child;

        public UserControl_Matches(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                _mainContent = mainContent;
                typeMatchView = MatchViewTypeValues.AttackDefense;

                ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));

                teamService = new TeamService();
                matchService = new MatchService();

                IsDrawableMenuOpen = false;
                DrawableMenuContainer.Dispatcher.BeginInvoke((Action)(() => 
                    DrawableMenuContainer.Content = new UserControl_DrawableMenuMatches(_mainContent, this)));

                UtilsNotification.StopLoadingAnimation();

            }).Start();           
        }

        //PRIVATE
        private void LoadObservableCollection (ObservableCollection<MatchViewModel> matchItems, List<Match> jogos)
        {
            if (typeMatchView == MatchViewTypeValues.AttackDefense)
            {
                foreach (var item in jogos.OrderBy(x => x.Order))
                {
                    matchItems.Add(new MatchViewModel()
                    {
                        MatchId = item.MatchId,
                        Order = item.Order,
                        HomeTeam = teamService.GetTeamName(item.HomeTeamId),
                        HomeGoals = item.HomeGoals.ToString(),
                        AwayTeam = teamService.GetTeamName(item.AwayTeamId),
                        AwayGoals = item.AwayGoals.ToString(),
                        MatchResult = UtilsOddOperations.GetResultado(item.HomeGoals, item.AwayGoals),
                        MatchResultCSFT = item.HomeGoals.ToString() + " - " + item.AwayGoals.ToString(),
                        OddsHome = item.HomeOdd.ToString("#0.00").Replace(',', '.'),
                        OddsAway = item.AwayOdd.ToString("#0.00").Replace(',', '.'),
                        OddsDraw = item.DrawOdd.ToString("#0.00").Replace(',', '.'),
                        MatchType = UtilsOddOperations.GetTipoJogo(item.HomeOdd, item.AwayOdd),
                        Over15 = UtilsOddOperations.GetOverUnder15(item.HomeGoals, item.AwayGoals),
                        Over25 = UtilsOddOperations.GetOverUnder25(item.HomeGoals, item.AwayGoals),
                        Btts = UtilsOddOperations.GetBTTS(item.HomeGoals, item.AwayGoals)
                    });
                }
            }
            else if (typeMatchView == MatchViewTypeValues.HalfTime)
            {
                foreach (var item in jogos.OrderBy(x => x.Order))
                {
                    matchItems.Add(new MatchViewModel()
                    {
                        MatchId = item.MatchId,
                        Order = item.Order,
                        HomeTeam = teamService.GetTeamName(item.HomeTeamId),
                        HomeGoals = item.HomeGoals.ToString(),
                        AwayTeam = teamService.GetTeamName(item.AwayTeamId),
                        AwayGoals = item.AwayGoals.ToString(),
                        MatchResult = UtilsOddOperations.GetResultado(item.HalfHomeGoals.Value, item.HalfAwayGoals.Value),
                        MatchType = UtilsOddOperations.GetTipoJogo(item.HomeOdd, item.AwayOdd),
                        Over05 = UtilsOddOperations.GetOverUnder05(item.HalfHomeGoals.Value, item.HalfAwayGoals.Value),
                        Over15 = UtilsOddOperations.GetOverUnder15(item.HalfHomeGoals.Value, item.HalfAwayGoals.Value),                        
                        Btts = UtilsOddOperations.GetBTTS(item.HalfHomeGoals.Value, item.HalfAwayGoals.Value)
                    });
                }
            }
            else if (typeMatchView == MatchViewTypeValues.SecondTime)
            {
                foreach (var item in jogos.OrderBy(x => x.Order))
                {
                    matchItems.Add(new MatchViewModel()
                    {
                        MatchId = item.MatchId,
                        Order = item.Order,
                        HomeTeam = teamService.GetTeamName(item.HomeTeamId),
                        AwayTeam = teamService.GetTeamName(item.AwayTeamId),
                        HomeGoals = (item.HomeGoals - item.HalfHomeGoals.Value).ToString(),
                        AwayGoals = (item.AwayGoals - item.HalfAwayGoals.Value).ToString(),
                        MatchResult = UtilsOddOperations.GetResultado((item.HomeGoals - item.HalfHomeGoals.Value), (item.AwayGoals - item.HalfAwayGoals.Value)),
                        MatchType = UtilsOddOperations.GetTipoJogo(item.HomeOdd, item.AwayOdd),
                        Over05 = UtilsOddOperations.GetOverUnder05((item.HomeGoals - item.HalfHomeGoals.Value), (item.AwayGoals - item.HalfAwayGoals.Value)),
                        Over15 = UtilsOddOperations.GetOverUnder15((item.HomeGoals - item.HalfHomeGoals.Value), (item.AwayGoals - item.HalfAwayGoals.Value)),
                        Over25 = UtilsOddOperations.GetOverUnder25((item.HomeGoals - item.HalfHomeGoals.Value), (item.AwayGoals - item.HalfAwayGoals.Value)),
                        Btts = UtilsOddOperations.GetBTTS((item.HomeGoals - item.HalfHomeGoals.Value), (item.AwayGoals - item.HalfAwayGoals.Value))
                    });
                }
            }

        }

        private bool CheckMatches(List<Match> jogos)
        {
            foreach(var item in jogos)
            {
                if ((!item.HalfHomeGoals.HasValue || !item.HalfAwayGoals.HasValue) &&
                    typeMatchView != MatchViewTypeValues.AttackDefense)
                    return false;
            }

            return true;
        }

        private void ReloadGridMatches()
        {
            if(DataGridMatches.Visibility == Visibility.Visible && DataGridMatches.ItemsSource != null)
            {
                if (child.GetCompetitionValue() != null)
                    LoadGridWithCalculatedMatchesData(
                        child.GetCompetitionValue(),
                        child.GetFixtureValue(),
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

                Canvas.SetZIndex(DataGridMatches, 0);
            }
        }

        //PUBLIC
        public void LoadGridWithCalculatedMatchesData(int? compId, int? fixId, List<int> teamId)
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

                //get matches
                List<Match> mList = null;
                mList = matchService.GetMatchesFiltered(compId.Value, fixId, teamId).ToList();

                if(mList != null && mList.Count() > 0 && CheckMatches(mList))
                {
                    ObservableCollection<MatchViewModel> matchItems = new ObservableCollection<MatchViewModel>();
                    DataGridMatches.Dispatcher.BeginInvoke((Action)(() => DataGridMatches.ItemsSource = null));

                    LoadObservableCollection(matchItems, mList);

                    DataGridMatches.Dispatcher.BeginInvoke((Action)(() => DataGridMatches.ItemsSource = matchItems));

                    ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Collapsed));
                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Collapsed));
                    LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Collapsed));

                    if(typeMatchView == MatchViewTypeValues.AttackDefense)
                    {
                        DataGridMatches.Dispatcher.BeginInvoke((Action)(() => {
                            
                            // HEADERS
                            DataGridMatches.Columns[3].Header = "FTHG";
                            DataGridMatches.Columns[4].Header = "FTAG";
                            DataGridMatches.Columns[5].Header = "Resultado";

                            //VISIBILITY
                            DataGridMatches.Columns[6].Visibility = Visibility.Visible;
                            DataGridMatches.Columns[7].Visibility = Visibility.Visible;
                            DataGridMatches.Columns[8].Visibility = Visibility.Visible;
                            DataGridMatches.Columns[9].Visibility = Visibility.Visible;
                            DataGridMatches.Columns[11].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[13].Visibility = Visibility.Visible;
                        }));                   
                    }
                    else if (typeMatchView == MatchViewTypeValues.HalfTime)
                    {
                        DataGridMatches.Dispatcher.BeginInvoke((Action)(() => {

                            //HEADERS
                            DataGridMatches.Columns[3].Header = "HTHG";
                            DataGridMatches.Columns[4].Header = "HTAG";
                            DataGridMatches.Columns[5].Header = "HTR";

                            //VISIBILITY
                            DataGridMatches.Columns[6].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[7].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[8].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[9].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[11].Visibility = Visibility.Visible;
                            DataGridMatches.Columns[13].Visibility = Visibility.Collapsed;
                        }));
                    }
                    else if (typeMatchView == MatchViewTypeValues.SecondTime)
                    {
                        DataGridMatches.Dispatcher.BeginInvoke((Action)(() => {

                            //HEADERS
                            DataGridMatches.Columns[3].Header = "STHG";
                            DataGridMatches.Columns[4].Header = "STAG";
                            DataGridMatches.Columns[5].Header = "STR";

                            //VISIBILITY
                            DataGridMatches.Columns[6].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[7].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[8].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[9].Visibility = Visibility.Collapsed;
                            DataGridMatches.Columns[11].Visibility = Visibility.Visible;
                            DataGridMatches.Columns[13].Visibility = Visibility.Visible;
                        }));
                    }

                    DataGridMatches.Dispatcher.BeginInvoke((Action)(() => DataGridMatches.Visibility = Visibility.Visible));
                }
                else
                {
                    if(DataGridMatches.Visibility != Visibility.Collapsed)
                    {
                        DataGridMatches.Dispatcher.BeginInvoke((Action)(() => DataGridMatches.Visibility = Visibility.Collapsed));

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

            Canvas.SetZIndex(DataGridMatches, -1);
        }

        private void ButtonAttackDefense_Click(object sender, RoutedEventArgs e)
        {
            if(typeMatchView != MatchViewTypeValues.AttackDefense)
            {
                //Selection colors
                ButtonAttackDefense.Background = new SolidColorBrush(Color.FromRgb(170, 74, 59));
                ButtonHalfTime.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonSecondTime.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));

                typeMatchView = MatchViewTypeValues.AttackDefense;

                ReloadGridMatches();
            }            
        }

        private void ButtonHalfTime_Click(object sender, RoutedEventArgs e)
        {
            if (typeMatchView != MatchViewTypeValues.HalfTime)
            {
                //Selection colors
                ButtonHalfTime.Background = new SolidColorBrush(Color.FromRgb(170, 74, 59));
                ButtonAttackDefense.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonSecondTime.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));

                typeMatchView = MatchViewTypeValues.HalfTime;

                ReloadGridMatches();
            }        
        }

        private void ButtonSecondTime_Click(object sender, RoutedEventArgs e)
        {
            if (typeMatchView != MatchViewTypeValues.SecondTime)
            {
                //Selection colors
                ButtonSecondTime.Background = new SolidColorBrush(Color.FromRgb(170, 74, 59));
                ButtonAttackDefense.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));
                ButtonHalfTime.Background = new SolidColorBrush(Color.FromRgb(55, 54, 56));

                typeMatchView = MatchViewTypeValues.SecondTime;

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

                Canvas.SetZIndex(DataGridMatches, 0);
            }
        }        
    }
}
