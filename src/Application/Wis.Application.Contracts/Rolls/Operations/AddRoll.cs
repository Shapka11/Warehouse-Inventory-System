using Application.Contracts.Rolls.Models;

namespace Application.Contracts.Rolls.Operations;

public static class AddRoll
{
    public readonly record struct Request(double Length, double Weight);

    public readonly record struct Response(RollDto Roll);
}