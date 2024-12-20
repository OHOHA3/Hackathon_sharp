namespace HrManager.Rest;

public record TeamsInfo(List<Employee> TeamLeads, List<Employee> Juniors, List<WishList> TeamLeadsWishLists, List<WishList> JuniorsWishLists, List<Team> Teams);

public record EmployeeInfo(WishList WishList, Employee Employee, bool IsTeamLead);

public record Team(Employee TeamLead, Employee Junior);

public record Employee(int Id, string Name);

public record WishList(int EmployeeId, int[] DesiredEmployees);