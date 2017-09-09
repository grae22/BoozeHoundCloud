﻿using System;
using System.Linq;
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

    private readonly IApplicationDbContext _context;
    private readonly IRepository<Account> _accounts;
    private readonly IRepository<AccountType> _accountTypes;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      _context = new ApplicationDbContext();
      _accounts = new AccountRepository(_context);
      _accountTypes = new AccountTypeRepository(_context);
    }

    //-------------------------------------------------------------------------

    public AccountController(IApplicationDbContext context,
                             IRepository<Account> accounts,
                             IRepository<AccountType> accountTypes)
    {
      _context = context;
      _accounts = accounts;
      _accountTypes = accountTypes;
    }

    //-------------------------------------------------------------------------
    
    [HttpGet]
    public IHttpActionResult GetAccount(int id)
    {
      // Account already exists with name?
      IAccount account = _accounts.Get(id);

      if (account == null)
      {
        return NotFound();
      }

      var accountDto = Mapper.Map<IAccount, AccountDto>(account);

      return Ok(accountDto);
    }

    //-------------------------------------------------------------------------

    [HttpPost]
    public IHttpActionResult CreateAccount(AccountDto accountDto)
    {
      // Account already exists with name?
      IAccount existingAccount = _accounts.Get(
        a => a.Name.Equals(accountDto.Name, StringComparison.OrdinalIgnoreCase));

      if (existingAccount != null)
      {
        return BadRequest($"Account already exists with name '{accountDto.Name}'.");
      }

      // Get the account type.
      IAccountType accountType = _accountTypes.Get(accountDto.AccountTypeId);

      if (accountType == null)
      {
        return BadRequest($"AccountType not found for id {accountDto.AccountTypeId}.");
      }

      // Create new account object.
      var newAccount = Mapper.Map<AccountDto, Account>(accountDto);

      _accounts.Add(newAccount);
      _accounts.Save();

      return Created(
        new Uri(
          $"{Request.RequestUri}/{newAccount.Id}"),
          newAccount);
    }

    //-------------------------------------------------------------------------
  }
}
