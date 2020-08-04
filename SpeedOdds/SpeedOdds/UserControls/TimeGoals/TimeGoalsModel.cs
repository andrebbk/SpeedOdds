using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.UserControls.TimeGoals
{
    public class TimeGoalsModel : ObservableObject
    {
        private int? timeGoalsId;
        public int? TimeGoalsId
        {
            get { return timeGoalsId; }
            set
            {
                timeGoalsId = value;
                OnPropertyChanged("TimeGoalsId");
            }
        }

        private int? competitionId;
        public int? CompetitionId
        {
            get { return competitionId; }
            set
            {
                competitionId = value;
                OnPropertyChanged("CompetitionId");
            }
        }

        private int? teamId;
        public int? TeamId
        {
            get { return teamId; }
            set
            {
                teamId = value;
                OnPropertyChanged("TeamId");
            }
        }

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

        private int totalGoals;
        public int TotalGoals
        {
            get { return totalGoals; }
            set
            {
                totalGoals = value;
                OnPropertyChanged("TotalGoals");
            }
        }

        private int goal15;
        public int Goal15
        {
            get { return goal15; }
            set
            {
                goal15 = value;
                OnPropertyChanged("Goal15");
            }
        }

        private string goal15P;
        public string Goal15P
        {
            get { return goal15P; }
            set
            {
                goal15P = value;
                OnPropertyChanged("Goal15P");
            }
        }

        private int goal30;
        public int Goal30
        {
            get { return goal30; }
            set
            {
                goal30 = value;
                OnPropertyChanged("Goal30");
            }
        }

        private string goal30P;
        public string Goal30P
        {
            get { return goal30P; }
            set
            {
                goal30P = value;
                OnPropertyChanged("Goal30P");
            }
        }

        private int goal45;
        public int Goal45
        {
            get { return goal45; }
            set
            {
                goal45 = value;
                OnPropertyChanged("Goal45");
            }
        }

        private string goal45P;
        public string Goal45P
        {
            get { return goal45P; }
            set
            {
                goal45P = value;
                OnPropertyChanged("Goal45P");
            }
        }

        private int goal60;
        public int Goal60
        {
            get { return goal60; }
            set
            {
                goal60 = value;
                OnPropertyChanged("Goal60");
            }
        }

        private string goal60P;
        public string Goal60P
        {
            get { return goal60P; }
            set
            {
                goal60P = value;
                OnPropertyChanged("Goal60P");
            }
        }

        private int goal75;
        public int Goal75
        {
            get { return goal75; }
            set
            {
                goal75 = value;
                OnPropertyChanged("Goal75");
            }
        }

        private string goal75P;
        public string Goal75P
        {
            get { return goal75P; }
            set
            {
                goal75P = value;
                OnPropertyChanged("Goal75P");
            }
        }

        private int goal90;
        public int Goal90
        {
            get { return goal90; }
            set
            {
                goal90 = value;
                OnPropertyChanged("Goal90");
            }
        }

        private string goal90P;
        public string Goal90P
        {
            get { return goal90P; }
            set
            {
                goal90P = value;
                OnPropertyChanged("Goal90P");
            }
        }
    }
}
