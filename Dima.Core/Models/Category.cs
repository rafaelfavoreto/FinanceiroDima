using Dima.Core.Models.Core;

namespace Dima.Core.Models;

public class Category : Entity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}