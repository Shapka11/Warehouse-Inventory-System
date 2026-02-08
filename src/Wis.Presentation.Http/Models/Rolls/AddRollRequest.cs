using System.ComponentModel.DataAnnotations;

namespace Presentation.Http.Models.Rolls;

public sealed class AddRollRequest
{
    [Required]
    public double Length { get; set; }

    [Required]
    public double Weight { get; set; }
}