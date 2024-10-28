using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Request.Categories;

public class CreateCategoryRequest : BaseRequest
{
    [Required(ErrorMessage = "Título inválido")]
    [MaxLength(ErrorMessage = "O título deve conter até 80 carcteres")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Descrição inválido")]
    public string Description { get; set; } = string.Empty;
}