using Dima.Core.Configurations;

namespace Dima.Core.Request;

public abstract class PagedRequest : BaseRequest
{
    public int PageNumber { get; set; } = Configuration.PageNumber;
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
}