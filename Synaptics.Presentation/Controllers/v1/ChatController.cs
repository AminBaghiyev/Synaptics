using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Common;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.Message.MessagesBetweenUsers;
using System.Net;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/chat")]
[ApiController]
public class ChatController : ControllerBase
{
    readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Response> Messages(MessagesBetweenUsersQuery query)
    {
        try
        {
            Response response = await _mediator.Send(query);
            HttpContext.Response.StatusCode = (int)response.StatusCode;
            return response;
        }
        catch (ExternalException ex)
        {
            HttpContext.Response.StatusCode = 400;
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = ex.Message
            };
        }
        catch (Exception)
        {
            HttpContext.Response.StatusCode = 500;
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = "Something went wrong!"
            };
        }
    }
}
