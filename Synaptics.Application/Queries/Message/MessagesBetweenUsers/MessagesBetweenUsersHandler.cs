using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Common.Results;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.Message.MessagesBetweenUsers;

public class MessagesBetweenUsersHandler : IRequestHandler<MessagesBetweenUsersQuery, Response>
{
    readonly ICassandraService _cassandraService;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IHttpContextAccessor _contextAccessor;

    public MessagesBetweenUsersHandler(ICassandraService cassandraService, UserManager<Entities.AppUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _cassandraService = cassandraService;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<Response> Handle(MessagesBetweenUsersQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        Entities.AppUser? user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        Entities.AppUser? receiver = await _userManager.FindByNameAsync(request.Receiver);
        if (receiver is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        CassandraPaginatedResult res = await _cassandraService.GetMessagesBetweenUsersAsync(user.Id, receiver.Id, request.PagingState);

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = res
        };
    }
}
