using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class ListCategoriesPage : ComponentBase
{
    #region Proprieties

    public bool IsBusy { get; set; } = false;
    public List<Category> Categories { get; set; } = new();

    #endregion

    #region Services

    [Inject] public ICategoryHandler Handler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides
    
    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoryRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
                Categories = result.Data ?? new List<Category>();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    #endregion
}