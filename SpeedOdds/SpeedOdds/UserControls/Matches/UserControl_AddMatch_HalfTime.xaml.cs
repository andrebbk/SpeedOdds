using SpeedOdds.Commons.Helpers;
using SpeedOdds.Models.Shared;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.Services;
using SpeedOdds.Windows;
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
using Xceed.Wpf.Toolkit;

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_AddMatch_HalfTime.xaml
    /// </summary>
    public partial class UserControl_AddMatch_HalfTime : UserControl
    {
        private UserControl_AddMatches _matchesParent;
        private CompetitionService competitionService;
        private TeamService teamService;
        private MatchService matchService;

        //model
        private ObservableCollection<MatchesHFModel> matchItems;

        //save in temp memory
        private int comboBoxCompetionId = 0;
        private int numericBoxFixtureId = 0;

        private bool IsUiTeamsLocked = false;


        public UserControl_AddMatch_HalfTime(UserControl_AddMatches matchesParent)
        {
            InitializeComponent();
            _matchesParent = matchesParent;
            competitionService = new CompetitionService();
            teamService = new TeamService();
            matchService = new MatchService();
            matchItems = new ObservableCollection<MatchesHFModel>();

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
            if (!nRows.HasValue || nRows < 1)
            {
                NotificationHelper.notifier.ShowCustomMessage("Adiciona jogos para procederes com as odds!");
                return false;
            }

            if (!teamsValid && teamSide == 0 && matchId != 0)
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

        private void LoadMatchesToGrid()
        {
            //GetCompTeams
            var tList = teamService.GetCompetitionTeams(comboBoxCompetionId);

            //GetMatches
            var mList = matchService.GetMatchesByCompetitionFixture(comboBoxCompetionId, numericBoxFixtureId);

            if (tList != null && tList.Count() > 0 && mList != null && mList.Count() > 0)
            {
                int idx = 1;
                foreach (var itemMatch in mList.OrderBy(x => x.Order))
                {
                    MatchesHFModel match = new MatchesHFModel()
                    {
                        Order = idx,
                        MatchId = itemMatch.MatchId,
                        HomeTeamId = itemMatch.HomeTeamId,
                        HomeTeam = teamService.GetTeamName(itemMatch.HomeTeamId),
                        HomeGoals = itemMatch.HalfHomeGoals.HasValue? itemMatch.HalfHomeGoals.Value : itemMatch.HomeGoals,   
                        MinHomeGoals = itemMatch.HomeGoals,
                        AwayTeamId = itemMatch.AwayTeamId,
                        AwayTeam = teamService.GetTeamName(itemMatch.AwayTeamId),
                        AwayGoals = itemMatch.HalfAwayGoals.HasValue ? itemMatch.HalfAwayGoals.Value : itemMatch.AwayGoals,
                        MinAwayGoals = itemMatch.AwayGoals,
                    };

                    matchItems.Add(match);
                    idx++;
                }
            }
        }

        private void LockAddMatchesUI()
        {
            //Lock parent UI controls
            _matchesParent.LockMatchesUI();

            ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => {
                if (ButtonLoadForm.Visibility == Visibility.Visible)
                    ButtonLoadForm.IsEnabled = false;
            }));
            ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.IsEnabled = false));
            TextBoxFixture.Dispatcher.BeginInvoke((Action)(() => TextBoxFixture.IsEnabled = false));
            ButtonSaveAllMatches.Dispatcher.BeginInvoke((Action)(() => ButtonSaveAllMatches.IsEnabled = false));
            IsUiTeamsLocked = true;
        }

        private void UnlockAddMatchesUI()
        {
            //Unlock parent UI controls
            _matchesParent.UnlockMatchesUI();

            ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => {
                if (!ButtonLoadForm.IsEnabled)
                    ButtonLoadForm.IsEnabled = true;
            }));
            ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.IsEnabled = true));
            TextBoxFixture.Dispatcher.BeginInvoke((Action)(() => TextBoxFixture.IsEnabled = true));
            ButtonSaveAllMatches.Dispatcher.BeginInvoke((Action)(() => ButtonSaveAllMatches.IsEnabled = true));
            IsUiTeamsLocked = false;
        }

        private void UIRestartProcess()
        {
            foreach (var item in matchItems)
            {
                if (item.ImageDoneVisibility == Visibility.Visible)
                    item.ImageDoneVisibility = Visibility.Collapsed;
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

            int auxFixture = 0;
            try
            {

                if (Int32.TryParse(TextBoxFixture.Text, out auxFixture))
                {
                    if (auxFixture < 1)
                    {
                        NotificationHelper.notifier.ShowCustomMessage("Selecione uma jornada!");
                        return;
                    }
                }
                else
                {
                    NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro com a jornada!");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro com a jornada!");
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
                matchItems = new ObservableCollection<MatchesHFModel>();

                if (matchService.HasInitialMatchesForCompetitionFixture(comboBoxCompetionId, numericBoxFixtureId))
                {
                    //fill data grid with registered matches
                    LoadMatchesToGrid();
                }

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
                    }

                    ButtonLoadForm.Dispatcher.BeginInvoke((Action)(() => ButtonLoadForm.Visibility = Visibility.Hidden));
                }
                else
                {
                    //HIDE GRID
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.Visibility = Visibility.Collapsed));
                    ButtonSaveAllMatches.Dispatcher.BeginInvoke((Action)(() => ButtonSaveAllMatches.Visibility = Visibility.Hidden));

                    //SHOW INITIAL INFO 
                    ImageLogo.Dispatcher.BeginInvoke((Action)(() => ImageLogo.Visibility = Visibility.Visible));
                    LabelInfo.Dispatcher.BeginInvoke((Action)(() => LabelInfo.Visibility = Visibility.Visible));
                    LabelExtraInfo.Dispatcher.BeginInvoke((Action)(() => LabelExtraInfo.Visibility = Visibility.Visible));

                    NotificationHelper.notifier.ShowCustomMessage("Ainda não foram registados jogos para essa jornada!");
                }

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private void ButtonSaveSingleMatch_Click(object sender, RoutedEventArgs e)
        {
            if (IsUiTeamsLocked)
                return;

            MatchesHFModel item = (sender as Button).DataContext as MatchesHFModel;
            if (item != null)
            {
                //VALIDATIONS
                if (item.HomeTeamId == 0)
                {
                    NotificationHelper.notifier.ShowCustomMessage("No jogo nº " + item.Order.ToString() +
                                " falta selecionar a equipa de casa");
                    return;
                }
                else if (item.AwayTeamId == 0)
                {
                    NotificationHelper.notifier.ShowCustomMessage("No jogo nº " + item.Order.ToString() +
                                " falta selecionar a equipa de fora");
                    return;
                }

                new Thread(() =>
                {
                    UtilsNotification.StartLoadingAnimation();
                    LockAddMatchesUI();

                    //save data
                    var resultado = matchService.UpdateMatchHalfTime(item.MatchId, item.HomeGoals, item.AwayGoals);

                    if (resultado != Commons.OperationTypeValues.Error)
                    {
                        //update model
                        item.ButtonSaveVisibility = Visibility.Collapsed;
                        item.ImageDoneVisibility = Visibility.Visible;

                        string opType = resultado == Commons.OperationTypeValues.Create ? "registada" :
                            resultado == Commons.OperationTypeValues.Edit ? "editada" : "inalterada (erro)";
                        NotificationHelper.notifier.ShowCustomMessage("2ª parte do jogo nº " + item.Order + " foi " + opType + " com sucesso!");
                    }
                    else
                        NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro ao registar a 2ª parte do jogo nº " + item.Order);

                    UnlockAddMatchesUI();
                    UtilsNotification.StopLoadingAnimation();

                }).Start();
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
                    if (((MatchesHFModel)item).HomeTeamId == 0)
                    {
                        matchId = ((MatchesHFModel)item).Order;
                        teamSide = 0;
                        teamsValid = false;
                        break;
                    }
                    else if (((MatchesHFModel)item).AwayTeamId == 0)
                    {
                        matchId = ((MatchesHFModel)item).Order;
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
                    LockAddMatchesUI();
                    UIRestartProcess();

                    //Save
                    foreach (var item in matchItems)
                    {
                        //save data
                        var resultado = matchService.UpdateMatchHalfTime(item.MatchId, item.HomeGoals, item.AwayGoals);

                        if (resultado != Commons.OperationTypeValues.Error)
                        {
                            //update model
                            item.ButtonSaveVisibility = Visibility.Collapsed;
                            item.ImageDoneVisibility = Visibility.Visible;

                            string opType = resultado == Commons.OperationTypeValues.Create ? "registada" :
                            resultado == Commons.OperationTypeValues.Edit ? "editada" : "inalterada (erro)";
                            NotificationHelper.notifier.ShowCustomMessage("2ª parte do jogo nº " + item.Order + " foi " + opType + " com sucesso!");
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro ao registar a 2ª parte do jogo nº " + item.Order);

                        Thread.Sleep(300);
                    }

                    NotificationHelper.notifier.ShowCustomMessage("Operação concluída!");
                    UnlockAddMatchesUI();
                }

                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }               


        //EVENT VALIDATIONS
        private void ComboBoxCompetition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem) != null && !String.IsNullOrWhiteSpace(TextBoxFixture.Text))
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
                else if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId == comboBoxCompetionId)
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

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (((IntegerUpDown)sender).Tag != null)
            {
                int nOrder = 0;
                if (Int32.TryParse(((IntegerUpDown)sender).Tag.ToString(), out nOrder))
                {
                    var item = matchItems.Where(x => x.Order == nOrder).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.ButtonSaveVisibility != Visibility.Visible)
                            item.ButtonSaveVisibility = Visibility.Visible;

                        if (item.ImageDoneVisibility != Visibility.Collapsed)
                            item.ImageDoneVisibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }

    public class MatchesHFModel : ObservableObject
    {
        public MatchesHFModel()
        {
            homeTeamId = 0;
            awayTeamId = 0;

            homeGoals = 0;
            awayGoals = 0;

            ButtonSaveVisibility = Visibility.Visible;
            ImageDoneVisibility = Visibility.Collapsed;
        }

        private int matchId;
        public int MatchId
        {
            get { return matchId; }
            set
            {
                matchId = value;
                OnPropertyChanged("MatchId");
            }
        }

        private int order;
        public int Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
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

        private string homeTeam;
        public string HomeTeam
        {
            get { return homeTeam; }
            set
            {
                homeTeam = value;
                OnPropertyChanged("HomeTeam");
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

        private string awayTeam;
        public string AwayTeam
        {
            get { return awayTeam; }
            set
            {
                awayTeam = value;
                OnPropertyChanged("AwayTeam");
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

        private int minHomeGoals;
        public int MinHomeGoals
        {
            get { return minHomeGoals; }
            set
            {
                minHomeGoals = value;
                OnPropertyChanged("MinHomeGoals");
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

        private int minAwayGoals;
        public int MinAwayGoals
        {
            get { return minAwayGoals; }
            set
            {
                minAwayGoals = value;
                OnPropertyChanged("MinAwayGoals");
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

        private Visibility imageDoneVisibility;
        public Visibility ImageDoneVisibility
        {
            get { return imageDoneVisibility; }
            set
            {
                imageDoneVisibility = value;
                OnPropertyChanged("ImageDoneVisibility");
            }
        }
    }
}
