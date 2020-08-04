using SpeedOdds.Commons.Enums;
using SpeedOdds.Data;
using SpeedOdds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Services
{
    public class TimeGoalsService
    {
        public IEnumerable<TimeGoal> GetTimeGoalsByCompetition(int compId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.TimeGoals.Where(t => t.CompetitionId == compId)
                        .OrderBy(x => x.TeamId)
                        .ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting TimeGoals by competition from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public string GetPercentageOfTimeGoal(TimeGoalsTypeValues _time, TimeGoal timeGoal)
        {
            int totalGoals = (timeGoal.Goal15 + timeGoal.Goal30 + timeGoal.Goal45 + timeGoal.Goal60 + timeGoal.Goal75 + timeGoal.Goal90) ?? 0;
            if (totalGoals < 1) return "100%";

            if (_time is TimeGoalsTypeValues.Goal15)
            {
                if (!timeGoal.Goal15.HasValue) return "0%";

                return Math.Round(((decimal)timeGoal.Goal15.Value / totalGoals) * 100, 1).ToString() + "%";
            }
            else if (_time is TimeGoalsTypeValues.Goal30)
            {
                if (!timeGoal.Goal30.HasValue) return "0%";

                return Math.Round(((decimal)timeGoal.Goal30.Value / totalGoals) * 100, 1).ToString() + "%";
            }
            else if (_time is TimeGoalsTypeValues.Goal45)
            {
                if (!timeGoal.Goal45.HasValue) return "0%";

                return Math.Round(((decimal)timeGoal.Goal45.Value / totalGoals) * 100, 1).ToString() + "%";
            }
            else if (_time is TimeGoalsTypeValues.Goal60)
            {
                if (!timeGoal.Goal60.HasValue) return "0%";

                return Math.Round(((decimal)timeGoal.Goal60.Value / totalGoals) * 100, 1).ToString() + "%";
            }
            else if (_time is TimeGoalsTypeValues.Goal75)
            {
                if (!timeGoal.Goal75.HasValue) return "0%";

                return Math.Round(((decimal)timeGoal.Goal75.Value / totalGoals) * 100, 1).ToString() + "%";
            }
            else if (_time is TimeGoalsTypeValues.Goal90)
            {
                if (!timeGoal.Goal90.HasValue) return "0%";

                return Math.Round(((decimal)timeGoal.Goal90.Value / totalGoals) * 100, 1).ToString() + "%";
            }
            else
                return "0%";
        }

        public string GetPercentageOfTimeGoalByValue(int goals, int totalGoals)
        {
            if (goals > totalGoals || totalGoals < 1) return "100%";

            return Math.Round(((decimal)goals / totalGoals) * 100, 1).ToString() + "%";
        }
    }
}
