using SpeedOdds.Commons.Enums;
using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SpeedOdds.UserControls.Matches
{
    public class MatchViewModel : ObservableObject
    {
        private int matchId;
        public int MatchId
        {
            get { return matchId; }
            set
            {
                matchId = value;
                OnPropertyChanged("MatchId");
            }
        }

        private int order;
        public int Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }

        private string homeTeam;
        public string HomeTeam
        {
            get { return homeTeam; }
            set
            {
                homeTeam = value;
                OnPropertyChanged("HomeTeam");
            }
        }

        private string awayTeam;
        public string AwayTeam
        {
            get { return awayTeam; }
            set
            {
                awayTeam = value;
                OnPropertyChanged("AwayTeam");
            }
        }

        //Golos CASA
        private string homeGoals;
        public string HomeGoals
        {
            get { return homeGoals; }
            set
            {
                homeGoals = value;
                OnPropertyChanged("HomeGoals");
            }
        }

        //Golos FORA
        private string awayGoals;
        public string AwayGoals
        {
            get { return awayGoals; }
            set
            {
                awayGoals = value;
                OnPropertyChanged("AwayGoals");
            }
        }

        //RESULTADO
        private string matchResult;
        public string MatchResult
        {
            get { return matchResult; }
            set
            {
                matchResult = value;
                OnPropertyChanged("MatchResult");
            }
        }

        //CT - FT
        private string matchResultCSFT;
        public string MatchResultCSFT
        {
            get { return matchResultCSFT; }
            set
            {
                matchResultCSFT = value;
                OnPropertyChanged("MatchResultCSFT");
            }
        }

        //PSH   
        private string oddsHome;
        public string OddsHome
        {
            get { return oddsHome; }
            set
            {
                oddsHome = value;
                OnPropertyChanged("OddsHome");
            }
        }

        //PSD
        private string oddsDraw;
        public string OddsDraw
        {
            get { return oddsDraw; }
            set
            {
                oddsDraw = value;
                OnPropertyChanged("OddsDraw");
            }
        }

        //PSA
        private string oddsAway;
        public string OddsAway
        {
            get { return oddsAway; }
            set
            {
                oddsAway = value;
                OnPropertyChanged("OddsAway");
            }
        }

        //TIPO DE JOGO
        private string matchType;
        public string MatchType
        {
            get { return matchType; }
            set
            {
                matchType = value;
                OnPropertyChanged("MatchType");
            }
        }

        //OVER 0,5
        private string over05;
        public string Over05
        {
            get { return over05; }
            set
            {
                over05 = value;
                OnPropertyChanged("Over05");
            }
        }

        //OVER 1,5
        private string over15;
        public string Over15
        {
            get { return over15; }
            set
            {
                over15 = value;
                OnPropertyChanged("Over15");
            }
        }

        //OVER 0,5
        private string over25;
        public string Over25
        {
            get { return over25; }
            set
            {
                over25 = value;
                OnPropertyChanged("Over25");
            }
        }

        //BTTS
        private string btts;
        public string Btts
        {
            get { return btts; }
            set
            {
                btts = value;
                OnPropertyChanged("Btts");
            }
        }
    }
}
