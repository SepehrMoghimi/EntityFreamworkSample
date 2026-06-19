using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finshark.Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public int? StockId { get; set; }

    public Stock? Stock { get; set; }
}
