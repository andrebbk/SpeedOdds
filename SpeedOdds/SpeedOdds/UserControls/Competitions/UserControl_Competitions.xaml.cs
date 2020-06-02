using BlurMessageBox;
using SpeedOdds.Commons.Helpers;
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

namespace SpeedOdds.UserControls.Competitions
{
    /// <summary>
    /// Interaction logic for UserControl_Competitions.xaml
    /// </summary>
    public partial class UserControl_Competitions : UserControl
    {
        private UserControl_MainContent _mainContent;
        private CompetitionService competitionService;
        private SeasonService seasonService;

        public UserControl_Competitions(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;

            ComboBoxSeason.IsEnabled = false;

            competitionService = new CompetitionService();
            seasonService = new SeasonService();

            LoadFormCompetitions();
        }

        private void LoadFormCompetitions()
        {
            new Thread(() =>
            {
                //Get competitions
                var competitionList = competitionService.GetCompetitions();

                DataGridCompetitions.Dispatcher.BeginInvoke((Action)(() => DataGridCompetitions.ItemsSource = null));
                ObservableCollection<CompetitionDataModel> compItems = new ObservableCollection<CompetitionDataModel>();

                if (competitionList != null && competitionList.Count() > 0)
                {
                    foreach (var item in competitionList)
                        compItems.Add(new CompetitionDataModel()
                        {
                            CompetitionId = item.CompetitionId,
                            CompetitionName = item.Name,
                            SeasonName = seasonService.GetSeasonName(item.SeasonId),
                            CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                        });

                    //BINDING
                    DataGridCompetitions.Dispatcher.BeginInvoke((Action)(() => DataGridCompetitions.ItemsSource = compItems));
                }


                //Load other data
                var seasonList = seasonService.GetSeasons();

                ObservableCollection<SeasonComboModel> seasonsBox = new ObservableCollection<SeasonComboModel>();
                foreach (var item in seasonList)
                    seasonsBox.Add(new SeasonComboModel()
                    {
                        SeasonId = item.SeasonId,
                        SeasonName = item.Name
                    });

                ComboBoxSeason.Dispatcher.BeginInvoke((Action)(() => ComboBoxSeason.ItemsSource = seasonsBox));
                ComboBoxSeason.Dispatcher.BeginInvoke((Action)(() => ComboBoxSeason.SelectedValue = seasonsBox.FirstOrDefault()));

                //Enable Ui
                ComboBoxSeason.Dispatcher.BeginInvoke((Action)(() => ComboBoxSeason.IsEnabled = true));
            }).Start();
        }

        private void LoadCompetitionsGrid()
        {
            new Thread(() =>
            {
                //Get competitions
                var competitionList = competitionService.GetCompetitions();

                DataGridCompetitions.Dispatcher.BeginInvoke((Action)(() => DataGridCompetitions.ItemsSource = null));
                ObservableCollection<CompetitionDataModel> compItems = new ObservableCollection<CompetitionDataModel>();

                if (competitionList != null && competitionList.Count() > 0)
                {
                    foreach (var item in competitionList)
                        compItems.Add(new CompetitionDataModel()
                        {
                            CompetitionId = item.CompetitionId,
                            CompetitionName = item.Name,
                            SeasonName = seasonService.GetSeasonName(item.SeasonId),
                            CreateDate = Utils.FormatDateTimeToGrid(item.CreateDate)
                        });

                    //BINDING
                    DataGridCompetitions.Dispatcher.BeginInvoke((Action)(() => DataGridCompetitions.ItemsSource = compItems));
                }

            }).Start();
        }


        //BUTTONS
        private void ButtonManageSeasons_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Seasons)) 
                _mainContent.WFAPContentContainer.Content = new UserControl_Seasons(_mainContent);
        }
        
        private void ButtonSaveCompetition_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TextBoxCompetitionName.Text) || TextBoxCompetitionName.Text.Length < 3)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Nome da competição inválido!");
                return;
            }

            if ((SeasonComboModel)ComboBoxSeason.SelectedValue == null)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Falta escolher a época!");
                return;
            }                  

            if(competitionService.AlreadyExistsCompetition(TextBoxCompetitionName.Text, ((SeasonComboModel)ComboBoxSeason.SelectedValue).SeasonId))
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Já existe uma competição idêntica!");
                return;
            }

            //Save new competition
            if (competitionService.CreateCompetition(TextBoxCompetitionName.Text, ((SeasonComboModel)ComboBoxSeason.SelectedValue).SeasonId))
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Competição criada com sucesso!");

                //Reset UI
                TextBoxCompetitionName.Clear();

                LoadCompetitionsGrid();
            }
            else
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Ocorreu um erro ao criar a Competição!");
        }

        private void ButtonRemoveCompetition_Click(object sender, RoutedEventArgs e)
        {
            CompetitionDataModel item = (sender as Button).DataContext as CompetitionDataModel;
            if (item != null)
            {
                if (this.MessageBoxShow("Tens a certeza que pretendes remover a competição '" + item.CompetitionName + "'?", "SpeedOdds",
                Buttons.YesNo, Icons.Warning, AnimateStyle.FadeIn) == System.Windows.Forms.DialogResult.Yes)
                {
                    new Thread(() =>
                    {
                        if (competitionService.CanDeleteById(item.CompetitionId))
                        {
                            if (competitionService.RemoveCompetition(item.CompetitionId))
                            {
                                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Competição removida com sucesso!");
                                LoadCompetitionsGrid();
                            }
                            else
                                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Erro ao remover competição!");
                        }
                        else
                            NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Não é possivel eliminar esta competição!\nContacte o Admin do sistema...");
                    }).Start();
                    
                }

            }
        }
    }

    class CompetitionDataModel
    {
        public int CompetitionId { get; set; }

        public string CompetitionName { get; set; }

        public string SeasonName { get; set; }

        public string CreateDate { get; set; }
    }

    class SeasonComboModel
    {
        public int SeasonId { get; set; }

        public string SeasonName { get; set; }
    }
}
