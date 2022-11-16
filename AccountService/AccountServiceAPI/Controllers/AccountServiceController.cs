using AccountServiceModels.Interfaces;
using AccountServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountServiceController : ControllerBase
{
    private readonly ILogger<AccountServiceController> _logger;
    private readonly IAccountLogic _accountLogic;

    public AccountServiceController(ILogger<AccountServiceController> logger, IAccountLogic AccountLogic)
    {
        _logger = logger;
        _accountLogic = AccountLogic;
    }

    [HttpGet("GetAccounts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Account>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAccounts()
    {
        var accounts = _accountLogic.GetAccounts();
        return Ok(accounts);
    }

    [HttpGet("GetAccount/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAccount(string name)
    {
        var account = _accountLogic.GetAccount(name);
        if (account is null)
        {
            _logger.Log(LogLevel.Information, "Account with name {name} not found", name);
            return NotFound();
        }
        return Ok(account);
    }
    
    [HttpPost("PostAccount")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostAccount(Account Account)
    {
        var result = _accountLogic.AddAccount(Account);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Account with id {name} attempted to be created, but already exists", Account.Name);
            return BadRequest();
        }
        return Created($"GetAccount/{result.Name}", result);
    }

    [HttpPut("PutAccount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutAccount(Account Account)
    {
        var result = _accountLogic.UpdateAccount(Account);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Account with id {name} attempted to be updated, but does not exist", Account.Name);
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpDelete("DeleteAccount/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteAccount(string name)
    {
        var result = _accountLogic.DeleteAccount(name);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Account with name {name} attempted to be deleted, but does not exist", name);
            return NotFound();
        }
        return Ok();
    }
}