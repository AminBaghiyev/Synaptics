using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.PostCommentReply.CreatePostCommentReply;
using Synaptics.Application.Commands.PostCommentReply.LikePostCommentReply;
using Synaptics.Application.Commands.PostCommentReply.SoftDeletePostCommentReply;
using Synaptics.Application.Commands.PostCommentReply.UnlikePostCommentReply;
using Synaptics.Application.Commands.PostCommentReply.UpdatePostCommentReply;
using Synaptics.Application.Common;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;
using Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;
using System.Net;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/post/comment/reply")]
[ApiController]
public class PostCommentReplyController : ControllerBase
{
    readonly IMediator _mediator;

    public PostCommentReplyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    public async Task<Response> Replies([FromQuery] RepliesOfPostCommentQuery query)
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

    [HttpGet("update")]
    public async Task<Response> ReplyForUpdate([FromQuery] PostCommentReplyForUpdateQuery query)
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

    [HttpPost("add")]
    public async Task<Response> Add([FromBody] CreatePostCommentReplyCommand command)
    {
        try
        {
            Response response = await _mediator.Send(command);
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

    [HttpPut("edit")]
    public async Task<Response> Edit([FromBody] UpdatePostCommentReplyCommand command)
    {
        try
        {
            Response response = await _mediator.Send(command);
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

    [HttpPost("like")]
    public async Task<Response> Like([FromBody] LikePostCommentReplyCommand command)
    {
        try
        {
            Response response = await _mediator.Send(command);
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

    [HttpPost("unlike")]
    public async Task<Response> Unlike([FromBody] UnlikePostCommentReplyCommand command)
    {
        try
        {
            Response response = await _mediator.Send(command);
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

    [HttpDelete("delete")]
    public async Task<Response> Delete([FromBody] SoftDeletePostCommentReplyCommand command)
    {
        try
        {
            Response response = await _mediator.Send(command);
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
