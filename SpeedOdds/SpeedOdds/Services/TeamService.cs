using SpeedOdds.Data;
using SpeedOdds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Services
{
    public class TeamService
    {
        public bool AlreadyExistsTeam(string name, int competitionId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return db.Teams.Where(t => t.Name == name && t.CompetitionId == competitionId).Any();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking team -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return true;
            }
        }
        
        public bool CreateTeam(string name, int competitionId, bool isFavorite)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    Team team = new Team()
                    {
                        Name = name.Trim(),
                        CompetitionId = competitionId,
                        IsFavorite = isFavorite,
                        CreateDate = DateTime.Now
                    };

                    db.Teams.Add(team);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new team -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
   
        public bool ChangeFavoriteValue(int teamId, bool newVal)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var team = db.Teams.Where(x => x.TeamId == teamId).FirstOrDefault();

                    if (team != null)
                    {
                        team.IsFavorite = newVal;
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error changing favorite team from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public IEnumerable<Team> GetTeams()
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.Teams.OrderBy(x => x.Name).ThenBy(x => x.CreateDate).ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Teams from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public bool RemoveTeam(int teamId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var team = db.Teams.Where(x => x.TeamId == teamId).FirstOrDefault();

                    if (team != null)
                    {
                        db.Teams.Remove(team);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing team from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
