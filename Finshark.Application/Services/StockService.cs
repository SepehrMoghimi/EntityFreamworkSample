using Finshark.Application.Model.Requests;
using Finshark.Application.Model.Responses;
using Finshark.Domain.Entities;
using Finshark.Domain.Interface.Base;
using Finshark.Domain.ValueObjects;
using Finshark.Infrastructure.Repositories;

namespace Finshark.Application.Services;

public class StockService : IScopedService
{
    private readonly StockRepository _stockRepository;

    public StockService(StockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<List<StockResponse>> GetAllAsync(StockQueryRequest request)
    {
        var query = new StockQuery
        {
            Symbol = request.Symbol,
            CompanyName = request.CompanyName,
            SortBy = request.SortBy,
            IsDecsending = request.IsDecsending,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var stocks = await _stockRepository.GetAllAsync(query);
        return stocks.Select(MapToResponse).ToList();
    }

    public async Task<StockResponse?> GetByIdAsync(int id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);
        return stock == null ? null : MapToResponse(stock);
    }

    public async Task<StockResponse> CreateAsync(CreateStockRequest request)
    {
        var stock = new Stock
        {
            Symbol = request.Symbol,
            CompanyName = request.CompanyName,
            Purchase = request.Purchase,
            LastDiv = request.LastDiv,
            Industry = request.Industry,
            MarketCap = request.MarketCap
        };

        var created = await _stockRepository.CreateAsync(stock);
        return MapToResponse(created!);
    }

    public async Task<StockResponse?> UpdateAsync(int id, UpdateStockRequest request)
    {
        var stock = new Stock
        {
            Symbol = request.Symbol,
            CompanyName = request.CompanyName,
            Purchase = request.Purchase,
            LastDiv = request.LastDiv,
            Industry = request.Industry,
            MarketCap = request.MarketCap
        };

        var updated = await _stockRepository.UpdateAsync(id, stock);
        return updated == null ? null : MapToResponse(updated);
    }

    public async Task<StockResponse?> DeleteAsync(int id)
    {
        var deleted = await _stockRepository.DeleteAsync(id);
        return deleted == null ? null : MapToResponse(deleted);
    }

    public Task<bool> StockExistsAsync(int id) => _stockRepository.StockExists(id);

    private static StockResponse MapToResponse(Stock stock)
    {
        return new StockResponse
        {
            ID = stock.ID,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            LastDiv = stock.LastDiv,
            Industry = stock.Industry,
            MarketCap = stock.MarketCap,
            Comments = stock.Comments.Select(c => new CommentResponse
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                StockId = c.StockId
            }).ToList()
        };
    }
}
