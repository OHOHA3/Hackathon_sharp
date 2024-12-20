using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hackathon.Db;

[Table("hackathons")]
public class HackathonData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("harmonic")]
    public double Harmonic { get; set; }
}