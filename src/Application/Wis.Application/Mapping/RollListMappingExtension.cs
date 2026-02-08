using Application.Contracts.Rolls.Models;
using Domain.Entities;

namespace Application.Mapping;

public static class RollListMappingExtension
{
    public static RollsListDto MapToDto(this IEnumerable<Roll> operations)
        => new(operations.Select(operation => operation.MapToDto()));
}