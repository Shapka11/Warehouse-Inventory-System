using System;

namespace Wis.Presentation.Http.Models.Rolls;

public sealed class GetRollsListRequest
{
    public Guid? Id { get; set;}

    public double? Length { get; set; }

    public double? Weight { get; set; }

    public DateTimeOffset? AddedDate { get; set; }

    public DateTimeOffset? RemovedDate { get; set; }
}