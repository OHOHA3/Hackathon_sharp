using Hackathon.Employees;

namespace Hackathon.Hr
{
    public class HrDirector
    {
        public double CalculateHarmonic(
            List<Team> teams, 
            List<Wishlist> teamLeadsWishLists, 
            List<Wishlist> juniorsWishLists)
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
            List<Wishlist> wishListForEmployee, Employee employee, Employee teammate)
        {
            var desiredTeammates = wishListForEmployee.First(wl => wl.EmployeeId == employee.Id);
            var desiredEmployees = desiredTeammates.DesiredEmployees;
            return desiredEmployees.Length - Array.IndexOf(desiredEmployees, teammate.Id);
        }
    }
}
