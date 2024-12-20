using Contracts;

namespace HrDirector.Director
{
    public class HrDirector
    {
        public double CalculateHarmonic(
            List<Team> teams,
            List<WishList> teamLeadsWishLists,
            List<WishList> juniorsWishLists)
        {
            double denominator = 0;

            foreach (var team in teams)
            {
                denominator += 1 / CalculateSatisfactionForEmployee(teamLeadsWishLists, team.TeamLead, team.Junior);
                denominator += 1 / CalculateSatisfactionForEmployee(juniorsWishLists, team.Junior, team.TeamLead);
            }

            return (juniorsWishLists.Count + teamLeadsWishLists.Count) / denominator;
        }

        private static double CalculateSatisfactionForEmployee(
            List<WishList> wishListForEmployee, Employee employee, Employee teammate)
        {
            var desiredTeammates = wishListForEmployee.First(wl => wl.EmployeeId == employee.Id);
            var desiredEmployees = desiredTeammates.DesiredEmployees;
            return desiredEmployees.Length - Array.IndexOf(desiredEmployees, teammate.Id);
        }
    }
}