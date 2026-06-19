using Finshark.Application.Model.Requests;
using Finshark.Application.Model.Responses;
using Finshark.Domain.Entities;
using Finshark.Domain.Interface.Base;
using Finshark.Infrastructure.Repositories;

namespace Finshark.Application.Services;

public class CommentService : IScopedService
{
    private readonly CommentRepository _commentRepository;
    private readonly StockRepository _stockRepository;

    public CommentService(CommentRepository commentRepository, StockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }

    public async Task<List<CommentResponse>> GetAllAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
        return comments.Select(MapToResponse).ToList();
    }

    public async Task<CommentResponse?> GetByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        return comment == null ? null : MapToResponse(comment);
    }

    public async Task<(CommentResponse? Comment, string? Error)> CreateAsync(int stockId, CreateCommentRequest request)
    {
        if (!await _stockRepository.StockExists(stockId))
            return (null, "stock Does not Exist");

        var comment = new Comment
        {
            Title = request.Title,
            Content = request.Content,
            StockId = stockId
        };

        var created = await _commentRepository.CreateAsync(comment);
        return (MapToResponse(created!), null);
    }

    public async Task<CommentResponse?> UpdateAsync(int id, UpdateCommentRequest request)
    {
        var comment = new Comment
        {
            Title = request.Title,
            Content = request.Content
        };

        var updated = await _commentRepository.UpdateAsync(id, comment);
        return updated == null ? null : MapToResponse(updated);
    }

    public async Task<CommentResponse?> DeleteAsync(int id)
    {
        var deleted = await _commentRepository.DeleteAsync(id);
        return deleted == null ? null : MapToResponse(deleted);
    }

    private static CommentResponse MapToResponse(Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            CreatedOn = comment.CreatedOn,
            StockId = comment.StockId
        };
    }
}
