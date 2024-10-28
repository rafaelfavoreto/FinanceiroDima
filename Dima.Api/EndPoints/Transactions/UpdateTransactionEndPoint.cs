using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Transactions;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Transactions;

public class UpdateTransactionEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Transaction: Update")
            .WithSummary("Atualizar uma atualiza transação")
            .WithDescription("Atualizar uma atualiza transação")
            .WithOrder(2)
            .Produces<BaseResponse<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        UpdateTransactionRequest request,
        long id)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        request.Id = id;
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}