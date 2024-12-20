namespace Contracts;

public record StartMessage;

public record TeamsInfo(List<Team> Teams);

public record EmployeeInfo(WishList WishList, Employee Employee, bool IsTeamLead);

public record Team(Employee TeamLead, Employee Junior);

public record Employee(int Id, string Name);

public record WishList(int EmployeeId, int[] DesiredEmployees);