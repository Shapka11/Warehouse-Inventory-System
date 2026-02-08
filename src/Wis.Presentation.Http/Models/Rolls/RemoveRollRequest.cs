using System;
using System.ComponentModel.DataAnnotations;

namespace Wis.Presentation.Http.Models.Rolls;

public sealed class RemoveRollRequest
{
    [Required]
    public Guid Id { get; set; }
}