using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.DTOs.stock;
using api.Helpers;
using api.Interfaces;
using api.models;
using Dapper;

namespace api.Respository
{
    public class StockRepository : IStockRepository
    {
        private readonly IDapperContext _context;

        public StockRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<Stock?> CreateAsync(Stock stockModel)
        {
            const string query = @"
                INSERT INTO Stocks (Symbol, CompanyName, Purchase, LastDiv, Industry, MarketCap)
                VALUES (@Symbol, @CompanyName, @Purchase, @LastDiv, @Industry, @MarketCap);
                SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            var id = await connection.QuerySingleAsync<int>(query, stockModel);
            stockModel.ID = id;
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await GetByIdAsync(id);
            if (stock == null)
                return null;

            const string query = "DELETE FROM Stocks WHERE ID = @id";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { id });
            return stock;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var sqlQuery = "SELECT * FROM Stocks WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                sqlQuery += " AND CompanyName LIKE @CompanyName";
                parameters.Add("@CompanyName", $"%{query.CompanyName}%");
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                sqlQuery += " AND Symbol LIKE @Symbol";
                parameters.Add("@Symbol", $"%{query.Symbol}%");
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    sqlQuery += query.IsDecsending ? " ORDER BY Symbol DESC" : " ORDER BY Symbol ASC";
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            sqlQuery += $" LIMIT {query.PageSize} OFFSET {skipNumber}";

            using var connection = _context.CreateConnection();
            var stocks = (await connection.QueryAsync<Stock>(sqlQuery, parameters)).ToList();

            foreach (var stock in stocks)
            {
                const string commentsQuery = "SELECT * FROM Comments WHERE StockId = @StockId";
                var comments = (await connection.QueryAsync<Comment>(commentsQuery, new { StockId = stock.ID })).ToList();
                stock.Comments = comments;
            }

            return stocks;
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            const string query = "SELECT * FROM Stocks WHERE ID = @id";
            using var connection = _context.CreateConnection();
            var stock = await connection.QuerySingleOrDefaultAsync<Stock>(query, new { id });

            if (stock == null)
                return null;

            const string commentsQuery = "SELECT * FROM Comments WHERE StockId = @StockId";
            var comments = (await connection.QueryAsync<Comment>(commentsQuery, new { StockId = stock.ID })).ToList();
            stock.Comments = comments;

            return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            const string query = "SELECT COUNT(1) FROM Stocks WHERE ID = @id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, new { id }) > 0;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await GetByIdAsync(id);
            if (existingStock == null)
                return null;

            const string query = @"
                UPDATE Stocks
                SET Symbol = @Symbol, CompanyName = @CompanyName, Purchase = @Purchase,
                    LastDiv = @LastDiv, Industry = @Industry, MarketCap = @MarketCap
                WHERE ID = @id";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                id,
                stockDto.Symbol,
                stockDto.CompanyName,
                stockDto.Purchase,
                stockDto.LastDiv,
                stockDto.Industry,
                stockDto.MarketCap
            });

            return await GetByIdAsync(id);
        }
    }
}
