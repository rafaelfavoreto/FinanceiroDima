using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Configurations;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request;
using Dima.Core.Request.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories;

public class GetAllCategoryEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", HandleAsync)
            .WithName("Categories: GetAll")
            .WithSummary("Obter uma lista categoria")
            .WithDescription("Obter uma lista categoria")
            .WithOrder(5)
            .Produces<PageResponse<List<Category?>>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        [FromQuery]int pageNumber = Configuration.PageNumber,
        [FromQuery]int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllCategoryRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        var result = await handler.GetAllAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}