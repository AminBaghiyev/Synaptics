using Synaptics.Application.Common;

namespace Synaptics.Application.Interfaces.Services;

public interface IUserDeviceInfoService
{
    DeviceInfo GetDeviceInfo();

    string ExtractDeviceName(string userAgent);

    string ExtractOperatingSystem(string userAgent);
}
