using Microsoft.SolverFoundation.Services;
using SpeedOdds.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.UserControls.Rankings
{
    public class TeamSolverData
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public Decision TeamDecision { get; set; }
    }

    public class TeamSolver
    {
        public TeamSolver()
        {
            HomeAdvantage = new Decision(Domain.RealNonnegative, "HomeAdvantage");
            Dados = new List<TeamSolverData>();
            Jogos = new List<MatchSolverItem>();
            InputDomain = Domain.RealRange(-2.00, 2.00);
        }

        public Domain InputDomain { get; }

        public Decision HomeAdvantage { get; set; }

        public List<TeamSolverData> Dados;

        public List<MatchSolverItem> Jogos;
    }
}
