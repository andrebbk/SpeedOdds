using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.UserControls.HomeAwayTeams
{
    public class HomeAwayTeamViewModel : ObservableObject
    {
        private string team;
        public string Team
        {
            get { return team; }
            set
            {
                team = value;
                OnPropertyChanged("Team");
            }
        }

        private string matches;
        public string Matches
        {
            get { return matches; }
            set
            {
                matches = value;
                OnPropertyChanged("Matches");
            }
        }

        private string wins;
        public string Wins
        {
            get { return wins; }
            set
            {
                wins = value;
                OnPropertyChanged("Wins");
            }
        }

        private string winsP;
        public string WinsP
        {
            get { return winsP; }
            set
            {
                winsP = value;
                OnPropertyChanged("WinsP");
            }
        }

        private string winsO;
        public string WinsO
        {
            get { return winsO; }
            set
            {
                winsO = value;
                OnPropertyChanged("WinsO");
            }
        }

        private string draws;
        public string Draws
        {
            get { return draws; }
            set
            {
                draws = value;
                OnPropertyChanged("Draws");
            }
        }

        private string drawsP;
        public string DrawsP
        {
            get { return drawsP; }
            set
            {
                drawsP = value;
                OnPropertyChanged("DrawsP");
            }
        }

        private string drawsO;
        public string DrawsO
        {
            get { return drawsO; }
            set
            {
                drawsO = value;
                OnPropertyChanged("DrawsO");
            }
        }

        private string defeats;
        public string Defeats
        {
            get { return defeats; }
            set
            {
                defeats = value;
                OnPropertyChanged("Defeats");
            }
        }

        private string defeatsP;
        public string DefeatsP
        {
            get { return defeatsP; }
            set
            {
                defeatsP = value;
                OnPropertyChanged("DefeatsP");
            }
        }

        private string defeatsO;
        public string DefeatsO
        {
            get { return defeatsO; }
            set
            {
                defeatsO = value;
                OnPropertyChanged("DefeatsO");
            }
        }

        private string forma;
        public string Forma
        {
            get { return forma; }
            set
            {
                forma = value;
                OnPropertyChanged("Forma");
            }
        }

        private string gm;
        public string GM
        {
            get { return gm; }
            set
            {
                gm = value;
                OnPropertyChanged("GM");
            }
        }

        private string gs;
        public string GS
        {
            get { return gs; }
            set
            {
                gs = value;
                OnPropertyChanged("GS");
            }
        }

        private string gmGs;
        public string GmGs
        {
            get { return gmGs; }
            set
            {
                gmGs = value;
                OnPropertyChanged("GmGs");
            }
        }

        private string gm_Gs;
        public string Gm_Gs
        {
            get { return gm_Gs; }
            set
            {
                gm_Gs = value;
                OnPropertyChanged("Gm_Gs");
            }
        }

        private string fatorCasa;
        public string FatorCasa
        {
            get { return fatorCasa; }
            set
            {
                fatorCasa = value;
                OnPropertyChanged("FatorCasa");
            }
        }

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

        private string over25P;
        public string Over25P
        {
            get { return over25P; }
            set
            {
                over25P = value;
                OnPropertyChanged("Over25P");
            }
        }

        private string over25O;
        public string Over25O
        {
            get { return over25O; }
            set
            {
                over25O = value;
                OnPropertyChanged("Over25O");
            }
        }

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

        private string over15P;
        public string Over15P
        {
            get { return over15P; }
            set
            {
                over15P = value;
                OnPropertyChanged("Over15P");
            }
        }

        private string over15O;
        public string Over15O
        {
            get { return over15O; }
            set
            {
                over15O = value;
                OnPropertyChanged("Over15O");
            }
        }

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

        private string bttsP;
        public string BttsP
        {
            get { return bttsP; }
            set
            {
                bttsP = value;
                OnPropertyChanged("BttsP");
            }
        }

        private string bttsO;
        public string BttsO
        {
            get { return bttsO; }
            set
            {
                bttsO = value;
                OnPropertyChanged("BttsO");
            }
        }

        private string p00;
        public string P00
        {
            get { return p00; }
            set
            {
                p00 = value;
                OnPropertyChanged("P00");
            }
        }

        private string p10;
        public string P10
        {
            get { return p10; }
            set
            {
                p10 = value;
                OnPropertyChanged("P10");
            }
        }

        private string p01;
        public string P01
        {
            get { return p01; }
            set
            {
                p01 = value;
                OnPropertyChanged("P01");
            }
        }

        private string p11;
        public string P11
        {
            get { return p11; }
            set
            {
                p11 = value;
                OnPropertyChanged("P11");
            }
        }

        private string p20;
        public string P20
        {
            get { return p20; }
            set
            {
                p20 = value;
                OnPropertyChanged("P20");
            }
        }

        private string p02;
        public string P02
        {
            get { return p02; }
            set
            {
                p02 = value;
                OnPropertyChanged("P02");
            }
        }

        private string p21;
        public string P21
        {
            get { return p21; }
            set
            {
                p21 = value;
                OnPropertyChanged("P21");
            }
        }

        private string p12;
        public string P12
        {
            get { return p12; }
            set
            {
                p12 = value;
                OnPropertyChanged("P12");
            }
        }

        private string p22;
        public string P22
        {
            get { return p22; }
            set
            {
                p22 = value;
                OnPropertyChanged("P22");
            }
        }

        private string p30;
        public string P30
        {
            get { return p30; }
            set
            {
                p30 = value;
                OnPropertyChanged("P30");
            }
        }

        private string p03;
        public string P03
        {
            get { return p03; }
            set
            {
                p03 = value;
                OnPropertyChanged("P03");
            }
        }

        private string p31;
        public string P31
        {
            get { return p31; }
            set
            {
                p31 = value;
                OnPropertyChanged("P31");
            }
        }

        private string p13;
        public string P13
        {
            get { return p13; }
            set
            {
                p13 = value;
                OnPropertyChanged("P13");
            }
        }

        private string p32;
        public string P32
        {
            get { return p32; }
            set
            {
                p32 = value;
                OnPropertyChanged("P32");
            }
        }

        private string p23;
        public string P23
        {
            get { return p23; }
            set
            {
                p23 = value;
                OnPropertyChanged("P23");
            }
        }

        private string p33;
        public string P33
        {
            get { return p33; }
            set
            {
                p33 = value;
                OnPropertyChanged("P33");
            }
        }
    }
}
