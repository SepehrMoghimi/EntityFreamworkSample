namespace Finshark.Application.Model.Responses;

public class RegisterResponse
{
    public NewUserResponse? User { get; set; }
    public object? Errors { get; set; }
    public bool Succeeded { get; set; }
}
