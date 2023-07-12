using System.Net;
using DotForum.Application.Enums;

namespace DotForum.Application.Models.Common;

public class AppResponse<T>
{
    public T Data { get; set; }
    public Status Status { get; set; }
}

public class Status
{
    public StatusCode Code { get; set; }
    public string? Message { get; set; }
}