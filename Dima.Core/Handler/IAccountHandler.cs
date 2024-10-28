using Dima.Core.Request.Account;
using Dima.Core.Responses;

namespace Dima.Core.Handler;

public interface IAccountHandler
{
    Task<BaseResponse<string>> LoginAsync(LoginRequest request);
    Task<BaseResponse<string>> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}