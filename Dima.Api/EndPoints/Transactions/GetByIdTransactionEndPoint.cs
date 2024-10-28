using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Transactions;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Transactions;

public class GetByIdTransactionEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: GetById")
            .WithSummary("Obter uma transação")
            .WithDescription("Obter uma transação")
            .WithOrder(4)
            .Produces<BaseResponse<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        long id)
    {
        var request = new GetByIdTransactionRequest()
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