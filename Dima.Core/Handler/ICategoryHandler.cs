using Dima.Core.Models;
using Dima.Core.Request.Categories;
using Dima.Core.Responses;

namespace Dima.Core.Handler;

public interface ICategoryHandler
{
    Task<BaseResponse<Category?>> CreateAsync(CreateCategoryRequest request);
    Task<BaseResponse<Category?>> UpdateAsync(UpdateCategoryRequest request);
    Task<BaseResponse<Category?>> DeleteAsync(DeleteCaterotyRequest request);
    Task<BaseResponse<Category?>> GetByIdAsync(GetByIdCategoryRequest request);
    Task<PageResponse<List<Category>>> GetAllAsync(GetAllCategoryRequest request);
}