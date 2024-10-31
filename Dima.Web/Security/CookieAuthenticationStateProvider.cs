using System.Net.Http.Json;
using System.Security.Claims;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider : 
    AuthenticationStateProvider,
    ICookieAuthenticationStateProvider
{
    private readonly HttpClient _client;
    private bool _isAuthenticaded = false;

    public CookieAuthenticationStateProvider(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _isAuthenticaded;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _isAuthenticaded = false;

        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var userInfo = await GetUser();

        if (userInfo is null) 
            return new AuthenticationState(user);

        var claims = await GetClaims(userInfo);
        var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
        user = new ClaimsPrincipal(id);

        _isAuthenticaded = true;
        return new AuthenticationState(user);
    }

    private async Task<User?> GetUser()
    {
        try
        {
            return await _client.GetFromJsonAsync<User>("v1/identity/manage/info");
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(
            user.Claims.Where(x => 
                    x.Key != ClaimTypes.Name
                    && x.Key != ClaimTypes.Email)
                .Select(x=> new Claim(x.Key,x.Value)));

        RoleClaim[]? roles;

        try
        {
            roles = await _client.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
        }
        catch
        {
            return claims;
        }

        claims.AddRange(
            from role in roles! 
            where !string.IsNullOrEmpty(role.Type ) || !string.IsNullOrEmpty(role.Type) 
            select new Claim(role.Type, role.Value!, role.ValueType, role.Issuer, role.OriginalIssuer));


        return claims;
    }

    public void NotifyAuthenticationStateChanged()
    => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    
}