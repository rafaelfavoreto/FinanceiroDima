using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories;

public class UpdateCategoryEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Categories: Update")
            .WithSummary("Cria uma atualiza categoria")
            .WithDescription("Cria uma atualiza categoria")
            .WithOrder(2)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        UpdateCategoryRequest request,
        long id)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        request.Id = id;
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok( result)
            : TypedResults.BadRequest(result);
    }
}