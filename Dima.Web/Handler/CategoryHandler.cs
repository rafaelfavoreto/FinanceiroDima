using System.Net.Http;
using System.Net.Http.Json;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using Dima.Core.Responses;

namespace Dima.Web.Handler;

public class CategoryHandler: ICategoryHandler
{
    private static IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _client;

    public CategoryHandler(IHttpClientFactory httpClientFactory, HttpClient client)
    {
        _httpClientFactory = httpClientFactory;
        _client = _httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<BaseResponse<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/categories", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<Category?>>()
               ?? new BaseResponse<Category?>(null, 400, "Falha ao criar a categoria");
    }

    public async Task<BaseResponse<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<Category?>>()
               ?? new BaseResponse<Category?>(null, 400, "Falha ao atualizar a categoria");
    }

    public async Task<BaseResponse<Category?>> DeleteAsync(DeleteCaterotyRequest request)
    {
        var result = await _client.DeleteAsync($"v1/categories/{request.CaterotyId}");
        return await result.Content.ReadFromJsonAsync<BaseResponse<Category?>>()
               ?? new BaseResponse<Category?>(null, 400, "Falha ao excluir a categoria");
    }

    public async Task<BaseResponse<Category?>> GetByIdAsync(GetByIdCategoryRequest request)
        => await _client.GetFromJsonAsync<BaseResponse<Category?>>($"v1/categories/{request.Id}")
           ?? new BaseResponse<Category?>(null, 400, "Não foi possível obter a categoria");

    public async Task<PageResponse<List<Category>>> GetAllAsync(GetAllCategoryRequest request)
        => await _client.GetFromJsonAsync<PageResponse<List<Category>>>("v1/categories")
           ?? new PageResponse<List<Category>>(null, 400, "Não foi possível obter as categorias");
}