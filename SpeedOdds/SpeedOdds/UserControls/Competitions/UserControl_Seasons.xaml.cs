using SpeedOdds.Commons.Helpers;
using SpeedOdds.Notifications.CustomMessage;
using SpeedOdds.UserControls.MainContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for UserControl_Seasons.xaml
    /// </summary>
    public partial class UserControl_Seasons : UserControl
    {
        private UserControl_MainContent _mainContent;

        public UserControl_Seasons(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;

            LoadFormData();
        }

        private void LoadFormData()
        {
            //Get seasons

            //Load other data
            ObservableCollection<int> yearsList = new ObservableCollection<int>();
            int currentYear = DateTime.Now.Year;

            //10 years before
            for(int i = currentYear - 10; i < currentYear; i++)
                yearsList.Add(i);

            yearsList.Add(currentYear);

            //10 years after
            for (int i = currentYear + 1; i < currentYear + 10; i++)
                yearsList.Add(i);

            ComboBoxStartYear.ItemsSource = yearsList;
            ComboBoxStartYear.SelectedValue = currentYear;
            ComboBoxEndYear.ItemsSource = yearsList;
            ComboBoxEndYear.SelectedValue = currentYear;
        }

        //Buttons
        private void ButtonGoBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_Competitions(_mainContent);
        }

        private void ButtonSaveSeason_Click(object sender, RoutedEventArgs e)
        {
            if((int)ComboBoxStartYear.SelectedValue > (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Os anos da época são inválidos!");
                return;
            }

            if((int)ComboBoxStartYear.SelectedValue == (int)ComboBoxEndYear.SelectedValue)
            {
                NotificationHelper.notifier.ShowCustomMessage("SpeedOdds", "Os anos da época são inválidos!");
                return;
            }

        }
    }
}
