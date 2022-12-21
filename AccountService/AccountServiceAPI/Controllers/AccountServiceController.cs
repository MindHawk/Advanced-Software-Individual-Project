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

    public AccountServiceController(ILogger<AccountServiceController> logger, IAccountLogic accountLogic)
    {
        _logger = logger;
        _accountLogic = accountLogic;
    }

    [HttpGet("GetAccounts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Account>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAccounts()
    {
        var accounts = _accountLogic.GetAccounts();
        return Ok(accounts);
    }

    [HttpGet("GetAccount/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAccount(int id)
    {
        var account = _accountLogic.GetAccount(id);
        if (account is null)
        {
            _logger.Log(LogLevel.Information, "Account with id {id} not found", id);
            return NotFound();
        }
        return Ok(account);
    }
    
    [HttpPost("PostAccount")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostAccount(Account account)
    {
        var result = _accountLogic.AddAccount(account);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Account with name {name} attempted to be created, but already exists", account.Name);
            return BadRequest();
        }
        return Created($"GetAccount/{result.Name}", result);
    }

    [HttpPut("PutAccount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutAccount(Account account)
    {
        var result = _accountLogic.UpdateAccount(account);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Account with id {id} attempted to be updated, but does not exist", account.Id);
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpDelete("DeleteAccount/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteAccount(int id)
    {
        var result = _accountLogic.DeleteAccount(id);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Account with id {id} attempted to be deleted, but does not exist", id);
            return NotFound();
        }
        return Ok();
    }
}