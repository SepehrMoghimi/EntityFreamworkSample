using Finshark.Application.Model.Requests;
using Finshark.Application.Model.Responses;
using Finshark.Domain.Entities;
using Finshark.Domain.Interface.Base;
using Finshark.Infrastructure.Services;

namespace Finshark.Application.Services;

public class AccountService : IScopedService
{
    private readonly IdentityAccountService _identityAccountService;
    private readonly TokenService _tokenService;

    public AccountService(IdentityAccountService identityAccountService, TokenService tokenService)
    {
        _identityAccountService = identityAccountService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _identityAccountService.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return new LoginResponse { ErrorMessage = "Invalid username!" };
        }

        var result = await _identityAccountService.CheckPasswordSignInAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new LoginResponse { ErrorMessage = "username Not found / password incorrect!" };
        }

        return new LoginResponse
        {
            User = MapToNewUserResponse(user)
        };
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var appUser = new AppUser
        {
            UserName = request.Username,
            Email = request.Emial
        };

        var createdUser = await _identityAccountService.CreateAsync(appUser, request.Password!);
        if (!createdUser.Succeeded)
        {
            return new RegisterResponse
            {
                Succeeded = false,
                Errors = createdUser.Errors
            };
        }

        var roleResult = await _identityAccountService.AddToRoleAsync(appUser, "User");
        if (!roleResult.Succeeded)
        {
            return new RegisterResponse
            {
                Succeeded = false,
                Errors = roleResult.Errors
            };
        }

        return new RegisterResponse
        {
            Succeeded = true,
            User = MapToNewUserResponse(appUser)
        };
    }

    private NewUserResponse MapToNewUserResponse(AppUser user)
    {
        return new NewUserResponse
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }
}
