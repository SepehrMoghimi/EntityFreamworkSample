using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finshark.Domain.Entities;

public class Stock
{
    public int ID { get; set; }

    [MaxLength(10)]
    public string Symbol { get; set; } = string.Empty;

    [MaxLength(255)]
    public string CompanyName { get; set; } = string.Empty;

    [Column(TypeName = "Decimal(18,2)")]
    public decimal Purchase { get; set; }

    [Column(TypeName = "Decimal(18,2)")]
    public decimal LastDiv { get; set; }

    [MaxLength(100)]
    public string Industry { get; set; } = string.Empty;

    public long MarketCap { get; set; }

    public List<Comment> Comments { get; set; } = new();
}
