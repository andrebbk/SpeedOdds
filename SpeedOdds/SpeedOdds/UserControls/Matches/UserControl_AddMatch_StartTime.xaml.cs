using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models.Shared;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
using SpeedOdds.Windows;

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_AddMatch_StartTime.xaml
    /// </summary>
    public partial class UserControl_AddMatch_StartTime : UserControl
    {
        private CompetitionService competitionService;
        private TeamService teamService;

        //model
        private ObservableCollection<MatchesModel> matchItems;

        //save in temp memory
        private int comboBoxCompetionId = 0;
        private int numericBoxFixtureId = 0;


        public UserControl_AddMatch_StartTime()
        {
            InitializeComponent();
            competitionService = new CompetitionService();
            teamService = new TeamService();
            matchItems = new ObservableCollection<MatchesModel>();

            IniForm();
        }


        //FUNCTIONS
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

                comboBoxCompetionId = 0;
                numericBoxFixtureId = 0;
                ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => ButtonLoadForm.Visibility = Visibility.Hidden));

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private bool IsMatchesListValid(int? nRows = null, bool teamsValid = true, int teamSide = 0, int matchId = 0)
        {
            if(!nRows.HasValue || nRows < 1)
            {
                NotificationHelper.notifier.ShowCustomMessage("Adiciona jogos para procederes com as odds!");
                return false;
            }

            if(!teamsValid && teamSide == 0 && matchId != 0)
            {
                NotificationHelper.notifier.ShowCustomMessage("No jogo nº " + matchId.ToString() +
                            " falta selecionar a equipa de casa");
                return false;
            }
            else if (!teamsValid && teamSide == 1 && matchId != 0)
            {
                NotificationHelper.notifier.ShowCustomMessage("No jogo nº " + matchId.ToString() +
                            " falta selecionar a equipa de fora");
                return false;
            }

            return true;

        }

        private void LoadCompetitionDataToGrid()
        {
            //GetCompTeams
            var tList = teamService.GetCompetitionTeams(comboBoxCompetionId);

            if (tList != null && tList.Count() > 0)
            {
                for (int i = 1; i < ((tList.Count()/2) + 1); i++)
                {
                    MatchesModel newMatch = new MatchesModel()
                    {
                        MatchViewId = i,
                        HomeGoals = 0,
                        AwayGoals = 0,
                        OddsHome = (decimal)1.5,
                        OddsDraw = (decimal)1.5,
                        OddsAway = (decimal)1.5
                    };

                    //Fill Teams
                    if (tList != null && tList.Count() > 0)
                        foreach (var item in tList) newMatch.TeamsList.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                    matchItems.Add(newMatch);
                }
            }                
        }


        //BUTTONS
        private void ButtonLoadForm_Click(object sender, RoutedEventArgs e)
        {
            //Verificações
            if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId == 0)
            {
                NotificationHelper.notifier.ShowCustomMessage("Selecione uma competição!");
                return;
            }

            Visibility auxGridVisibility = DataGridTeams.Visibility;
            int compId = ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId;
            comboBoxCompetionId = compId;
            numericBoxFixtureId = Convert.ToInt16(TextBoxFixture.Text);

            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                //GRID
                DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = null));
                matchItems = new ObservableCollection<MatchesModel>();


                //Logic to fill data grid
                LoadCompetitionDataToGrid();

                if (matchItems.Count() > 0)
                {
                    if (auxGridVisibility != Visibility.Visible)
                    {
                        //HIDE INITIAL INFO 
                        ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Collapsed));
                        LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Collapsed));
                        LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Collapsed));
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

                    ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => ButtonLoadForm.Visibility = Visibility.Hidden));
                }
                else
                {
                    //HIDE GRID
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.Visibility = Visibility.Collapsed));
                    ButtonSaveAllMatches.Dispatcher.BeginInvoke((Action)(() => ButtonSaveAllMatches.Visibility = Visibility.Hidden));
                    ButtonAddNewGame.Dispatcher.BeginInvoke((Action)(() => ButtonAddNewGame.Visibility = Visibility.Hidden));

                    //SHOW INITIAL INFO 
                    ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                    LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));

                    NotificationHelper.notifier.ShowCustomMessage("Não existem equipas participantes na competição selecionada!");                                       
                }                    

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private void ButtonSaveSingleMatch_Click(object sender, RoutedEventArgs e)
        {
            MatchesModel item = (sender as Button).DataContext as MatchesModel;
            if (item != null)
            {
                item.ButtonSaveVisibility = Visibility.Collapsed;
                item.ButtonRemoveVisibility = Visibility.Collapsed;
            }
        }

        private void ButtonSaveAllMatches_Click(object sender, RoutedEventArgs e)
        {
            //hold temp values
            int nRows = 0;
            int matchId = 0;
            int teamSide = 0;
            bool teamsValid = true;

            UtilsNotification.StartLoadingAnimation();

            //Has to be calculated this way
            Thread thread = new System.Threading.Thread(() => {

                nRows = DataGridTeams.ItemsSource.OfType<object>().Count();

                foreach (var item in DataGridTeams.ItemsSource)
                    if (((MatchesModel)item).HomeTeamId == 0)
                    {
                        matchId = ((MatchesModel)item).MatchViewId;
                        teamSide = 0;
                        teamsValid = false;
                        break;
                    }
                    else if (((MatchesModel)item).AwayTeamId == 0)
                    {
                        matchId = ((MatchesModel)item).MatchViewId;
                        teamSide = 1;
                        teamsValid = false;
                        break;
                    }

            });
            thread.Start();
            thread.Join();

            new Thread(() =>
            {
                if (IsMatchesListValid(nRows, teamsValid, teamSide, matchId))
                {
                    Thread.Sleep(5000);
                }

                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }

        private void ButtonAddNewGame_Click(object sender, RoutedEventArgs e)
        {
            int compId = ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId;

            new Thread(() =>
            {
                //Get teams
                var tList = teamService.GetCompetitionTeams(compId);

                MatchesModel newMatch = new MatchesModel()
                {
                    MatchViewId = matchItems.Count() + 1,
                    HomeGoals = 0,
                    AwayGoals = 0,
                    OddsHome = (decimal)1.5,
                    OddsDraw = (decimal)1.5,
                    OddsAway = (decimal)1.5
                };

                if (tList != null && tList.Count() > 0)
                    foreach (var item in tList) newMatch.TeamsList.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                Application.Current.Dispatcher.BeginInvoke((Action)(() => { matchItems.Add(newMatch); }));

                DataGridTeams.Dispatcher.BeginInvoke((Action)(() =>
                {
                    var border = VisualTreeHelper.GetChild(DataGridTeams, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }));

            }).Start();
        }

        private void ButtonRemoveSingleMatch_Click(object sender, RoutedEventArgs e)
        {
            MatchesModel item = (sender as Button).DataContext as MatchesModel;
            if (item != null)
            {
                ConfirmationWindow _popupConfirmation = new ConfirmationWindow("Tens a certeza que pretendes remover o jogo nº "
                    + item.MatchViewId.ToString());
                if (_popupConfirmation.ShowDialog() == true)
                {
                    int nRows = 0;
                    UtilsNotification.StartLoadingAnimation();

                    Thread thread = new System.Threading.Thread(() => {
                        nRows = DataGridTeams.ItemsSource.OfType<object>().Count();
                    });
                    thread.Start();
                    thread.Join();

                    new Thread(() =>
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                            matchItems.Remove(item);

                            for (int i = 0; i < (nRows - 1); i++)
                                matchItems[i].MatchViewId = i + 1;
                        }));                        

                        NotificationHelper.notifier.ShowCustomMessage("Jogo nº " + item.MatchViewId.ToString() + " removido com sucesso!");

                        UtilsNotification.StopLoadingAnimation();

                    }).Start();
                }                           
            }
        }


        //EVENT VALIDATIONS
        private void ComboBoxCompetition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(((CompetitionComboModel)ComboBoxCompetition.SelectedItem) != null && !String.IsNullOrWhiteSpace(TextBoxFixture.Text))
            {
                if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId == comboBoxCompetionId ||
                    ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId == 0)
                {
                    ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => {
                        if (ButtonLoadForm.Visibility != Visibility.Hidden)
                            ButtonLoadForm.Visibility = Visibility.Hidden;
                    }));
                }
                else if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId != 0)
                {
                    ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => {
                        if (ButtonLoadForm.Visibility != Visibility.Visible)
                            ButtonLoadForm.Visibility = Visibility.Visible;
                    }));
                }
            }                
        }

        private void TextBoxFixture_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!String.IsNullOrWhiteSpace(TextBoxFixture.Text) && ((CompetitionComboModel)ComboBoxCompetition.SelectedItem) != null)
            {
                if (Convert.ToInt16(TextBoxFixture.Text.Trim()) != numericBoxFixtureId)
                {
                    ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => {
                        if (ButtonLoadForm.Visibility != Visibility.Visible)
                            ButtonLoadForm.Visibility = Visibility.Visible;
                    }));
                }
                else if(((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId == comboBoxCompetionId)
                {
                    ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => {
                        if (ButtonLoadForm.Visibility != Visibility.Hidden)
                            ButtonLoadForm.Visibility = Visibility.Hidden;
                    }));
                }
            }
        }

        private void InputControlDigits_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                case Key.D1:
                case Key.NumPad1:
                case Key.D2:
                case Key.NumPad2:
                case Key.D3:
                case Key.NumPad3:
                case Key.D4:
                case Key.NumPad4:
                case Key.D5:
                case Key.NumPad5:
                case Key.D6:
                case Key.NumPad6:
                case Key.D7:
                case Key.NumPad7:
                case Key.D8:
                case Key.NumPad8:
                case Key.D9:
                case Key.NumPad9:
                case Key.Delete:
                case Key.Back:
                case Key.Right:
                case Key.Left:
                    e.Handled = false;
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void InputControlDigitsType2_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Include comma
            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                case Key.D1:
                case Key.NumPad1:
                case Key.D2:
                case Key.NumPad2:
                case Key.D3:
                case Key.NumPad3:
                case Key.D4:
                case Key.NumPad4:
                case Key.D5:
                case Key.NumPad5:
                case Key.D6:
                case Key.NumPad6:
                case Key.D7:
                case Key.NumPad7:
                case Key.D8:
                case Key.NumPad8:
                case Key.D9:
                case Key.NumPad9:
                case Key.OemComma:
                case Key.OemPeriod:
                case Key.Delete:
                case Key.Back:
                case Key.Right:
                case Key.Left:
                    e.Handled = false;
                    break;
                default:
                    e.Handled = true;
                    break;
            }
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

            ButtonSaveVisibility = Visibility.Visible;
            ButtonRemoveVisibility = Visibility.Visible;
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

        private Visibility buttonSaveVisibility;
        public Visibility ButtonSaveVisibility
        {
            get { return buttonSaveVisibility; }
            set
            {
                buttonSaveVisibility = value;
                OnPropertyChanged("ButtonSaveVisibility");
            }
        }

        private Visibility buttonRemoveVisibility;
        public Visibility ButtonRemoveVisibility
        {
            get { return buttonRemoveVisibility; }
            set
            {
                buttonRemoveVisibility = value;
                OnPropertyChanged("ButtonRemoveVisibility");
            }
        }
    }
}
