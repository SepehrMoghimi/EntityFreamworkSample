using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;

namespace api.models
{
    public class Stock
    {
        public int ID { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Column(TypeName = "Decimal(18,2)")]

        public decimal Purchase { get; set; }
        [Column(TypeName = "Decimal(18,2)")]

        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}