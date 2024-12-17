using Nsu.HackathonProblem.Contracts;

namespace LeontyevStrategy
{
    public class LeontyevStrategy : ITeamBuildingStrategy
    {
        public IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
            IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists)
        {
            var desireMatrix = GenerateDesireMatrix(teamLeads.ToList(), juniors.ToList(), teamLeadsWishlists.ToList(), juniorsWishlists.ToList());
            var optimalAssignments = SolveUsingGreedyOptimization(desireMatrix);

            var optimalTeams = new List<Team>();
            for (int i = 0; i < optimalAssignments.Count; i++)
            {
                var teamLead = teamLeads.ToList()[i];
                var junior = juniors.ToList()[optimalAssignments[i]];
                optimalTeams.Add(new Team(teamLead, junior));
            }

            return optimalTeams;
        }

        private int[,] GenerateDesireMatrix(List<Employee> teamLeads, List<Employee> juniors,
            List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists)
        {
            int n = teamLeads.Count;
            var desireMatrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                var teamLead = teamLeads[i];
                var teamLeadWishlist = teamLeadsWishlists.FirstOrDefault(w => w.EmployeeId == teamLead.Id);

                for (int j = 0; j < n; j++)
                {
                    var junior = juniors[j];
                    var juniorWishlist = juniorsWishlists.FirstOrDefault(w => w.EmployeeId == junior.Id);

                    int teamLeadSatisfaction = n - Array.IndexOf(teamLeadWishlist!.DesiredEmployees, junior.Id);

                    int juniorSatisfaction = n - Array.IndexOf(juniorWishlist!.DesiredEmployees, teamLead.Id);

                    int teamSatisfaction = teamLeadSatisfaction + juniorSatisfaction;

                    desireMatrix[i, j] = teamSatisfaction;
                }
            }

            return desireMatrix;
        }

        private List<int> SolveUsingGreedyOptimization(int[,] desireMatrix)
        {
            int n = desireMatrix.GetLength(0);
            var result = new List<int>(new int[n]);

            var assignedJuniors = new HashSet<int>();

            for (int i = 0; i < n; i++)
            {
                int bestCost = 0;
                int bestJuniorIndex = -1;

                for (int j = 0; j < n; j++)
                {
                    if (!assignedJuniors.Contains(j) && desireMatrix[i, j] > bestCost)
                    {
                        bestCost = desireMatrix[i, j];
                        bestJuniorIndex = j;
                    }
                }

                if (bestJuniorIndex != -1)
                {
                    result[i] = bestJuniorIndex;
                    assignedJuniors.Add(bestJuniorIndex);
                }
            }

            result = LocalOptimization(desireMatrix, result);

            return result;
        }

        private List<int> LocalOptimization(int[,] desireMatrix, List<int> result)
        {
            int n = desireMatrix.GetLength(0);
            bool improved = true;
            int maxIterations = 1000;
            int iteration = 0;

            while (improved && iteration < maxIterations)
            {
                improved = false;
                iteration++;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            var newResult = new List<int>(result);
                            var temp = newResult[i];
                            newResult[i] = j;
                            newResult[j] = temp;

                            if (EvaluateCost(desireMatrix, newResult) > EvaluateCost(desireMatrix, result))
                            {
                                result = newResult;
                                improved = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private int EvaluateCost(int[,] desireMatrix, List<int> result)
        {
            return result.Select((t, i) => desireMatrix[i, t]).Sum();
        }
    }
}