using Finshark.Application.Model.Requests;
using Finshark.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finshark.Api.Controllers;

[Route("api/Comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comments = await _commentService.GetAllAsync();
        return Ok(comments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentService.GetByIdAsync(id);
        if (comment == null)
            return NotFound();

        return Ok(comment);
    }

    [HttpPost("{StockId:int}")]
    public async Task<IActionResult> create([FromRoute] int StockId, CreateCommentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (comment, error) = await _commentService.CreateAsync(StockId, request);
        if (error != null)
            return BadRequest(error);

        return CreatedAtAction(nameof(GetById), new { id = comment!.Id }, comment);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentService.UpdateAsync(id, request);
        if (comment == null)
            return NotFound("Comment Not Found");

        return Ok(comment);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentService.DeleteAsync(id);
        if (comment == null)
            return NotFound("Comment Does not exist");

        return Ok(comment);
    }
}
