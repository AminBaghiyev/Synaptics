using Synaptics.Domain.Enums;
using System.Net;

namespace Synaptics.Application.Common;

public class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public MessageCode? MessageCode { get; set; }
    public object? Data { get; set; }
}
