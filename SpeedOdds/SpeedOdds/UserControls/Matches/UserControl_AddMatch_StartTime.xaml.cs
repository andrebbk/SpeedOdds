using SpeedOdds.Commons.Helpers;
using SpeedOdds.Services;
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

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_AddMatch_StartTime.xaml
    /// </summary>
    public partial class UserControl_AddMatch_StartTime : UserControl
    {
        private CompetitionService competitionService;

        public UserControl_AddMatch_StartTime()
        {
            InitializeComponent();
            competitionService = new CompetitionService();

            IniForm();
        }

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

                UtilsNotification.StopLoadingAnimation();
            }).Start();
        }

        private void ButtonLoadForm_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    class CompetitionComboModel
    {
        public int CompetitionId { get; set; }

        public string CompetitionName { get; set; }
    }
}
