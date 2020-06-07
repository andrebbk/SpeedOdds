using SpeedOdds.Models.Shared;
using SpeedOdds.Services;
using SpeedOdds.UserControls.MainContent;
using SpeedOdds.UserControls.Matches;
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

namespace SpeedOdds.UserControls.DrawableMenu
{
    /// <summary>
    /// Interaction logic for UserControl_DrawableMenuMatches.xaml
    /// </summary>
    public partial class UserControl_DrawableMenuMatches : UserControl
    {
        private UserControl_MainContent _mainContent;
        private CompetitionService competitionService;
        private TeamService teamService;

        public UserControl_DrawableMenuMatches(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
            competitionService = new CompetitionService();
            teamService = new TeamService();

            InitFilterForm();
        }

        //FUNCTIONS
        public void InitFilterForm()
        {
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

            ComboBoxCompetition.ItemsSource = compsBox;
            ComboBoxCompetition.SelectedValue = compsBox.FirstOrDefault();
  
            ComboBoxTeam.IsEnabled = false;
        }


        //BUTTONS
        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_AddMatches(_mainContent);
        }

        private void ButtonApplyFilters_Click(object sender, RoutedEventArgs e)
        {

        }


        //EVENT VALIDATIONS
        private void ComboBoxCompetition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem) != null)
            {
                if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId != 0)
                {
                    int compId = ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId;
                    new Thread(() =>
                    {
                        //Load teams
                        var teamList = teamService.GetCompetitionTeams(compId);

                        if (teamList != null && teamList.Count() > 0)
                        {
                            ObservableCollection<TeamComboModel> teamsBox = new ObservableCollection<TeamComboModel>();
                            teamsBox.Add(new TeamComboModel()
                            {
                                TeamId = 0,
                                TeamName = "Selecionar equipa"
                            });                           

                            foreach(var item in teamList) teamsBox.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.ItemsSource = teamsBox));
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.SelectedValue = teamsBox.FirstOrDefault()));                            

                            //Active combobox
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.IsEnabled = true));
                        }
                        else
                        {
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.ItemsSource = null));
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.IsEnabled = false));
                        }

                    }).Start();                    
                }
                else
                    ComboBoxTeam.IsEnabled = false;
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
    }
}
