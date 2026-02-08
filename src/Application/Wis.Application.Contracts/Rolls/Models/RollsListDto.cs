namespace Application.Contracts.Rolls.Models;

public sealed record RollsListDto(IEnumerable<RollDto> Rolls);