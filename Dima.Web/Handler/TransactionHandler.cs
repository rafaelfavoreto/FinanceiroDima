using System.Net.Http.Json;
using Dima.Core.Common;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Transactions;
using Dima.Core.Responses;

namespace Dima.Web.Handler;

public class TransactionHandler : ITransactionHandler
{
    private static IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _client;
    public TransactionHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _client = _httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<BaseResponse<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<Transaction?>>()
               ?? new BaseResponse<Transaction?>(null, 400, "Não foi possível criar sua transação");
    }

    public async Task<BaseResponse<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<Transaction?>>()
               ?? new BaseResponse<Transaction?>(null, 400, "Não foi possível atualizar sua transação");
    }

    public async Task<BaseResponse<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<BaseResponse<Transaction?>>()
               ?? new BaseResponse<Transaction?>(null, 400, "Não foi possível excluir sua transação");
    }

    public async Task<BaseResponse<Transaction?>> GetByIdAsync(GetByIdTransactionRequest request)
        => await _client.GetFromJsonAsync<BaseResponse<Transaction?>>($"v1/transactions/{request.Id}")
           ?? new BaseResponse<Transaction?>(null, 400, "Não foi possível obter a transação");

    public async Task<PageResponse<List<Transaction>?>> GetPeriodAsync(GetByPeriodTransactionRequest request)
    {
        const string format = "yyyy-MM-dd";
        var startDate = request.StartDate is not null
            ? request.StartDate.Value.ToString(format)
            : DateTime.Now.GetFirstDay().ToString(format);

        var endDate = request.EndDate is not null
            ? request.EndDate.Value.ToString(format)
            : DateTime.Now.GetLastDay().ToString(format);
        
        var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

        return await _client.GetFromJsonAsync<PageResponse<List<Transaction>?>>(url)
            ?? new PageResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
    }
}