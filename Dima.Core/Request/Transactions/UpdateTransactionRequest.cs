using Dima.Core.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Request.Transactions;

public class UpdateTransactionRequest : BaseRequest
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Título inválido")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo inválido")]
    public ETransactionType Type { get; set; }

    [Required(ErrorMessage = "Valor inválido")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Categoria inválido")]
    public long CategoryId { get; set; }
    public DateTime? PaidOrReceivedAt { get; set; }
}