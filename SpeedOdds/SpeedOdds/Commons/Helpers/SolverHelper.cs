using Microsoft.SolverFoundation.Services;
using SpeedOdds.UserControls.Rankings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Commons.Helpers
{
    public static class SolverHelper
    {
        private const string HomeAdvantageName = "HomeAdvantage";

        public static TeamSolver SolverProblem(TeamSolver solverData)
        {
            // Initializing the solver.
            Console.WriteLine("Initializing the solver");

            var solver = SolverContext.GetContext();
            var model = solver.CreateModel();


            // Defining our final outputs. These are referred to as our “decisions”.
            Console.WriteLine("Defining our decisions");

            model.AddDecision(solverData.HomeAdvantage);

            foreach (var item in solverData.Dados)
                model.AddDecision(item.TeamDecision);

            Console.WriteLine("Defining our constraint");
            model.AddConstraint("CapacityAdvantage", solverData.HomeAdvantage > 0);


            // We define the expression of our Average Rank
            int totalTeams = solverData.Dados.Count();
            string averageExp = "(";

            for(int i = 0; i < solverData.Dados.Count(); i++)
            {
                if(i == solverData.Dados.Count() - 1)
                    averageExp += Utils.RemoveSpecialChars(solverData.Dados[i].TeamName) + ") / " + totalTeams + " == 0"; 
                else
                    averageExp += Utils.RemoveSpecialChars(solverData.Dados[i].TeamName) + " + ";
            }

            model.AddConstraint("AverageRank", averageExp);


            // Define goal to Minimize sum
            Console.WriteLine("Define goal to Minimize sum");

            string goalTerm = "";

            for(int f = 0; f < solverData.Jogos.Count(); f++)
            {
                //teams decisions
                string tH = Utils.RemoveSpecialChars(solverData.Dados.Where(d => d.TeamId == solverData.Jogos[f].HomeTeamId).FirstOrDefault().TeamName);
                string tA = Utils.RemoveSpecialChars(solverData.Dados.Where(d => d.TeamId == solverData.Jogos[f].AwayTeamId).FirstOrDefault().TeamName);

                string mtch = "(" + solverData.Jogos[f].MargemCasa + " - (" + HomeAdvantageName + " + " + tH + " - " + tA + "))";

                if (f == solverData.Jogos.Count() - 1)
                    goalTerm += mtch + " * " + mtch;
                else
                    goalTerm += mtch + " * " + mtch + " + ";
            }

            model.AddGoal("BestProfit", GoalKind.Minimize, goalTerm);


            // Solve our problem
            Console.WriteLine("Solving problem...");
            var solution = solver.Solve();

            //return result
            return solverData;
        }
    }
}
