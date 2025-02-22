using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Common;
using Synaptics.Application.Common.Results;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.AppUser.AISearchAppUser;

public class AISearchAppUserHandler : IRequestHandler<AISearchAppUserQuery, Response>
{
    readonly IPyBridgeService _pyBridgeService;
    readonly IQdrantService _qdrantService;
    readonly UserManager<Entities.AppUser> _userManager;

    public AISearchAppUserHandler(IPyBridgeService pyBridgeService, IQdrantService qdrantService, UserManager<Entities.AppUser> userManager)
    {
        _pyBridgeService = pyBridgeService;
        _qdrantService = qdrantService;
        _userManager = userManager;
    }

    public async Task<Response> Handle(AISearchAppUserQuery request, CancellationToken cancellationToken)
    {
        string normalizedQuery = request.SearchQuery.Trim();

        if (normalizedQuery.Length == 0) throw new ExternalException("Invalid query");

        (PyBridgeResult pyRes, float[] queryEmbedding) = await _pyBridgeService.EmbeddingAsync(normalizedQuery);
        if (!pyRes.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        (QdrantResult qdrantRes, ICollection<(string Id, float Score)> usersWithScores) = await _qdrantService.SearchCosineSimilarityAsync("users", queryEmbedding, 10, request.Offset);

        if (!qdrantRes.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        Dictionary<string, Entities.AppUser> users = await _userManager.Users
            .Where(user => usersWithScores.Select(u => u.Id).Contains(user.Id))
            .ToDictionaryAsync(user => user.Id, cancellationToken: cancellationToken);

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = usersWithScores
                .Where(us => users.ContainsKey(us.Id))
                .OrderByDescending(us => us.Score)
                .Select(us => new AISearchAppUserQueryResponse
                {
                    UserName = users[us.Id].UserName,
                    FullName = $"{users[us.Id].FirstName} {users[us.Id].LastName}",
                    ProfilePhotoPath = users[us.Id].ProfilePhotoPath,
                    SelfDescription = users[us.Id].SelfDescription,
                    Score = us.Score
                })
                .ToList()
        };
    }
}
