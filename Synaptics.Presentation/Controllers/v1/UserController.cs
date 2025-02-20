using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;
using Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;
using Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;
using Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;
using Synaptics.Application.Commands.AppUser.LoginAppUser;
using Synaptics.Application.Commands.AppUser.RegisterAppUser;
using Synaptics.Application.Commands.UserRelation.FollowUser;
using Synaptics.Application.Commands.UserRelation.RemoveFollower;
using Synaptics.Application.Commands.UserRelation.UnfollowUser;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.AppUser.AISearchAppUser;
using Synaptics.Application.Queries.AppUser.GetAppUserInfo;
using Synaptics.Application.Queries.AppUser.GetAppUserProfile;
using Synaptics.Application.Queries.AppUser.SearchAppUser;
using Synaptics.Application.Queries.UserRelation.Followers;
using Synaptics.Application.Queries.UserRelation.Followings;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/user")]
[ApiController]
public class UserController : ControllerBase
{
    readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ai-search")]
    public async Task<IActionResult> AISearch([FromQuery] string query, ulong offset)
    {
        try
        {
            return Ok(await _mediator.Send(new AISearchAppUserQuery { Query = query, Offset = offset }));
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

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, int offset)
    {
        try
        {
            return Ok(await _mediator.Send(new SearchAppUserQuery { Query = query, Offset = offset }));
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
    public async Task<IActionResult> Profile([FromRoute] string username)
    {
        try
        {
            return Ok(await _mediator.Send(new GetAppUserProfileQuery { UserName = username }));
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

    [HttpGet("{username}/followers")]
    public async Task<IActionResult> UserFollowers([FromRoute] string username, [FromQuery] int page)
    {
        try
        {
            return Ok(await _mediator.Send(new FollowersQuery { UserName = username, Page = page }));
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

    [HttpGet("{username}/followings")]
    public async Task<IActionResult> UserFollowings([FromRoute] string username, [FromQuery] int page)
    {
        try
        {
            return Ok(await _mediator.Send(new FollowingsQuery { UserName = username, Page = page }));
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

    [HttpGet("getUserInfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            return Ok(await _mediator.Send(new GetAppUserInfoQuery()));
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginAppUserDTO dto)
    {
        try
        {
            return Ok(await _mediator.Send(new LoginAppUserCommand { AppUser = dto }));
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterAppUserDTO dto)
    {
        try
        {
            return Ok(await _mediator.Send(new RegisterAppUserCommand { AppUser = dto }));
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

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordAppUserDTO dto)
    {
        try
        {
            await _mediator.Send(new ChangePasswordAppUserCommand { Passwords = dto });
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

    [HttpPut("changeProfilePhoto")]
    public async Task<IActionResult> ChangeProfilePhoto([FromForm] ChangeProfilePhotoAppUserDTO dto)
    {
        try
        {
            return Ok(await _mediator.Send(new ChangeProfilePhotoAppUserCommand { File = dto }));
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

    [HttpPut("changeCoverPhoto")]
    public async Task<IActionResult> ChangeCoverPhoto([FromForm] ChangeCoverPhotoAppUserDTO dto)
    {
        try
        {
            return Ok(await _mediator.Send(new ChangeCoverPhotoAppUserCommand { File = dto }));
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

    [HttpPut("changeUserInfo")]
    public async Task<IActionResult> ChangeUserInfo([FromBody] ChangeAppUserInfoDTO dto)
    {
        try
        {
            await _mediator.Send(new ChangeAppUserInfoCommand { Info = dto });
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

    [HttpPost("{username}/follow")]
    public async Task<IActionResult> FollowUser([FromRoute] string username)
    {
        try
        {
            await _mediator.Send(new FollowUserCommand { FollowTo = username });
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

    [HttpPost("{username}/unfollow")]
    public async Task<IActionResult> UnfollowUser([FromRoute] string username)
    {
        try
        {
            await _mediator.Send(new UnfollowUserCommand { UnfollowTo = username });
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

    [HttpPost("{username}/removefollow")]
    public async Task<IActionResult> RemoveFollower([FromRoute] string username)
    {
        try
        {
            await _mediator.Send(new RemoveFollowerCommand { Follower = username });
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
