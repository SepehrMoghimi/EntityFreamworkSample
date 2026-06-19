using Finshark.Application.Model.Requests;
using Finshark.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finshark.Api.Controllers;

[Route("api/Account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _accountService.LoginAsync(request);
        if (response.ErrorMessage != null)
            return Unauthorized(response.ErrorMessage);

        return Ok(response.User);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.RegisterAsync(request);
            if (!response.Succeeded)
                return StatusCode(500, response.Errors);

            return Ok(response.User);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
