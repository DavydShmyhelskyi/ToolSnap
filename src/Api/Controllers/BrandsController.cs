using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Brands.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("brands")]
    public class BrandsController(
        IBrandQueries queries,
        IBrandControllerService service,
        ISender sender) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<BrandDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands(
            CancellationToken cancellationToken)
        {
            var brands = await queries.GetAllAsync(cancellationToken);

            // inline мапінг на DTO
            var result = brands
                .Select(b => new BrandDto(b.Id.Value, b.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<BrandDto>>(
                brand => Ok(new BrandDto(brand.Id, brand.Title)),
                () => NotFound());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BrandDto>> Create(
            [FromBody] CreateBrandDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateBrandCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<BrandDto>>(
                brand => CreatedAtAction(
                    nameof(GetById),
                    new { id = brand.Id.Value },
                    new BrandDto(brand.Id.Value, brand.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandDto>> Update(
            Guid id,
            [FromBody] UpdateBrandDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateBrandCommand
            {
                BrandId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<BrandDto>>(
                brand => Ok(new BrandDto(brand.Id.Value, brand.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteBrandCommand
            {
                BrandId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}
