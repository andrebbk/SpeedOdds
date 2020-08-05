using SpeedOdds.Commons.Enums;
using SpeedOdds.Data;
using SpeedOdds.Models;
using SpeedOdds.UserControls.TimeGoals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public bool InsertOrUpdateTimeGoals(ObservableCollection<TimeGoalsModel> dados)
        {
            if (dados == null || dados != null ? dados.Count() < 1 : true)
                return false;

            try
            {
                using (var db = new SpeedOddsContext())
                {
                    foreach(var item in dados)
                    {
                        if (item.TimeGoalsId.HasValue)
                        {
                            //Update
                            var timeGoal = db.TimeGoals.Where(t => t.TimeGoalsId == item.TimeGoalsId).FirstOrDefault();
                            if(timeGoal != null)
                            {
                                timeGoal.Goal15 = item.Goal15;
                                timeGoal.Goal30 = item.Goal30;
                                timeGoal.Goal45 = item.Goal45;
                                timeGoal.Goal60 = item.Goal60;
                                timeGoal.Goal75 = item.Goal75;
                                timeGoal.Goal90 = item.Goal90;

                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            //Create
                            TimeGoal timeGoal = new TimeGoal()
                            {
                                CompetitionId = item.CompetitionId.Value,
                                TeamId = item.TeamId.Value,
                                Goal15 = item.Goal15,
                                Goal30 = item.Goal30,
                                Goal45 = item.Goal45,
                                Goal60 = item.Goal60,
                                Goal75 = item.Goal75,
                                Goal90 = item.Goal90
                            };

                            db.TimeGoals.Add(timeGoal);
                            db.SaveChanges();
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving TimeGoals in BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
