using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpeedOdds.UserControls.Matches
{
    public class MatchesModel : ObservableObject
    {
        public MatchesModel()
        {
            homeTeamId = 0;
            awayTeamId = 0;

            teamsList = new ObservableCollection<TeamComboModel>();
            teamsList.Add(new TeamComboModel()
            {
                TeamId = 0,
                TeamName = "Selecionar Equipa"
            });

            homeGoals = 0;
            awayGoals = 0;

            oddsHome = 0;
            oddsDraw = 0;
            oddsAway = 0;

            ButtonSaveVisibility = Visibility.Visible;
            ButtonRemoveVisibility = Visibility.Visible;
            ImageDoneVisibility = Visibility.Collapsed;
        }

        private int? matchId;
        public int? MatchId
        {
            get { return matchId; }
            set
            {
                matchId = value;
                OnPropertyChanged("MatchId");
            }
        }

        private int competitionId;
        public int CompetitionId
        {
            get { return competitionId; }
            set
            {
                competitionId = value;
                OnPropertyChanged("CompetitionId");
            }
        }

        private int fixtureId;
        public int FixtureId
        {
            get { return fixtureId; }
            set
            {
                fixtureId = value;
                OnPropertyChanged("FixtureId");
            }
        }

        private int matchViewId;
        public int MatchViewId
        {
            get { return matchViewId; }
            set
            {
                matchViewId = value;
                OnPropertyChanged("MatchViewId");
            }
        }

        private int homeTeamId;
        public int HomeTeamId
        {
            get { return homeTeamId; }
            set
            {
                homeTeamId = value;
                OnPropertyChanged("HomeTeamId");
            }
        }

        private int awayTeamId;
        public int AwayTeamId
        {
            get { return awayTeamId; }
            set
            {
                awayTeamId = value;
                OnPropertyChanged("AwayTeamId");
            }
        }

        private ObservableCollection<TeamComboModel> teamsList;
        public ObservableCollection<TeamComboModel> TeamsList
        {
            get { return teamsList; }
            set
            {
                teamsList = value;
                OnPropertyChanged("TeamsList");
            }
        }

        private int homeGoals;
        public int HomeGoals
        {
            get { return homeGoals; }
            set
            {
                homeGoals = value;
                OnPropertyChanged("HomeGoals");
            }
        }

        private int awayGoals;
        public int AwayGoals
        {
            get { return awayGoals; }
            set
            {
                awayGoals = value;
                OnPropertyChanged("AwayGoals");
            }
        }

        private decimal oddsHome;
        public decimal OddsHome
        {
            get { return oddsHome; }
            set
            {
                oddsHome = value;
                OnPropertyChanged("OddsHome");
            }
        }

        private decimal oddsDraw;
        public decimal OddsDraw
        {
            get { return oddsDraw; }
            set
            {
                oddsDraw = value;
                OnPropertyChanged("OddsDraw");
            }
        }

        private decimal oddsAway;
        public decimal OddsAway
        {
            get { return oddsAway; }
            set
            {
                oddsAway = value;
                OnPropertyChanged("OddsAway");
            }
        }

        private Visibility buttonSaveVisibility;
        public Visibility ButtonSaveVisibility
        {
            get { return buttonSaveVisibility; }
            set
            {
                buttonSaveVisibility = value;
                OnPropertyChanged("ButtonSaveVisibility");
            }
        }

        private Visibility buttonRemoveVisibility;
        public Visibility ButtonRemoveVisibility
        {
            get { return buttonRemoveVisibility; }
            set
            {
                buttonRemoveVisibility = value;
                OnPropertyChanged("ButtonRemoveVisibility");
            }
        }

        private Visibility imageDoneVisibility;
        public Visibility ImageDoneVisibility
        {
            get { return imageDoneVisibility; }
            set
            {
                imageDoneVisibility = value;
                OnPropertyChanged("ImageDoneVisibility");
            }
        }
    }
}
