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
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBulletinByIdQuery(id);
        var bulletin = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<GetBulletinByIdResponse>(bulletin);

        return Ok(response);
    }

    [HttpGet]
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
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateBulletinRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateBulletinCommand>(request);
        command = command with { Id = id };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBulletinCommand(id);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}