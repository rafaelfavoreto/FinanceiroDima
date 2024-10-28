using Dima.Api.Common.Api;
using Dima.Core.Configurations;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using Dima.Core.Request.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Transactions;

public class GetByPeriodTransactionEndPoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("", HandleAsync)
            .WithName("Transactions: GetAll")
            .WithSummary("Obter uma lista transações")
            .WithDescription("Obter uma lista transações")
            .WithOrder(5)
            .Produces<PageResponse<List<Transaction?>>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = Configuration.PageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetByPeriodTransactionRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            PageSize = pageSize,
            PageNumber = pageNumber,
            StartDate = startDate,
            EndDate = endDate,
        };

        var result = await handler.GetPeriodAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}