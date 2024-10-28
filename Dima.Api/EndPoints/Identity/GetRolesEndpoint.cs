﻿using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.EndPoints.Identity;

public class GetRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/roles", Handle)
            .RequireAuthorization();

    private static Task<IResult> Handle(
        ClaimsPrincipal claims)
    {
        if (claims.Identity is not null || claims.Identity!.IsAuthenticated)
            return Task.FromResult(Results.Unauthorized());

        var identity = (ClaimsIdentity)claims.Identity;

        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(c => new RoleClaim
            {
              Issuer  = c.Issuer,
              OriginalIssuer  = c.OriginalIssuer,
              Type = c.Type,
              Value = c.Value,
              ValueType = c.ValueType
            });

        return Task.FromResult<IResult>(TypedResults.Json(roles));
    }
}