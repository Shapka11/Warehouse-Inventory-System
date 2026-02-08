using System;
using System.ComponentModel.DataAnnotations;

namespace Wis.Presentation.Http.Models.StatisticsRolls;

public sealed class GetStatisticsRollRequest
{
    [Required]
    public DateTimeOffset From { get; set; }

    [Required]
    public DateTimeOffset To { get; set; }
}