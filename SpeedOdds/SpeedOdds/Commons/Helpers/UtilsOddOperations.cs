using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Commons.Helpers
{
    public class UtilsOddOperations
    {
        //ATAQUE_DEFESA 
        //HALF TIME
        //ST

        public static string GetResultado(int homeGoals, int awayGoals)
        {
            if (homeGoals > awayGoals)
                return "H";
            else if (homeGoals < awayGoals)
                return "A";
            else if (homeGoals == awayGoals)
                return "D";

            return "Error";
        }

        public static string GetTipoJogo (decimal psh, decimal psa)
        {
            if (psh < (decimal)1.5)
                return "H_STRONG_FAV";
            else if (psh >= (decimal)1.5 && psh < (decimal)2)
                return "H_MEDIUM_FAV";
            else if (psh >= (decimal)2 && psa <= (decimal)3)
                return "COMPETITIVE";
            else if (psh >= (decimal)2 && psa > (decimal)3)
                return "H_SMALL_FAV";
            else if (psa >= (decimal)2 && psh > (decimal)3)
                return "A_SMALL_FAV";
            else if (psa >= (decimal)1.5 && psa < (decimal)2)
                return "A_MEDIUM_FAV";
            else
                return "A_STRONG_FAV";
        }

        public static string GetBTTS (int homeGoals, int awayGoals)
        {
            if (homeGoals > (decimal)0.5 && awayGoals > (decimal)0.5)
                return "BTTS";
            else
                return "NO";

        }

        public static string GetOverUnder05(int homeGoals, int awayGoals)
        {
            if ((homeGoals + awayGoals) > (decimal)0.5)
                return "OVER 0,5";
            else
                return "UNDER 0,5";
        }

        public static string GetOverUnder15(int homeGoals, int awayGoals)
        {
            if ((homeGoals + awayGoals) > (decimal)1.5)
                return "OVER 1,5";
            else
                return "UNDER 1,5";
        }

        public static string GetOverUnder25(int homeGoals, int awayGoals)
        {
            if ((homeGoals + awayGoals) > (decimal)2.5)
                return "OVER 2,5";
            else
                return "UNDER 2,5";
        }
        
    }
}
