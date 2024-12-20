namespace Employee.Rest;

public record EmployeeInfo(WishList WishList, Employee Employee, bool IsTeamLead);

public record Employee(int Id, string Name);

public record WishList(int EmployeeId, int[] DesiredEmployees);