using SpeedOdds.UserControls.MainContent;
using System;
using System.Collections.Generic;
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

namespace SpeedOdds.UserControls.TimeGoals
{
    public partial class UserControl_TimeGoals : UserControl
    {
        private UserControl_MainContent _mainContent;

        public UserControl_TimeGoals(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            mainContent = _mainContent;
        }
    }
}
