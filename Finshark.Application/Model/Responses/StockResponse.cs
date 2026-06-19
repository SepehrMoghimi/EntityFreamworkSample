namespace Finshark.Application.Model.Responses;

public class StockResponse
{
    public int ID { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Purchase { get; set; }
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }
    public List<CommentResponse>? Comments { get; set; }
}
