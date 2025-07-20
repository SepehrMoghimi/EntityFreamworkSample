using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title Must be 5 characters")]
        [MaxLength(280, ErrorMessage ="title can not be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MinLength(5, ErrorMessage = "Content Must be 5 characters")]
        [MaxLength(280, ErrorMessage ="Content can not be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}