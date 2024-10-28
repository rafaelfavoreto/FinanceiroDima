using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core.Configurations;
using Dima.Core.Handler;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace Dima.Api.Common.Api;

public static class BuilderExtension
{
    public static void ManagerConfigurationBuilder(this WebApplicationBuilder builder)
    {
        builder.AddConfiguration();
        builder.AddSecurity();
        builder.AddDataContexts();
        builder.AddCrossOrigin();
        builder.AddDocumentation();
        builder.AddServices();
    }

    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
      Configuration.ConnectionString =  builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty ;
      Configuration.FrontendUrl =  builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty ;
      Configuration.BackendUrl =  builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty ;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => x.CustomSchemaIds(n => n.FullName));
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        builder.Services.AddAuthorization();
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.ConnectionString));

        builder.Services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(ApiConfiguration.CorsPolicyName,
            policy => policy
                .WithOrigins(
                    Configuration.FrontendUrl,
                    Configuration.BackendUrl)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            ));
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }
}