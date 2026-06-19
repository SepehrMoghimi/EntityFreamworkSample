using System.ComponentModel.DataAnnotations;

namespace Finshark.Application.Model.Requests;

public class RegisterRequest
{
    [Required]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string? Emial { get; set; }

    [Required]
    public string? Password { get; set; }
}
