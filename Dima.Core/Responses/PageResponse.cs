using Dima.Core.Configurations;
using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class PageResponse<TData> : BaseResponse<TData>
{
    [JsonConstructor]
    public PageResponse(
        TData? data, 
        int totalCount,
        int currentPage = 1,
        int pageSize = Configuration.PageNumber) : base(data)
    {
        Data = data;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    public PageResponse(
        TData? data,
        int statusCode = Configuration.DefaultCode,
        string? message = null) : base(data,statusCode,message) 
    { }

    public int CurrentPage { get; set; }
    public int TotalPage => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
