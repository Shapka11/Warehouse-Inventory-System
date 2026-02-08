using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Mapping;

public static class RollMappingExtensions
{
    public static RollEntity MapToEntity(this Roll domain)
    {
        return new RollEntity
        {
            Id = domain.Id,
            Length = domain.Length.Value,
            Weight = domain.Weight.Value,
            AddedDate = domain.AddedDate,
            RemovedDate = domain.RemovedDate
        };
    }
    
    public static Roll MapToDomain(this RollEntity entity)
    {
        return new Roll(
            entity.Id,
            new Length(entity.Length),
            new Weight(entity.Weight),
            entity.AddedDate,
            entity.RemovedDate
        );
    }
}