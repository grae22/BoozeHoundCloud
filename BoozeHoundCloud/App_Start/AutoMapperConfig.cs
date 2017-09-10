using AutoMapper;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud
{
  public static class AutoMapperConfig
  {
    public static void Initialise()
    {
      Mapper.Initialize(cfg =>
      {
        cfg.CreateMap<Account, AccountDto>();

        cfg.CreateMap<AccountDto, Account>()
          .ForMember(m => m.AccountType, opt => opt.Ignore());
      });

      Mapper.AssertConfigurationIsValid();
    }
  }
}