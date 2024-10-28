using Azure;
using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories;

public class GetByIdCategoryEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: GetById")
            .WithSummary("Obter uma categoria")
            .WithDescription("Obter uma categoria")
            .WithOrder(4)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        long id)
    {
        var request = new GetByIdCategoryRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            Id = id
        };

        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}