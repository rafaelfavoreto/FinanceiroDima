using Dima.Api;
using Dima.Api.Common.Api;
using Dima.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);
builder.ManagerConfigurationBuilder();
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvionment();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.UseSecurity();
app.MapEndpoints();

app.Run();

