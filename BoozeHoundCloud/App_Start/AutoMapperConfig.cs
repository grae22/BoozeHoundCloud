using AutoMapper;
using BoozeHoundCloud.Dtos;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud
{
  public static class AutoMapperConfig
  {
    public static void Initialise()
    {
      Mapper.Initialize(cfg =>
      {
        //cfg.CreateMap<Account, AccountDto>();
        cfg.CreateMap<IAccount, AccountDto>();

        cfg.CreateMap<AccountDto, IAccount>()
          .ForMember(m => m.AccountType, opt => opt.Ignore());
      });

      Mapper.AssertConfigurationIsValid();
    }
  }
}