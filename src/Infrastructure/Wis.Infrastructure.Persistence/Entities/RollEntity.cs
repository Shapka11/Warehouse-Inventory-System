namespace Infrastructure.Persistence.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("rolls")]
public class RollEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("length")]
    public double Length { get; set; }

    [Required]
    [Column("weight")]
    public double Weight { get; set; }

    [Required]
    [Column("added_date")]
    public DateTimeOffset AddedDate { get; set; }

    [Column("removed_date")]
    public DateTimeOffset? RemovedDate { get; set; }
}