using AutoMapper;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud
{
  public static class AutoMapperConfig
  {
    public static void Initialise()
    {
      Mapper.Initialize(cfg =>
      {
        // Account.
        cfg.CreateMap<Account, AccountDto>();

        cfg.CreateMap<AccountDto, Account>()
          .ForMember(m => m.AccountType, opt => opt.Ignore());

        // Transaction.
        cfg.CreateMap<Transaction, TransactionDto>();

        cfg.CreateMap<TransactionDto, Transaction>()
          .ForMember(m => m.DebitAccount, opt => opt.Ignore())
          .ForMember(m => m.CreditAccount, opt => opt.Ignore());
      });

      Mapper.AssertConfigurationIsValid();
    }
  }
}