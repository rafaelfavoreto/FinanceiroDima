using Dima.Api.Data;
using Dima.Core.Common;
using Dima.Core.Enuns;
using Dima.Core.Handler;
using Dima.Core.Models;
using Dima.Core.Request.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly AppDbContext _appDbContext;

    public TransactionHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<BaseResponse<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreateAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type,
            };

            await _appDbContext.Transactions.AddAsync(transaction);
            await _appDbContext.SaveChangesAsync();

            return new BaseResponse<Transaction?>(transaction, 201, "Transação criada com sucesso");
        }
        catch 
        {
            return new BaseResponse<Transaction?>(null, 500, "Não foi possivel criar uma Transação");
        }
        
    }

    public async Task<BaseResponse<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        try
        {
            var transaction = await
                _appDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new BaseResponse<Transaction?>(null, 404, "Transação não encontrada");

            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = DateTime.Now;
            transaction.Title = request.Title;
            transaction.Type = request.Type;

             _appDbContext.Update(transaction);
             await _appDbContext.SaveChangesAsync();

             return new BaseResponse<Transaction?>(transaction, message: "Transação alterada com sucesso!");
        }
        catch
        {
            return new BaseResponse<Transaction?>(null, 500, "Não foi possivel alterar a transação");
        }
      
    }

    public async Task<BaseResponse<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await
                _appDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new BaseResponse<Transaction?>(null, 404, "Transação não encontrada");

            _appDbContext.Transactions.Remove(transaction);
            await _appDbContext.SaveChangesAsync();

            return new BaseResponse<Transaction?>(transaction, message: "Transação excluída com sucesso!");
        }
        catch
        {
            return new BaseResponse<Transaction?>(null, 500, "Não foi excluir uma Transação");
        }
    }

    public async Task<BaseResponse<Transaction?>> GetByIdAsync(GetByIdTransactionRequest request)
    {
        try
        {
            var transaction = await
                _appDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return transaction is null
                ? new BaseResponse<Transaction?>(null, 404, "Transação não encontrada")
                : new BaseResponse<Transaction?>(transaction);
        }
        catch 
        {
            return new BaseResponse<Transaction?>(null, 500, "Não foi excluir uma Transação");
        }
    }

    public async Task<PageResponse<List<Transaction>?>> GetPeriodAsync(GetByPeriodTransactionRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch 
        {
            return new PageResponse<List<Transaction?>>(null, 500, "Não foi converter as datas de início e fim do período");
        }

        try
        {
            var query = _appDbContext.Transactions
                .AsNoTracking()
                .Where(x => x.PaidOrReceivedAt == request.StartDate
                            && x.PaidOrReceivedAt == request.EndDate
                            && x.UserId == request.UserId)
                .OrderBy(x => x.PaidOrReceivedAt);

            var count = await query.CountAsync();

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageNumber)
                .Take(request.PageSize)
                .ToListAsync();

            return new PageResponse<List<Transaction?>>(transactions!, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PageResponse<List<Transaction?>>(null, 500, "Não foi possivel buscar as transações por período");
        }
       
    }
}