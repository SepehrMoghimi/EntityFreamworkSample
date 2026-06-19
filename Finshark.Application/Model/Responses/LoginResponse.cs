namespace Finshark.Application.Model.Responses;

public class LoginResponse
{
    public NewUserResponse? User { get; set; }
    public string? ErrorMessage { get; set; }
}
