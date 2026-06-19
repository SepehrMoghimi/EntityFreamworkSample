using Finshark.Domain.Entities;
using Finshark.Domain.Interface.Base;
using Microsoft.AspNetCore.Identity;

namespace Finshark.Infrastructure.Services;

public class IdentityAccountService : IScopedService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public IdentityAccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public Task<AppUser?> FindByNameAsync(string userName) =>
        _userManager.FindByNameAsync(userName);

    public Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password) =>
        _signInManager.CheckPasswordSignInAsync(user, password, false);

    public Task<IdentityResult> CreateAsync(AppUser user, string password) =>
        _userManager.CreateAsync(user, password);

    public Task<IdentityResult> AddToRoleAsync(AppUser user, string role) =>
        _userManager.AddToRoleAsync(user, role);
}
