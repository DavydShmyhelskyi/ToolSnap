using Api.DTOs;
using Application.Common.Interfaces.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("overdue-report")]
    public class OverdueReportController(IOverdueReportQueries queries) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IReadOnlyList<WorkerOverdueReportDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<WorkerOverdueReportDto>>> Get(CancellationToken cancellationToken)
        {
            var data = await queries.GetOverdueAsync(cancellationToken);
            var now = DateTime.UtcNow;

            var report = data
                .GroupBy(d => d.Assignment.UserId.Value)
                .Select(g => new WorkerOverdueReportDto(
                    UserId: g.Key,
                    OverdueCount: g.Count(),
                    TotalValueAtRisk: g.Sum(d => d.ValueAtRisk),
                    Assignments: g
                        .Select(d => new OverdueAssignmentItemDto(
                            AssignmentId: d.Assignment.Id.Value,
                            ToolId: d.Assignment.ToolId.Value,
                            DueAt: d.Assignment.DueAt!.Value,
                            OverdueMinutes: (long)(now - d.Assignment.DueAt!.Value).TotalMinutes,
                            ValueAtRisk: d.ValueAtRisk))
                        .ToList()))
                .OrderByDescending(w => w.TotalValueAtRisk)
                .ToList();

            return Ok(report);
        }
    }
}
