using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.UserControls.Rankings
{
    public class RankingsModel : ObservableObject
    {
        private string teamName;
        public string TeamName
        {
            get { return teamName; }
            set
            {
                teamName = value;
                OnPropertyChanged("TeamName");
            }
        }

        private string teamRating;
        public string TeamRating
        {
            get { return teamRating; }
            set
            {
                teamRating = value;
                OnPropertyChanged("TeamRating");
            }
        }

        private string teamRank;
        public string TeamRank
        {
            get { return teamRank; }
            set
            {
                teamRank = value;
                OnPropertyChanged("TeamRank");
            }
        }
    }
}
