using System.ComponentModel.DataAnnotations.Schema;

namespace Hackathon.Db;

[Table("employees")]
public class EmployeeData
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("is_team_lead")]
    public bool IsTeamLead { get; set; }

    [Column("hackathon_id")]
    public int HackathonId { get; set; }

    public HackathonData? Hackathon { get; set; }
}