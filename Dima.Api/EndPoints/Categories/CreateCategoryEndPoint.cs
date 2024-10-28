using System.Security.Claims;
using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;

namespace Dima.Api.EndPoints.Categories;

public class CreateCategoryEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Cria uma nova categoria")
            .WithDescription("Cria uma nova categoria")
            .WithOrder(1)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        CreateCategoryRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.CreateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data?.Id}", result) 
            : TypedResults.BadRequest(result);
    }
}