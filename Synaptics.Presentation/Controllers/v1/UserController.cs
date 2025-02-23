using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;
using Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;
using Synaptics.Application.Commands.AppUser.ChangePasswordAppUser;
using Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;
using Synaptics.Application.Commands.AppUser.LoginAppUser;
using Synaptics.Application.Commands.AppUser.LogoutAppUser;
using Synaptics.Application.Commands.AppUser.RegisterAppUser;
using Synaptics.Application.Commands.AppUser.ResetPasswordAppUser;
using Synaptics.Application.Commands.AppUser.SendResetPasswordAppUser;
using Synaptics.Application.Commands.UserRelation.FollowUser;
using Synaptics.Application.Commands.UserRelation.RemoveFollower;
using Synaptics.Application.Commands.UserRelation.UnfollowUser;
using Synaptics.Application.Common;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Queries.AppUser.GetAccessTokenAppUser;
using Synaptics.Application.Queries.AppUser.GetAppUserInfo;
using Synaptics.Application.Queries.AppUser.GetAppUserProfile;
using Synaptics.Application.Queries.UserRelation.Followers;
using Synaptics.Application.Queries.UserRelation.Followings;
using System.Net;

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

    [HttpGet]
    public async Task<Response> Profile([FromQuery] GetAppUserProfileQuery query)
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

    [HttpGet("followers")]
    public async Task<Response> UserFollowers([FromQuery] FollowersQuery query)
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

    [HttpGet("followings")]
    public async Task<Response> UserFollowings([FromQuery] FollowingsQuery query)
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

    [HttpGet("getUserInfo")]
    public async Task<Response> GetUserInfo()
    {
        try
        {
            Response response = await _mediator.Send(new GetAppUserInfoQuery());
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

    [HttpGet("accessToken")]
    public async Task<Response> GetAccessToken([FromQuery] GetAccessTokenAppUserQuery query)
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

    [HttpPost("logout")]
    public async Task<Response> Logout([FromBody] LogoutAppUserCommand command)
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

    [HttpPost("login")]
    public async Task<Response> Login([FromBody] LoginAppUserCommand command)
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

    [HttpPost("register")]
    public async Task<Response> Register([FromForm] RegisterAppUserCommand command)
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

    [HttpPost("resetPasswordRequest")]
    public async Task<Response> ResetPassswordRequest([FromBody] SendResetPasswordAppUserCommand command)
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

    [HttpPost("resetPassword")]
    public async Task<Response> ResetPasssword([FromBody] ResetPasswordAppUserCommand command)
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

    [HttpPut("changePassword")]
    public async Task<Response> ChangePassword([FromBody] ChangePasswordAppUserCommand command)
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

    [HttpPut("changeProfilePhoto")]
    public async Task<Response> ChangeProfilePhoto([FromForm] ChangeProfilePhotoAppUserCommand command)
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

    [HttpPut("changeCoverPhoto")]
    public async Task<Response> ChangeCoverPhoto([FromForm] ChangeCoverPhotoAppUserCommand command)
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

    [HttpPut("changeUserInfo")]
    public async Task<Response> ChangeUserInfo([FromBody] ChangeAppUserInfoCommand command)
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

    [HttpPost("follow")]
    public async Task<Response> FollowUser([FromBody] FollowUserCommand command)
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

    [HttpPost("unfollow")]
    public async Task<Response> UnfollowUser([FromBody] UnfollowUserCommand command)
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

    [HttpPost("removefollow")]
    public async Task<Response> RemoveFollower([FromBody] RemoveFollowerCommand command)
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
