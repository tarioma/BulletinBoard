﻿using Ardalis.GuardClauses;
using BulletinBoard.Application.Users.CreateUser;
using BulletinBoard.Application.Users.DeleteUser;
using BulletinBoard.Application.Users.GetUserById;
using BulletinBoard.Application.Users.SearchUsers;
using BulletinBoard.Application.Users.UpdateUser;
using BulletinBoard.Contracts.Users.Requests;
using BulletinBoard.Contracts.Users.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(mapper);

        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var userId = await _mediator.Send(command, cancellationToken);
        var response = new CreateUserResponse(userId);

        var uri = Url.Action("Get", "Users", new { id = userId });
        return Created(uri, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetUserByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<GetUserByIdResponse>(user);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<SearchUsersResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] SearchUsersRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<SearchUsersQuery>(request);
        var users = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<SearchUsersResponse>(users);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserCommand>(request);
        command.Id = id;
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}