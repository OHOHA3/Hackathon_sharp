using System.ComponentModel.DataAnnotations.Schema;

namespace HrDirector.Db;

[Table("teams")]
public class TeamData
{
    [Column("id")]
    public int Id { get; set; }

    [Column("junior_id")]
    public int JuniorId { get; set; }

    public EmployeeData? Junior { get; set; }

    [Column("team_lead_id")]
    public int TeamLeadId { get; set; }

    public EmployeeData? TeamLead { get; set; }
}