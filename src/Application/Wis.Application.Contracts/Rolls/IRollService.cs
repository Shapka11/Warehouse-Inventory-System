using Application.Contracts.Rolls.Operations;

namespace Application.Contracts.Rolls;

public interface IRollService
{
    Task<AddRoll.Response> AddRollAsync(AddRoll.Request request);

    Task<RemoveRoll.Response> RemoveRollAsync(RemoveRoll.Request request);

    Task<GetRollsList.Response> GetRollsListAsync(GetRollsList.Request request);
}