using Azure;
using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories;

public class DeleteCategoryEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Categories: Delete")
            .WithSummary("Cria uma deleta uma categoria")
            .WithDescription("Cria uma delete uma categoria")
            .WithOrder(3)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        long id)
    {
        var request = new DeleteCaterotyRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            CaterotyId = id
        };
        
        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}