using Ardalis.GuardClauses;
using BulletinBoard.Application.Bulletins.CreateBulletin;
using BulletinBoard.Application.Bulletins.DeleteBulletin;
using BulletinBoard.Application.Bulletins.GetBulletinById;
using BulletinBoard.Application.Bulletins.SearchBulletins;
using BulletinBoard.Application.Bulletins.UpdateBulletin;
using BulletinBoard.Contracts.Bulletins.Requests;
using BulletinBoard.Contracts.Bulletins.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BulletinsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BulletinsController(IMediator mediator, IMapper mapper)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(mapper);

        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType<CreateBulletinResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromForm] CreateBulletinRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateBulletinCommand>(request);
        var bulletinId = await _mediator.Send(command, cancellationToken);
        var response = new CreateBulletinResponse(bulletinId);

        var uri = Url.Action("Get", "Bulletins", new { id = bulletinId });
        return Created(uri, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetBulletinByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBulletinByIdQuery(id);
        var bulletin = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<GetBulletinByIdResponse>(bulletin);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<SearchBulletinsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchBulletinsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<SearchBulletinsQuery>(request);
        var bulletins = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<SearchBulletinsResponse>(bulletins);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateBulletinRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateBulletinCommand>(request);
        command.Id = id;
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBulletinCommand(id);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}