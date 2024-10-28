using System.Text.Json.Serialization;
using Dima.Core.Configurations;
using Microsoft.VisualBasic;

namespace Dima.Core.Responses;

public class BaseResponse<TData>
{
    public readonly int StatusCode;
    [JsonConstructor]
    public BaseResponse()
    => StatusCode = Configuration.DefaultCode;

    public BaseResponse(TData? data, int statusCode = Configuration.DefaultCode, string? message = null)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message;
    }

    public TData? Data { get; set; }
    public string Message { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsSuccess => StatusCode is >= Configuration.DefaultCode and <= Configuration.DefaultCodeLast;
}