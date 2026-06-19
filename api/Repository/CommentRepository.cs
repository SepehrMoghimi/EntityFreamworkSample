using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.stock;
using api.Interfaces;
using api.models;
using Dapper;

namespace api.Respository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDapperContext _context;

        public CommentRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<bool> CommentExists(int id)
        {
            const string query = "SELECT COUNT(1) FROM Comments WHERE Id = @id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, new { id }) > 0;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            const string query = @"
                INSERT INTO Comments (Title, Content, CreatedOn, StockId)
                VALUES (@Title, @Content, @CreatedOn, @StockId);
                SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            var id = await connection.QuerySingleAsync<int>(query, commentModel);
            commentModel.Id = id;
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await GetByIdAsync(id);
            if (comment == null)
                return null;

            const string query = "DELETE FROM Comments WHERE Id = @id";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { id });
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            const string query = "SELECT * FROM Comments";
            using var connection = _context.CreateConnection();
            return (await connection.QueryAsync<Comment>(query)).ToList();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            const string query = "SELECT * FROM Comments WHERE Id = @id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Comment>(query, new { id });
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await GetByIdAsync(id);
            if (existingComment == null)
                return null;

            const string query = @"
                UPDATE Comments
                SET Title = @Title, Content = @Content
                WHERE Id = @id";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                id,
                commentModel.Title,
                commentModel.Content
            });

            return await GetByIdAsync(id);
        }
    }
}
