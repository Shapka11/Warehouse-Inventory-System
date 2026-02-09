using System.Diagnostics;
using System.Threading.Tasks;
using Application.Contracts.Statistics;
using Application.Contracts.Statistics.Models;
using Application.Contracts.Statistics.Operations;
using Microsoft.AspNetCore.Mvc;
using Wis.Presentation.Http.Models.StatisticsRolls;

namespace Wis.Presentation.Http.Controllers;

[ApiController]
[Route("api/statistics")]
public sealed class StatisticsRollController : ControllerBase
{
    private readonly IStatisticsRollService _statisticsRollService;

    public StatisticsRollController(IStatisticsRollService statisticsRollService)
    {
        _statisticsRollService = statisticsRollService;
    }

    [HttpGet]
    public async Task<ActionResult<StatisticsRollDto>> GetStatistics(
        [FromQuery] GetStatisticsRollRequest httpRollRequest)
    {
        var request = new GetStatisticsRoll.Request(httpRollRequest.From, httpRollRequest.To);
        GetStatisticsRoll.Response response = await _statisticsRollService.GetStatisticsAsync(request);

        return response switch
        {
            GetStatisticsRoll.Response.Success success => Ok(success.StatisticsRoll),
            GetStatisticsRoll.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };

    }
}