using System.ComponentModel.DataAnnotations;
using Dima.Core.Enuns;

namespace Dima.Core.Request.Transactions;

public class CreateTransactionRequest : BaseRequest
{
    [Required(ErrorMessage = "Título inválido")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo inválido")]
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

    [Required(ErrorMessage = "Valor inválido")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Categoria inválido")]
    public long CategoryId { get; set; }
    public DateTime? PaidOrReceivedAt { get; set; }
}