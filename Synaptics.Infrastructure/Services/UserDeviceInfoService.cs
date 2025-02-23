using Microsoft.AspNetCore.Http;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces.Services;

namespace Synaptics.Infrastructure.Services;

public class UserDeviceInfoService : IUserDeviceInfoService
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public UserDeviceInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DeviceInfo GetDeviceInfo()
    {
        var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        return new DeviceInfo
        {
            DeviceName = ExtractDeviceName(userAgent),
            OperatingSystem = ExtractOperatingSystem(userAgent),
            IpAddress = ipAddress,
        };
    }

    public string ExtractDeviceName(string userAgent)
    {
        if (userAgent.Contains("Windows"))
            return "Windows";
        if (userAgent.Contains("Mac"))
            return "Mac";
        if (userAgent.Contains("iPhone"))
            return "iPhone";
        if (userAgent.Contains("Android"))
            return "Android";

        return "Unknown Device";
    }

    public string ExtractOperatingSystem(string userAgent)
    {
        if (userAgent.Contains("Windows"))
            return "Windows";
        if (userAgent.Contains("Mac"))
            return "Mac OS";
        if (userAgent.Contains("Linux"))
            return "Linux";
        if (userAgent.Contains("Android"))
            return "Android";
        if (userAgent.Contains("iOS"))
            return "iOS";

        return "Unknown OS";
    }
}
