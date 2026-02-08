using Application.Contracts.Rolls.Models;
using Domain.Entities;

namespace Application.Mapping;

public static class RollMappingExtension
{
    public static RollDto MapToDto(this Roll roll)
        => new RollDto(
            roll.Id,
            roll.Length.Value,
            roll.Weight.Value,
            roll.AddedDate,
            roll.RemovedDate);
}