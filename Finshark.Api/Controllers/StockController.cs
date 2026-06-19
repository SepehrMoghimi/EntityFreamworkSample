using Finshark.Application.Model.Requests;
using Finshark.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finshark.Api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly StockService _stockService;

    public StockController(StockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] StockQueryRequest query)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stocks = await _stockService.GetAllAsync(query);
        return Ok(stocks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockService.GetByIdAsync(id);
        if (stock == null)
            return NotFound();

        return Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> create([FromBody] CreateStockRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = stock.ID }, stock);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockService.UpdateAsync(id, request);
        if (stock == null)
            return NotFound();

        return Ok(stock);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockService.DeleteAsync(id);
        if (stock == null)
            return NotFound();

        return NoContent();
    }
}
