using Api.DTOs;
using Application.Common.Interfaces.Queries;
using Domain.Models.Tools;
using Domain.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tool-valuations")]
    public class ToolValuationsController(IToolLiabilityQueries queries) : ControllerBase
    {
        [HttpGet("inventory")]
        [ProducesResponseType(typeof(InventoryStatsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<InventoryStatsDto>> GetInventoryStats(CancellationToken cancellationToken)
        {
            var (totalValue, toolCount) = await queries.GetInventoryStatsAsync(cancellationToken);
            return Ok(new InventoryStatsDto(totalValue, toolCount));
        }

        [HttpGet("worker/{userId:guid}")]
        [ProducesResponseType(typeof(WorkerOnHandsStatsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<WorkerOnHandsStatsDto>> GetWorkerOnHandsStats(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var (totalValue, toolCount) = await queries.GetWorkerOnHandsStatsAsync(new UserId(userId), cancellationToken);
            return Ok(new WorkerOnHandsStatsDto(userId, totalValue, toolCount));
        }

        [HttpGet("liabilities")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolLiabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolLiabilityDto>>> GetAll(CancellationToken cancellationToken)
        {
            var liabilities = await queries.GetAllAsync(cancellationToken);
            return Ok(liabilities.Select(ToolLiabilityDto.FromDomain).ToList());
        }

        [HttpGet("liabilities/tool/{toolId:guid}")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolLiabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolLiabilityDto>>> GetByTool(
            Guid toolId,
            CancellationToken cancellationToken)
        {
            var liabilities = await queries.GetAllByToolAsync(new ToolId(toolId), cancellationToken);
            return Ok(liabilities.Select(ToolLiabilityDto.FromDomain).ToList());
        }

        [HttpGet("liabilities/worker/{userId:guid}")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolLiabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolLiabilityDto>>> GetByWorker(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var liabilities = await queries.GetAllByWorkerAsync(new UserId(userId), cancellationToken);
            return Ok(liabilities.Select(ToolLiabilityDto.FromDomain).ToList());
        }

        [HttpGet("liabilities/worker/{userId:guid}/open")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolLiabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolLiabilityDto>>> GetOpenByWorker(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var liabilities = await queries.GetOpenByWorkerAsync(new UserId(userId), cancellationToken);
            return Ok(liabilities.Select(ToolLiabilityDto.FromDomain).ToList());
        }
    }
}
