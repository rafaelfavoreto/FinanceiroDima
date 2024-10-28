

using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Transactions;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Transactions;

public class DeleteTransactionEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactios: Delete")
            .WithSummary("Cria uma deleta uma transação")
            .WithDescription("Cria uma delete uma transação")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        long id)
    {
        var request = new DeleteTransactionRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            Id = id
        };

        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}