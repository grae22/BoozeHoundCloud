using System;
using System.Web.Http;
using AutoMapper;
using BoozeHoundCloud.DAL;
using BoozeHoundCloud.Dtos;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Controllers.Api
{
  public class AccountController : ApiController
  {
    //-------------------------------------------------------------------------

    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<AccountType> _accountTypeRepository;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      var applicationDbContext = new ApplicationDbContext();
      _accountRepository = new AccountRepository(applicationDbContext);
      _accountTypeRepository = new AccountTypeRepository(applicationDbContext);
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetAccount(int id)
    {
      // Account already exists with name?
      Account account = _accountRepository.Get(id);

      if (account == null)
      {
        return NotFound();
      }

      var accountDto = Mapper.Map<Account, AccountDto>(account);

      return Json(accountDto);
    }

    //-------------------------------------------------------------------------

    [HttpPost]
    public IHttpActionResult CreateAccount(AccountDto accountDto)
    {
      // Account already exists with name?
      Account existingAccount =
        _accountRepository.Get(
          a => a.Name.Equals(accountDto.Name, StringComparison.OrdinalIgnoreCase));

      if (existingAccount != null)
      {
        return BadRequest($"Account already exists with name '{accountDto.Name}'.");
      }

      // Get the account type.
      AccountType accountType = _accountTypeRepository.Get(a => accountDto.AccountTypeId == a.Id);

      if (accountType == null)
      {
        return BadRequest($"AccountType not found for id {accountDto.AccountTypeId}.");
      }

      // Create new account object.
      var newAccount = Mapper.Map<AccountDto, Account>(accountDto);

      _accountRepository.Add(newAccount);
      _accountRepository.Save();

      return Created(
        new Uri(
          $"{Request.RequestUri}/{newAccount.Id}"),
          newAccount);
    }

    //-------------------------------------------------------------------------
  }
}
