using AccountServiceModels.Interfaces;
using AccountServiceModels;
using Microsoft.AspNetCore.Mvc;
using AccountServiceAPI.Attributes;
using Google.Apis.Auth;

namespace AccountServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountServiceController : ControllerBase
{
    private readonly ILogger<AccountServiceController> _logger;
    private readonly IAccountLogic _accountLogic;
    private readonly IConfiguration _configuration;
    
    public AccountServiceController(ILogger<AccountServiceController> logger, IAccountLogic accountLogic, IConfiguration configuration)
    {
        _logger = logger;
        _accountLogic = accountLogic;
        _configuration = configuration;
    }
    
    [HttpGet("GetAccounts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Account>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAccounts()
    {
        var accounts = _accountLogic.GetAccounts();
        return Ok(accounts);
    }

    [HttpGet("GetAccount/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAccount(int id)
    {
        var account = _accountLogic.GetAccount(id);
        if (account is null)
        {
            _logger.Log(LogLevel.Information, "Account with id {Id} not found", id);
            return NotFound();
        }
        return Ok(account);
    }
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpPost("PostAccount")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostAccount()
    {
        (int _, string googleId, string username) = GetTokenInfo();
        if(googleId == "" || username == "")
        {
            _logger.Log(LogLevel.Error, "Google id or username not found in token");
            return BadRequest();
        }
        Account account = new() { GoogleId = googleId, Name = username };
        Account? result = _accountLogic.AddAccount(account);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Account with name {Name} attempted to be created, but already exists", account.Name);
            return BadRequest();
        }
        return Created($"GetAccount/{result.Name}", result);
    }

    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpDelete("DeleteAccount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteAccount()
    {
        (int id, string googleId, string username) = GetTokenInfo();
        if(id == -1)
        {
            _logger.Log(LogLevel.Error, "User id not found in token");
            return BadRequest();
        }
        bool result = _accountLogic.DeleteAccount(id);
        if (result is false)
        {
            _logger.LogInformation("Account with id {Id} attempted to be deleted, but does not exist", id);
            return NotFound();
        }
        return Ok();
    }

    private (int, string, string) GetTokenInfo()
    {
        string googleId = HttpContext.Items["GoogleId"] as string ?? "";
        string username = HttpContext.Items["Username"] as string ?? "";
        _logger.LogInformation("GoogleId: {GoogleId}, Username: {Username}", googleId, username);
        int id = _accountLogic.GetAccountIdFromGoogleId(googleId);
        return (id, googleId, username);
    }
}