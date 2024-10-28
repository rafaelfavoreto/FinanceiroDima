using Dima.Api.Common.Api;
using Dima.Api.EndPoints.Categories;
using Dima.Api.EndPoints.Identity;
using Dima.Api.EndPoints.Transactions;
using Dima.Api.Models;

namespace Dima.Api.EndPoints;

public static class EndPoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("/")
            .WithTags("Helth-Check")
            .MapGet("/", () => new { message = "OK" });

        endpoints.MapGroup("v1/categories")
            .WithTags("Categories")
            .RequireAuthorization()
            .MapEndpoint<CreateCategoryEndPoint>()
            .MapEndpoint<UpdateCategoryEndPoint>()
            .MapEndpoint<DeleteCategoryEndPoint>()
            .MapEndpoint<GetByIdCategoryEndPoint>()
            .MapEndpoint<GetAllCategoryEndPoint>();

        endpoints.MapGroup("v1/transactions")
            .WithTags("Transactions")
            .RequireAuthorization()
            .MapEndpoint<CreateTransactionEndPoint>()
            .MapEndpoint<UpdateTransactionEndPoint>()
            .MapEndpoint<DeleteTransactionEndPoint>()
            .MapEndpoint<GetByIdTransactionEndPoint>()
            .MapEndpoint<GetByPeriodTransactionEndPoint>();

        endpoints.MapGroup("v1/identity")
            .WithTags("Identity")
            .MapIdentityApi<User>();

        endpoints.MapGroup("v1/identity")
            .WithTags("Identity")
            .MapEndpoint<LogoutEndpoint>()
            .MapEndpoint<GetRolesEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>( this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}