using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;
using Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;
using Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;
using Synaptics.Application.Commands.AppUser.LoginAppUser;
using Synaptics.Application.Commands.AppUser.RegisterAppUser;
using Synaptics.Application.Queries.AppUser.SearchAppUser;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;
using Synaptics.Application.Queries.AppUser.GetAppUserProfile;
using Synaptics.Application.Queries.AppUser.GetAppUserInfo;

namespace Synaptics.Presentation.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        try
        {
            return Ok(await _mediator.Send(new SearchAppUserCommand { Query = query }));
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
            return Ok(await _mediator.Send(new GetAppUserProfileCommand { UserName = username }));
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
            return Ok(await _mediator.Send(new GetAppUserInfoCommand()));
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
}
