using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.AppUser.EndSessionAppUser;
using Synaptics.Application.Common;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.AppUser.CurrentSessionsAppUser;
using System.Net;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/user/session")]
[ApiController]
public class SessionController : ControllerBase
{
    readonly IMediator _mediator;

    public SessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    public async Task<Response> Sessions([FromQuery] CurrentSessionsAppUserQuery query)
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

    [HttpPost("end")]
    public async Task<Response> EndSession([FromBody] EndSessionAppUserCommand query)
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
