using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.Post.CreatePost;
using Synaptics.Application.Commands.Post.HardDeletePost;
using Synaptics.Application.Commands.Post.LikePost;
using Synaptics.Application.Commands.Post.RecoverPost;
using Synaptics.Application.Commands.Post.SoftDeletePost;
using Synaptics.Application.Commands.Post.UnlikePost;
using Synaptics.Application.Commands.Post.UpdatePost;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.Post.LikesOfPost;
using Synaptics.Application.Queries.Post.PostForUpdate;
using Synaptics.Application.Queries.Post.PostOfUser;
using Synaptics.Application.Queries.Post.PostsOfCurrentUser;
using Synaptics.Application.Queries.Post.PostsOfUser;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyPosts([FromQuery] PostsOfCurrentUserQuery command)
    {
        try
        {
            return Ok(await _mediator.Send(command));
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

    [HttpGet("my/{id}")]
    public async Task<IActionResult> MyPost([FromRoute] long id)
    {
        try
        {
            return Ok(await _mediator.Send(new PostOfCurrentUserQuery { Id = id }));
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

    [HttpGet("{username}")]
    public async Task<IActionResult> Posts([FromRoute] string username, [FromQuery] int page)
    {
        try
        {
            return Ok(await _mediator.Send(new PostsOfUserQuery { UserName = username, Page = page }));
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

    [HttpGet("{username}/{id}")]
    public async Task<IActionResult> Post(string username, long id)
    {
        try
        {
            return Ok(await _mediator.Send(new PostOfUserQuery { UserName = username, Id = id}));
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

    [HttpGet("{username}/{id}/likes")]
    public async Task<IActionResult> LikesOfPost(string username, long id, [FromQuery] int page)
    {
        try
        {
            return Ok(await _mediator.Send(new LikesOfPostQuery { UserName = username, PostId = id, Page = page }));
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

    [HttpPost("{username}/{id}/like")]
    public async Task<IActionResult> LikePost(string username, long id)
    {
        try
        {
            await _mediator.Send(new LikePostCommand { UserName = username, PostId = id });
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

    [HttpPost("{username}/{id}/unlike")]
    public async Task<IActionResult> UnlikePost(string username, long id)
    {
        try
        {
            await _mediator.Send(new UnlikePostCommand { UserName = username, PostId = id });
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

    [HttpGet("my/update/{id}")]
    public async Task<IActionResult> MyPosts([FromRoute] long id)
    {
        try
        {
            return Ok(await _mediator.Send(new PostForUpdateQuery { Id = id}));
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDTO post)
    {
        try
        {
            await _mediator.Send(new CreatePostCommand { Post = post });
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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePostDTO post)
    {
        try
        {
            await _mediator.Send(new UpdatePostCommand { Post = post });
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

    [HttpPut("my/recover/{id}")]
    public async Task<IActionResult> MyPostRecover([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new RecoverPostCommand { Id = id });
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

    [HttpPut("my/delete/{id}")]
    public async Task<IActionResult> MyPostSoftDelete([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new SoftDeletePostCommand { Id = id });
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

    [HttpDelete("my/delete/{id}")]
    public async Task<IActionResult> MyPostHardDelete([FromRoute] long id)
    {
        try
        {
            await _mediator.Send(new HardDeletePostCommand { Id = id });
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
