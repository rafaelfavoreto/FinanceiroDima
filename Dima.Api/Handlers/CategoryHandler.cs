using Dima.Api.Data;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly AppDbContext _appDbContext;

    public CategoryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<BaseResponse<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
            };

            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();

            return new BaseResponse<Category?>(category,201,"Categoria criada com sucesso");
        }
        catch
        {
            return new BaseResponse<Category?>(null, 500, "Não foi possivel criar uma Categoria");
        }
    }

    public async Task<BaseResponse<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await _appDbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                return new BaseResponse<Category?>(null, 404, "Categoria não encontrada");

            category.Title = request.Title;
            category.Description = request.Description;

            _appDbContext.Categories.Update(category);
            await _appDbContext.SaveChangesAsync();

            return new BaseResponse<Category?>(category,message:"Categoria atualizar com sucesso.");
        }
        catch
        {
            return new BaseResponse<Category?>(null, 500, "Não foi possivel alterar uma Categoria");
        }
    }

    public async Task<BaseResponse<Category?>> DeleteAsync(DeleteCaterotyRequest request)
    {
        try
        {
            var category = await _appDbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == request.CaterotyId && x.UserId == request.UserId);

            if (category == null)
                return new BaseResponse<Category?>(null, 404, "Categoria não encontrada");

            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();

            return new BaseResponse<Category?>(category,200, "Categoria excluida com sucesso.");
        }
        catch
        {
            return new BaseResponse<Category?>(null, 500, "Não foi excluir uma Categoria");
        }
    }

    public async Task<BaseResponse<Category?>> GetByIdAsync(GetByIdCategoryRequest request)
    {
        var category = await _appDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

        return category is null
            ? new BaseResponse<Category?>(null, 404, "Categoria não encontrada")
            : new BaseResponse<Category?>(category);
    }

    public async Task<PageResponse<List<Category>>> GetAllAsync(GetAllCategoryRequest request)
    {
        try
        {
            var query = _appDbContext.Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1 ) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PageResponse<List<Category>>(categories, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PageResponse<List<Category>>(null, 500, "Não buscar todas as Categoria");
        }
        
    }
}