using System.Diagnostics;
using System.Threading.Tasks;
using Application.Contracts.Rolls;
using Application.Contracts.Rolls.Models;
using Application.Contracts.Rolls.Operations;
using Microsoft.AspNetCore.Mvc;
using Presentation.Http.Models.Rolls;
using Wis.Presentation.Http.Models.Rolls;

namespace Wis.Presentation.Http.Controllers;

[ApiController]
[Route("api/roll")]
public sealed class RollController : ControllerBase
{
    private readonly IRollService _rollService;

    public RollController(IRollService rollService)
    {
        _rollService = rollService;
    }

    [HttpPost("add")]
    public async Task<ActionResult<RollDto>> AddRoll([FromBody] AddRollRequest httpRequest)
    {
        var request = new AddRoll.Request(httpRequest.Length, httpRequest.Weight);
        AddRoll.Response response = await _rollService.AddRollAsync(request);

        return Ok(response.Roll);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RollDto>> RemoveRoll([FromBody] RemoveRollRequest httpRequest)
    {
        var request = new RemoveRoll.Request(httpRequest.Id);
        RemoveRoll.Response response = await _rollService.RemoveRollAsync(request);

        return response switch
        {
            RemoveRoll.Response.Success success => Ok(success.Roll),
            RemoveRoll.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }

    [HttpGet]
    public async Task<ActionResult<RollsListDto>> GetListRolls([FromQuery] GetRollsListRequest httpRequest)
    {
        var request = new GetRollsList.Request(
            httpRequest.Id,
            httpRequest.Length,
            httpRequest.Weight,
            httpRequest.AddedDate,
            httpRequest.RemovedDate);
        GetRollsList.Response response = await _rollService.GetRollsListAsync(request);

        return response switch
        {
            GetRollsList.Response.Success success => Ok(success.Rolls),
            GetRollsList.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }
}