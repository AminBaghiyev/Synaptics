using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.Post.CreatePostCommentReply;
using Synaptics.Application.Commands.Post.LikePostCommentReply;
using Synaptics.Application.Commands.Post.SoftDeletePostCommentReply;
using Synaptics.Application.Commands.Post.UnlikePostCommentReply;
using Synaptics.Application.Commands.Post.UpdatePostCommentReply;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;
using Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;

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

    [HttpGet]
    public async Task<IActionResult> GetReplies([FromQuery] long parentId, int page)
    {
        try
        {
            return Ok(await _mediator.Send(new RepliesOfPostCommentQuery { ParentId = parentId, Page = page }));
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReplyForUpdate([FromRoute] long id)
    {
        try
        {
            return Ok(await _mediator.Send(new PostCommentReplyForUpdateQuery { Id = id }));
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddReply([FromBody] CreatePostCommentReplyDTO reply)
    {
        try
        {
            await _mediator.Send(new CreatePostCommentReplyCommand { Reply = reply });
            return Ok();
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditReply([FromBody] UpdatePostCommentReplyDTO reply)
    {
        try
        {
            await _mediator.Send(new UpdatePostCommentReplyCommand { Reply = reply });
            return Ok();
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }

    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikeReply([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new LikePostCommentReplyCommand { Id = id });
            return Ok();
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }

    [HttpPost("{id}/unlike")]
    public async Task<IActionResult> UnlikeReply([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new UnlikePostCommentReplyCommand { Id = id });
            return Ok();
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteComment([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new SoftDeletePostCommentReplyCommand { Id = id });
            return Ok();
        }
        catch (ExternalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
        }
    }
}
