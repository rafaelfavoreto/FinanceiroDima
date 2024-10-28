using System.Net.Http.Json;
using System.Text;
using Dima.Core.Handler;
using Dima.Core.Request.Account;
using Dima.Core.Responses;  

namespace Dima.Web.Handler;

public class AccountHandler : IAccountHandler 
{
    private static IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _client;
    public AccountHandler(IHttpClientFactory httpClientFactory, HttpClient client)
    {
        _httpClientFactory = httpClientFactory;
        _client = _httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<BaseResponse<string>> LoginAsync(LoginRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return result.IsSuccessStatusCode
            ? new BaseResponse<string>("Login realizaedo com sucesso", 200, "Login realizaedo com sucesso")
            : new BaseResponse<string>("Não foi possível realizar o login", 400, "Não foi possível realizar o login");
    }

    public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/register", request);
        return result.IsSuccessStatusCode
            ? new BaseResponse<string>("Cadastro realizaedo com sucesso", 201, "Cadastro realizaedo com sucesso")
            : new BaseResponse<string>("Não foi possível realizar o cadastro", 400, "Não foi possível realizar o cadastro");
    }
    

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, mediaType: "application/json");
        await _client.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }
}