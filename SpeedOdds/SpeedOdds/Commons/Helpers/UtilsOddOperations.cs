using SpeedOdds.Commons.Enums;
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


        //EQUIPAS

        public static string GetPercentage(int n, int total)
        {
            if (total == 0) return "0%";

            return ((decimal)n / total).ToString("0.0%");
        }

        public static string GetAverageNonPercentual(int n, int total)
        {
            return (n / total).ToString();
        }

        public static string GetGMGS(int gm, int gs, int total)
        {
            return ((gm / total) + (gs / total)).ToString();
        }

        public static string GetGM_GS(int gm, int gs, int total)
        {
            return ((gm / total) - (gs / total)).ToString();
        }

        public static string GetFatorCasa(int gm, int gs, int total)
        {
            return (((gm / total) - (gs / total))/total).ToString("0.0") + "%";
        }

        public static string GetOdd(int n, int total)
        {
            if (n == 0)
                return "0";

            return (1/((decimal)n / total)).ToString("0");
        }

        //TIPOS DE JOGO
        public static MatchTypeValues GetMatchType(decimal psh, decimal psa)
        {
            if (psh < (decimal)1.5)
                return MatchTypeValues.H_STRONG_FAV;
            else if (psh >= (decimal)1.5 && psh < (decimal)2)
                return MatchTypeValues.H_MEDIUM_FAV;
            else if (psh >= (decimal)2 && psa <= (decimal)3)
                return MatchTypeValues.COMPETITIVE;
            else if (psh >= (decimal)2 && psa > (decimal)3)
                return MatchTypeValues.H_SMALL_FAV;
            else if (psa >= (decimal)2 && psh > (decimal)3)
                return MatchTypeValues.A_SMALL_FAV;
            else if (psa >= (decimal)1.5 && psa < (decimal)2)
                return MatchTypeValues.A_MEDIUM_FAV;
            else
                return MatchTypeValues.A_STRONG_FAV;
        }

        public static string GetMatchTypeString(MatchTypeValues tp)
        {
            switch (tp)
            {
                case MatchTypeValues.A_STRONG_FAV:
                    return "A_STRONG_FAV";
                case MatchTypeValues.A_MEDIUM_FAV:
                    return "A_MEDIUM_FAV";
                case MatchTypeValues.H_STRONG_FAV:
                    return "H_STRONG_FAV";
                case MatchTypeValues.H_MEDIUM_FAV:
                    return "H_MEDIUM_FAV";
                case MatchTypeValues.A_SMALL_FAV:
                    return "A_SMALL_FAV";
                case MatchTypeValues.H_SMALL_FAV:
                    return "H_SMALL_FAV";
                case MatchTypeValues.COMPETITIVE:
                    return "COMPETITIVE";
                default:
                    return "";
            }
        }
    }
}
