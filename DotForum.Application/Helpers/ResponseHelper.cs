using System.Net;
using DotForum.Application.Enums;
using DotForum.Application.Models.Common;

namespace DotForum.Application.Helpers;

public static class ResponseHelper
{
    public static AppResponse<T> Fail<T>(StatusCode statusCode = StatusCode.Error, string? message = null, T? data = null)
        where T : class
    {
        var result = new AppResponse<T>()
        {
            Data = data,
            Status = new Status()
            {
                Code = statusCode,
                Message = message ?? "Failed"
            },
        };

        return result;
    }

    public static AppResponse<EmptyResponse> Fail(StatusCode statusCode = StatusCode.Error, string? message = null)
    {
        return Fail<EmptyResponse>(statusCode, message);
    }

    public static AppResponse<T> Ok<T>(T data, string? message = null)
        where T : class
    {
        var result = new AppResponse<T>()
        {
            Data = data,
            Status = new Status()
            {
                Code = StatusCode.Success,
                Message = message
            }
        };

        return result;
    }

    public static AppResponse<EmptyResponse> Ok(string? message = null)
    {
        return Ok(default(EmptyResponse), message);
    }
}