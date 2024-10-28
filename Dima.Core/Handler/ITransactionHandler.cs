

using Dima.Core.Models;
using Dima.Core.Request.Transactions;
using Dima.Core.Responses;

namespace Dima.Core.Handler;

public interface ITransactionHandler
{
    Task<BaseResponse<Transaction?>> CreateAsync(CreateTransactionRequest  request);
    Task<BaseResponse<Transaction?>> UpdateAsync(UpdateTransactionRequest  request);
    Task<BaseResponse<Transaction?>> DeleteAsync(DeleteTransactionRequest  request);
    Task<BaseResponse<Transaction?>> GetByIdAsync(GetByIdTransactionRequest  request);
    Task<PageResponse<List<Transaction>?>> GetPeriodAsync(GetByPeriodTransactionRequest request);
}