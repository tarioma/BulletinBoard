using AutoMapper;
using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Domain.Entities;
using BulletinBoard.WebAPI.Models.Requests;
using BulletinBoard.WebAPI.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BulletinsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateBulletinResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBulletinRequest request,
        [FromServices] IRequestHandler<ICreateBulletinCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var bulletinId = await handler.Handle(request, cancellationToken);
        var response = new CreateBulletinResponse { Id = bulletinId };

        var uri = Url.Action("Get", "Bulletins", new { id = bulletinId });
        return Created(uri, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetBulletinByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        Guid id,
        [FromServices] IRequestHandler<IGetBulletinByIdQuery, Bulletin> handler,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = new GetBulletinByIdRequest { Id = id };
        var bulletin = await handler.Handle(request, cancellationToken);
        var response = mapper.Map<GetBulletinByIdResponse>(bulletin);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<SearchBulletinsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchBulletinsRequest request,
        [FromServices] IRequestHandler<ISearchBulletinsQuery, Bulletin[]> handler,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var bulletins = await handler.Handle(request, cancellationToken);
        var response = mapper.Map<SearchBulletinsResponse>(bulletins);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBulletinRequest request,
        [FromServices] IRequestHandler<IUpdateBulletinCommand> handler,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        await handler.Handle(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] IRequestHandler<IDeleteBulletinCommand> handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteBulletinRequest { Id = id };
        await handler.Handle(request, cancellationToken);

        return NoContent();
    }
}