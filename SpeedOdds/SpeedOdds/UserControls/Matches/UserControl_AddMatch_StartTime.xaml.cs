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
using Xceed.Wpf.Toolkit;

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_AddMatch_StartTime.xaml
    /// </summary>
    public partial class UserControl_AddMatch_StartTime : UserControl
    {
        private UserControl_AddMatches _matchesParent;
        private CompetitionService competitionService;
        private TeamService teamService;
        private MatchService matchService;
        
        //model
        private ObservableCollection<MatchesModel> matchItems;

        //save in temp memory
        private int comboBoxCompetionId = 0;
        private int numericBoxFixtureId = 0;

        private bool IsUiTeamsLocked = false;


        public UserControl_AddMatch_StartTime(UserControl_AddMatches matchesParent)
        {
            InitializeComponent();
            _matchesParent = matchesParent;
            competitionService = new CompetitionService();
            teamService = new TeamService();
            matchService = new MatchService();
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
                        Order = i,
                        HomeTeamId = 0,
                        AwayTeamId = 0,
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
                    MatchesModel match = new MatchesModel()
                    {
                        Order = idx,
                        MatchId = itemMatch.MatchId,
                        CompetitionId = itemMatch.CompetitionId,
                        FixtureId = itemMatch.FixtureId,
                        HomeGoals = itemMatch.HomeGoals,
                        AwayGoals = itemMatch.AwayGoals,
                        OddsHome = itemMatch.HomeOdd,
                        OddsDraw = itemMatch.DrawOdd,
                        OddsAway = itemMatch.AwayOdd,
                        ButtonRemoveVisibility = Visibility.Visible
                    };

                    //Fill Teams
                    if (tList != null && tList.Count() > 0)
                        foreach (var item in tList) match.TeamsList.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                    //Select teams
                    match.HomeTeamId = match.TeamsList.IndexOf(match.TeamsList.Where(x => x.TeamId == itemMatch.HomeTeamId).FirstOrDefault());
                    match.AwayTeamId = match.TeamsList.IndexOf(match.TeamsList.Where(x => x.TeamId == itemMatch.AwayTeamId).FirstOrDefault());

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
            ButtonAddNewGame.Dispatcher.BeginInvoke((Action)(() => ButtonAddNewGame.IsEnabled = false));
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
            ButtonAddNewGame.Dispatcher.BeginInvoke((Action)(() => ButtonAddNewGame.IsEnabled = true));
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
            try{

                if(Int32.TryParse(TextBoxFixture.Text, out auxFixture))
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
                matchItems = new ObservableCollection<MatchesModel>();

                if(matchService.HasInitialMatchesForCompetitionFixture(comboBoxCompetionId, numericBoxFixtureId))
                {
                    //fill data grid with registered matches
                    LoadMatchesToGrid();
                }
                else
                {
                    //Logic to fill data grid (pre load)
                    LoadCompetitionDataToGrid();
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
            if (IsUiTeamsLocked)
                return;

            MatchesModel item = (sender as Button).DataContext as MatchesModel;
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
                else if(comboBoxCompetionId < 1)
                {
                    NotificationHelper.notifier.ShowCustomMessage("A competição está em falta!");
                    return;
                }
                else if (numericBoxFixtureId < 1)
                {
                    NotificationHelper.notifier.ShowCustomMessage("A jornada está em falta!");
                    return;
                }

                //Load necessary data
                item.CompetitionId = comboBoxCompetionId;
                item.FixtureId = numericBoxFixtureId;
                int homeTeamIdAux = item.HomeTeamId;
                int awayTeamIdAux = item.AwayTeamId;
                item.HomeTeamId = item.TeamsList[item.HomeTeamId].TeamId;
                item.AwayTeamId = item.TeamsList[item.AwayTeamId].TeamId;                

                new Thread(() =>
                {
                    UtilsNotification.StartLoadingAnimation();
                    LockAddMatchesUI();

                    //save data
                    var resultado = matchService.CreateOrUpdateMatch(item);

                    if (resultado != null)
                    {
                        //update model
                        item.MatchId = resultado.Item1;
                        item.ButtonSaveVisibility = Visibility.Collapsed;
                        item.ButtonRemoveVisibility = Visibility.Collapsed;
                        item.ImageDoneVisibility = Visibility.Visible;
                        item.HomeTeamId = homeTeamIdAux;
                        item.AwayTeamId = awayTeamIdAux;

                        string opType = resultado.Item2 == Commons.OperationTypeValues.Create ? "registado" :
                            resultado.Item2 == Commons.OperationTypeValues.Edit ? "editado" : "inalterado (erro)";
                        NotificationHelper.notifier.ShowCustomMessage("O jogo nº " + item.Order + " foi " + opType + " com sucesso!");
                    }
                    else
                        NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro ao registar o jogo nº " + item.Order);

                    UnlockAddMatchesUI();
                    UtilsNotification.StopLoadingAnimation();

                }).Start();                
            }
        }

        private void ButtonSaveAllMatches_Click(object sender, RoutedEventArgs e)
        {
            //hold temp values
            int nRows = 0;
            int order = 0;
            int teamSide = 0;
            bool teamsValid = true;

            UtilsNotification.StartLoadingAnimation();

            //Has to be calculated this way
            Thread thread = new System.Threading.Thread(() => {

                nRows = DataGridTeams.ItemsSource.OfType<object>().Count();

                foreach (var item in DataGridTeams.ItemsSource)
                    if (((MatchesModel)item).HomeTeamId == 0)
                    {
                        order = ((MatchesModel)item).Order;
                        teamSide = 0;
                        teamsValid = false;
                        break;
                    }
                    else if (((MatchesModel)item).AwayTeamId == 0)
                    {
                        order = ((MatchesModel)item).Order;
                        teamSide = 1;
                        teamsValid = false;
                        break;
                    }

            });
            thread.Start();
            thread.Join();

            new Thread(() =>
            {
                if (IsMatchesListValid(nRows, teamsValid, teamSide, order))
                {
                    LockAddMatchesUI();
                    UIRestartProcess();

                    //Save
                    foreach (var item in matchItems)
                    {
                        //Load necessary data
                        item.CompetitionId = comboBoxCompetionId;
                        item.FixtureId = numericBoxFixtureId;
                        int homeTeamIdAux = item.HomeTeamId;
                        int awayTeamIdAux = item.AwayTeamId;
                        item.HomeTeamId = item.TeamsList[item.HomeTeamId].TeamId;
                        item.AwayTeamId = item.TeamsList[item.AwayTeamId].TeamId;

                        //save data
                        var resultado = matchService.CreateOrUpdateMatch(item);

                        if (resultado != null)
                        {
                            //update model
                            item.MatchId = resultado.Item1;
                            item.ButtonSaveVisibility = Visibility.Collapsed;
                            item.ButtonRemoveVisibility = Visibility.Collapsed;
                            item.ImageDoneVisibility = Visibility.Visible;
                            item.HomeTeamId = homeTeamIdAux;
                            item.AwayTeamId = awayTeamIdAux;

                            string opType = resultado.Item2 == Commons.OperationTypeValues.Create ? "registado" :
                                resultado.Item2 == Commons.OperationTypeValues.Edit ? "editado" : "inalterado (erro)";
                            NotificationHelper.notifier.ShowCustomMessage("O jogo nº " + item.Order + " foi " + opType + " com sucesso!");
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("Ocorreu um erro ao registar o jogo nº " + item.Order);                        

                        Thread.Sleep(300);
                    }

                    NotificationHelper.notifier.ShowCustomMessage("Operação concluída!");
                    UnlockAddMatchesUI();
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
                    Order = matchItems.Count() + 1,
                    HomeTeamId = 0,
                    AwayTeamId = 0,
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
            if (IsUiTeamsLocked)
                return;

            MatchesModel item = (sender as Button).DataContext as MatchesModel;
            if (item != null)
            {
                ConfirmationWindow _popupConfirmation = new ConfirmationWindow("Tens a certeza que pretendes remover o jogo nº "
                    + item.Order.ToString());
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
                        if (item.MatchId.HasValue)
                        {
                            if (!matchService.RemoveMatch(item.MatchId.Value))
                            {
                                NotificationHelper.notifier.ShowCustomMessage("Erro ao remover jogo nº " + item.Order.ToString() + "!");
                                UtilsNotification.StopLoadingAnimation();
                                return;
                            }
                        }
                        
                        Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                            matchItems.Remove(item);

                            for (int i = 0; i < (nRows - 1); i++)
                                matchItems[i].Order = i + 1;
                        }));                        

                        NotificationHelper.notifier.ShowCustomMessage("Jogo nº " + item.Order.ToString() + " removido com sucesso!");

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

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(((IntegerUpDown)sender).Tag != null)
            {
                int nOrder = 0;
                if(Int32.TryParse(((IntegerUpDown)sender).Tag.ToString(), out nOrder))
                {
                    var item = matchItems.Where(x => x.Order == nOrder).FirstOrDefault();
                    if(item != null)
                    {
                        if (item.ButtonSaveVisibility != Visibility.Visible)
                            item.ButtonSaveVisibility = Visibility.Visible;

                        if (item.ImageDoneVisibility != Visibility.Collapsed)
                            item.ImageDoneVisibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void DecimalUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (((DecimalUpDown)sender).Tag != null)
            {
                int nOrder = 0;
                if (Int32.TryParse(((DecimalUpDown)sender).Tag.ToString(), out nOrder))
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
    
}
