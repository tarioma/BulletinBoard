using BulletinBoard.Application.Abstraction.Models.Commands;
using BulletinBoard.Application.Abstraction.Models.Queries;
using BulletinBoard.Domain.Entities;
using BulletinBoard.WebAPI.Models.Requests;
using BulletinBoard.WebAPI.Models.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        [FromServices] IRequestHandler<ICreateUserCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var userId = await handler.Handle(request, cancellationToken);
        var response = new CreateUserResponse { Id = userId };

        var uri = Url.Action("Get", "Users", new { id = userId });
        return Created(uri, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetUserByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        Guid id,
        [FromServices] IRequestHandler<IGetUserByIdQuery, User> handler,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var request = new GetUserByIdQuery { Id = id };
        var user = await handler.Handle(request, cancellationToken);
        var response = mapper.Map<GetUserByIdResponse>(user);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<SearchUsersResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchUsersRequest request,
        [FromServices] IRequestHandler<ISearchUsersQuery, User[]> handler,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var users = await handler.Handle(request, cancellationToken);
        var response = mapper.Map<SearchUsersResponse>(users);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
        [FromServices] IRequestHandler<IUpdateUserCommand> handler,
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
        [FromServices] IRequestHandler<IDeleteUserCommand> handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteUserRequest { Id = id };
        await handler.Handle(request, cancellationToken);

        return NoContent();
    }
}