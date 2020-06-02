using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpeedOdds.UserControls.Teams
{
    public class TeamDataModel: ObservableObject
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public string CompetitionName { get; set; }

        public string SeasonName { get; set; }

        public string CreateDate { get; set; }

        private string favStarPath;
        public string FavStarPath
        {
            get { return favStarPath; }
            set
            {
                favStarPath = value;
                OnPropertyChanged("FavStarPath");
            }
        }
    }
}
