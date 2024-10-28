using System.Security.Claims;
using Azure;
using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.EndPoints.Identity;

public class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/logout", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
}