using MediatR;
using Synaptics.Application.Common;

namespace Synaptics.Application.Queries.Post.PostForUpdate;

public record PostForUpdateQuery : IRequest<Response>
{
    public long Id { get; set; }
}
