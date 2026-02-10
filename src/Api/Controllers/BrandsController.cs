using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;
using Application.Entities.Brands.Commands;
using Api.Modules.Errors;


namespace Api.Controllers
{
    public class BrandsController(
        IBrandQueries queries,
        IBrandControllerService service,
        ISender sender) : ControllerBase
    {

        // GET: all brands
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands(CancellationToken cancellationToken)
            { 
            var brands = await queries.GetAllAsync(cancellationToken);
            return brands.Select(BrandDto.FromDomain).ToList();
        }

        // GET: brand by id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BrandDto>> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<BrandDto>>(
                e => e,
                () => NotFound());
        }


        // Create
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Create(
            [FromBody] CreateBrandDto request,
            CancellationToken cancellationToken)
        {
            var input = new CreateBrandCommand { Title = request.Title };
            var result = await sender.Send(input, cancellationToken);
            return result.Match<ActionResult<BrandDto>>(
                b => BrandDto.FromDomain(b),
                e => e.ToObjectResult());
        }



        [HttpPut]
        public async Task<ActionResult<BrandDto>> Update(
            [FromBody] UpdateBrandDto request,
            CancellationToken cancellationToken)
        {
            var input = new UpdateBrandCommand { BrandId = request.Id, Title = request.Title };
            var result = await sender.Send(input, cancellationToken);
            return result.Match<ActionResult<BrandDto>>(
                b => BrandDto.FromDomain(b),
                e => e.ToObjectResult());
        }


        [HttpDelete]
        public async Task<ActionResult<BrandDto>> Delete(
           [FromBody] Guid id,
           CancellationToken cancellationToken)
        {
            var input = new DeleteBrandCommand { BrandId = id };
            var result = await sender.Send(input, cancellationToken);
            return result.Match<ActionResult<BrandDto>>(
                b => BrandDto.FromDomain(b),
                e => e.ToObjectResult());
        }
    }
}