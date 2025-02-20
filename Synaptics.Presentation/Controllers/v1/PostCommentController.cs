using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.Post.CreatePostComment;
using Synaptics.Application.Commands.Post.LikePostComment;
using Synaptics.Application.Commands.Post.SoftDeletePostComment;
using Synaptics.Application.Commands.Post.UnlikePostComment;
using Synaptics.Application.Commands.Post.UpdatePostComment;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.Post.CommentsOfPost;
using Synaptics.Application.Queries.Post.PostCommentForUpdate;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/post/comment")]
[ApiController]
public class PostCommentController : ControllerBase
{
    readonly IMediator _mediator;

    public PostCommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments([FromQuery] long postId, int page)
    {
        try
        {
            return Ok(await _mediator.Send(new CommentsOfPostQuery { PostId = postId, Page = page }));
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
    public async Task<IActionResult> GetCommentForUpdate([FromRoute] long id)
    {
        try
        {
            return Ok(await _mediator.Send(new PostCommentForUpdateQuery { Id = id }));
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
    public async Task<IActionResult> AddComment([FromBody] CreatePostCommentDTO comment)
    {
        try
        {
            await _mediator.Send(new CreatePostCommentCommand { Comment = comment });
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
    public async Task<IActionResult> EditComment([FromBody] UpdatePostCommentDTO comment)
    {
        try
        {
            await _mediator.Send(new UpdatePostCommentCommand { Comment = comment });
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
    public async Task<IActionResult> LikeComment([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new LikePostCommentCommand { CommentId = id });
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
    public async Task<IActionResult> UnlikeComment([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new UnlikePostCommentCommand { CommentId = id });
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
            await _mediator.Send(new SoftDeletePostCommentCommand { Id = id });
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
