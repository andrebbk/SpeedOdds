using SpeedOdds.Data;
using SpeedOdds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Services
{
    public class CompetitionService
    {
        public bool AlreadyExistsCompetition(string name, int seasonId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return db.Competitions.Where(c => c.Name == name.Trim() && c.SeasonId == seasonId).Any();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking competition -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return true;
            }
        }

        public bool CreateCompetition(string name, int seasonId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    Competition comp = new Competition()
                    {
                        Name = name.Trim(),
                        SeasonId = seasonId,
                        CreateDate = DateTime.Now
                    };

                    db.Competitions.Add(comp);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new competition -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public IEnumerable<Competition> GetCompetitions()
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.Competitions.OrderBy(x => x.Name).ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Competitions from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public bool RemoveSeason(int competitionId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var comp = db.Competitions.Where(x => x.CompetitionId == competitionId).FirstOrDefault();

                    if (comp != null)
                    {
                        db.Competitions.Remove(comp);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing competition from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
