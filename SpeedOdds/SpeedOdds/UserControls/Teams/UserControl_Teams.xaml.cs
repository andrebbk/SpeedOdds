using BlurMessageBox;
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

namespace SpeedOdds.UserControls.Teams
{
    /// <summary>
    /// Interaction logic for UserControl_Teams.xaml
    /// </summary>
    public partial class UserControl_Teams : UserControl
    {
        private UserControl_MainContent _mainContent;
        private CompetitionService competitionService;
        private TeamService teamService;

        //Pagination
        private int pagNumber = 1;
        private int pagLastNumber = 1;
        private int IPP = 30;

        public UserControl_Teams(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            ComboBoxCompetition.IsEnabled = false;
            _mainContent = mainContent;

            teamService = new TeamService();
            competitionService = new CompetitionService();

            LoadConfigurationForPagination();
            LoadFormTeams();
            IniSearchControls();
        }

        private void LoadFormTeams()
        {
            new Thread(() =>
            {
                //Get competitions
                var teamList = teamService.GetTeams().OrderBy(x => x.Name).Skip((pagNumber - 1) * IPP).Take(IPP);

                DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = null));
                ObservableCollection<TeamDataModel> teamItems = new ObservableCollection<TeamDataModel>();

                if (teamList != null && teamList.Count() > 0)
                {
                    foreach (var item in teamList)
                        teamItems.Add(new TeamDataModel()
                        {
                            TeamId = item.TeamId,
                            TeamName = item.Name,
                            CompetitionName = competitionService.GetCompetitionName(item.CompetitionId),
                            SeasonName = competitionService.GetCompetitionSeasonName(item.CompetitionId),
                            FavStarPath = item.IsFavorite? "pack://application:,,/Resources/ImageFiles/unnamed_star.png" : 
                            "pack://application:,,/Resources/ImageFiles/black_tilde_arrow.png",
                            CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                        });

                    //BINDING
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = teamItems));                    
                }

                //PAGINATION TEXT
                ShowPaginationText();


                //Load other data
                var competitionList = competitionService.GetCompetitions();

                ObservableCollection<CompetitionComboModel> compsBox = new ObservableCollection<CompetitionComboModel>();
                foreach (var item in competitionList)
                    compsBox.Add(new CompetitionComboModel()
                    {
                        CompetitionId = item.CompetitionId,
                        CompetitionName = item.Name + " - " + competitionService.GetCompetitionSeasonName(item.CompetitionId)
                    });

                ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.ItemsSource = compsBox));
                ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.SelectedValue = compsBox.FirstOrDefault()));

                //Enable Ui
                ComboBoxCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxCompetition.IsEnabled = true));
            }).Start();
        }

        private void LoadTeamsGrid()
        {
            new Thread(() =>
            {
                LoadConfigurationForPagination();

                //Get competitions
                var teamList = teamService.GetTeams().OrderBy(x => x.Name).Skip((pagNumber - 1) * IPP).Take(IPP);

                DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = null));
                ObservableCollection<TeamDataModel> teamItems = new ObservableCollection<TeamDataModel>();

                if (teamList != null && teamList.Count() > 0)
                {
                    foreach (var item in teamList)
                        teamItems.Add(new TeamDataModel()
                        {
                            TeamId = item.TeamId,
                            TeamName = item.Name,
                            CompetitionName = competitionService.GetCompetitionName(item.CompetitionId),
                            SeasonName = competitionService.GetCompetitionSeasonName(item.CompetitionId),
                            FavStarPath = item.IsFavorite ? "pack://application:,,/Resources/ImageFiles/unnamed_star.png" :
                            "pack://application:,,/Resources/ImageFiles/black_tilde_arrow.png",
                            CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                        });

                    //BINDING
                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = teamItems));
                }

                //PAGINATION TEXT
                ShowPaginationText();

            }).Start();
        }

        private void IniSearchControls()
        {
            new Thread(() =>
            {
                TextBoxFilterValue.Dispatcher.BeginInvoke((Action)(() => TextBoxFilterValue.Clear()));

                //Load data
                var competitionList = competitionService.GetCompetitions();

                ObservableCollection<CompetitionComboModel> compsBox = new ObservableCollection<CompetitionComboModel>();
                compsBox.Add(new CompetitionComboModel()
                {
                    CompetitionId = 0,
                    CompetitionName = ""
                });

                foreach (var item in competitionList)
                    compsBox.Add(new CompetitionComboModel()
                    {
                        CompetitionId = item.CompetitionId,
                        CompetitionName = item.Name + " - " + competitionService.GetCompetitionSeasonName(item.CompetitionId)
                    });

                ComboBoxFilterCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxFilterCompetition.ItemsSource = compsBox));
                ComboBoxFilterCompetition.Dispatcher.BeginInvoke((Action)(() => ComboBoxFilterCompetition.SelectedValue = compsBox.FirstOrDefault()));

                CheckBoxFilterIsFavoriteYes.Dispatcher.BeginInvoke((Action)(() => CheckBoxFilterIsFavoriteYes.IsChecked = false));
                CheckBoxFilterIsFavoriteNo.Dispatcher.BeginInvoke((Action)(() => CheckBoxFilterIsFavoriteNo.IsChecked = false));

            }).Start();
        }

        private void LoadConfigurationForPagination(int? total = null)
        {
            //Itens por página
            IPP = 30;

            if (total.HasValue)
            {
                //Total de páginas
                pagLastNumber = total.Value / IPP;
                if (total.Value % IPP != 0)
                    pagLastNumber += 1;
            }
            else
            {
                //Total de páginas
                pagLastNumber = teamService.GetTeamsNumber() / IPP;
                if (teamService.GetTeamsNumber() % IPP != 0)
                    pagLastNumber += 1;
            }
            

            if (pagLastNumber == 0) pagLastNumber = 1;
        }

        private void ShowPaginationText()
        {
            string texto = pagLastNumber == 0 ? "0" : pagNumber.ToString();
            texto += " de " + pagLastNumber + ((pagLastNumber > 1 || pagLastNumber == 0) ? " páginas" : " página");

            TextBlock_PaginationText.Dispatcher.BeginInvoke((Action)(
                  () => TextBlock_PaginationText.Text = texto));
        }


        //BUTTONS
        private void ButtonSaveTeam_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TextBoxTeamName.Text) || TextBoxTeamName.Text.Length < 3)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Nome da equipa inválido!");
                return;
            }

            if ((CompetitionComboModel)ComboBoxCompetition.SelectedValue == null)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Falta escolher a competição!");
                return;
            }

            if (teamService.AlreadyExistsTeam(TextBoxTeamName.Text, ((CompetitionComboModel)ComboBoxCompetition.SelectedValue).CompetitionId))
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Já existe uma equipa idêntica!");
                return;
            }

            //Save new team
            if (teamService.CreateTeam(TextBoxTeamName.Text, ((CompetitionComboModel)ComboBoxCompetition.SelectedValue).CompetitionId, 
                CheckBoxIsFavorite.IsChecked.HasValue? CheckBoxIsFavorite.IsChecked.Value : false))
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Equipa criada com sucesso!");

                //Reset UI
                TextBoxTeamName.Clear();
                CheckBoxIsFavorite.IsChecked = false;

                LoadTeamsGrid();
            }
            else
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Ocorreu um erro ao criar a Equipa!");
        }

        private void ButtonRemoveTeam_Click(object sender, RoutedEventArgs e)
        {
            TeamDataModel item = (sender as Button).DataContext as TeamDataModel;
            if (item != null)
            {
                if (this.MessageBoxShow("Tens a certeza que pretendes remover a equipa '" + item.TeamName + "'?", "SpeedOdds",
                Buttons.YesNo, Icons.Warning, AnimateStyle.FadeIn) == System.Windows.Forms.DialogResult.Yes)
                {
                    new Thread(() =>
                    {
                        if (teamService.RemoveTeam(item.TeamId))
                        {
                            NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Equipa removida com sucesso!");
                            LoadTeamsGrid();
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Erro ao remover equipa!");
                    }).Start();
                    
                }

            }
        }

        private void ButtonFav_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TeamDataModel item = (sender as Image).DataContext as TeamDataModel;
            if (item != null)
            {
                string msg = "";
                bool flagFav = true;

                if (item.FavStarPath.Contains("unnamed_star"))
                {
                    msg = "Tens a certeza que pretendes remover a equipa " + item.TeamName + " dos favoritos?";
                    flagFav = false;
                }
                else
                {
                    msg = "Tens a certeza que pretendes adicionar a equipa " + item.TeamName + " aos favoritos?";
                    flagFav = true;
                }                    

                if (this.MessageBoxShow(msg, "SpeedOdds",
                Buttons.YesNo, Icons.Warning, AnimateStyle.FadeIn) == System.Windows.Forms.DialogResult.Yes)
                {
                    new Thread(() =>
                    {
                        if (teamService.ChangeFavoriteValue(item.TeamId, flagFav))
                        {
                            NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Favoritismo do clube alterado com sucesso!");
                            LoadTeamsGrid();
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Erro ao mudar o favoritismo do clube/nContacte o Admin do sistema...");
                    }).Start();
                }
            }
        }


        //PAGINATION
        private void Button_Pag_Left_Click(object sender, RoutedEventArgs e)
        {
            if ((pagNumber - 1) >= 1)
            {
                pagNumber -= 1;
                LoadTeamsGrid();
            }
        }

        private void Button_Pag_First_Click(object sender, RoutedEventArgs e)
        {
            if (pagNumber != 1)
            {
                pagNumber = 1;
                LoadTeamsGrid();
            }
        }

        private void Button_Pag_Last_Click(object sender, RoutedEventArgs e)
        {
            if (pagNumber != pagLastNumber)
            {
                pagNumber = pagLastNumber;
                LoadTeamsGrid();
            }
        }

        private void Button_Pag_Right_Click(object sender, RoutedEventArgs e)
        {
            if ((pagNumber + 1) <= pagLastNumber)
            {
                pagNumber += 1;
                LoadTeamsGrid();
            }
        }

        private void ButtonFilterTeams_Click(object sender, RoutedEventArgs e)
        {
            //prepare data
            int? compId = ((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId != 0 ?
                (int?)((CompetitionComboModel)ComboBoxFilterCompetition.SelectedValue).CompetitionId : null;

            bool? isFav = CheckBoxFilterIsFavoriteYes.IsChecked;
            if (!CheckBoxFilterIsFavoriteYes.IsChecked.Value && !CheckBoxFilterIsFavoriteNo.IsChecked.Value) isFav = null;

            new Thread(() =>
            {                      
                string txtFilter = String.Empty;
                TextBoxFilterValue.Dispatcher.Invoke((Action)(() => { txtFilter = TextBoxFilterValue.Text; }));

                //Get competitions
                var dados = teamService.GetFilteredTeams(txtFilter, compId, isFav);

                if(dados != null)
                {
                    LoadConfigurationForPagination(dados.Item2);

                    var teamList = dados.Item1.OrderBy(x => x.Name).Skip((pagNumber - 1) * IPP).Take(IPP);

                    DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = null));
                    ObservableCollection<TeamDataModel> teamItems = new ObservableCollection<TeamDataModel>();

                    if (teamList != null && teamList.Count() > 0)
                    {
                        foreach (var item in teamList)
                            teamItems.Add(new TeamDataModel()
                            {
                                TeamId = item.TeamId,
                                TeamName = item.Name,
                                CompetitionName = competitionService.GetCompetitionName(item.CompetitionId),
                                SeasonName = competitionService.GetCompetitionSeasonName(item.CompetitionId),
                                FavStarPath = item.IsFavorite ? "pack://application:,,/Resources/ImageFiles/unnamed_star.png" :
                                "pack://application:,,/Resources/ImageFiles/black_tilde_arrow.png",
                                CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                            });

                        //BINDING
                        DataGridTeams.Dispatcher.BeginInvoke((Action)(() => DataGridTeams.ItemsSource = teamItems));
                    }

                    //PAGINATION TEXT
                    ShowPaginationText();
                }                

            }).Start();
        }      

        private void CheckBoxFilterIsFavoriteYes_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxFilterIsFavoriteNo.IsChecked.HasValue && CheckBoxFilterIsFavoriteNo.IsChecked.Value)
                CheckBoxFilterIsFavoriteNo.IsChecked = false;
        }

        private void CheckBoxFilterIsFavoriteNo_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxFilterIsFavoriteYes.IsChecked.HasValue && CheckBoxFilterIsFavoriteYes.IsChecked.Value)
                CheckBoxFilterIsFavoriteYes.IsChecked = false;
        }
    }


    class CompetitionComboModel
    {
        public int CompetitionId { get; set; }

        public string CompetitionName { get; set; }
    }
}
