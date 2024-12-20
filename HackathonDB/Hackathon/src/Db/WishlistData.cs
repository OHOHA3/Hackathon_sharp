using System.ComponentModel.DataAnnotations.Schema;

namespace Hackathon.Db;

[Table("wish_lists")]
public class WishListData
{
    [Column("id")]
    public int Id { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    public EmployeeData? Employee { get; set; }

    [Column("desired_employee_id")]
    public int DesiredEmployeeId { get; set; }

    public EmployeeData? DesiredEmployee { get; set; }

    [Column("grade")]
    public int Grade { get; set; }
}