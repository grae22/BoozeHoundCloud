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
        //cfg.CreateMap<NewAccountDto, Account>()
        //  .ForMember(m => m.)
      });

      Mapper.AssertConfigurationIsValid();
    }
  }
}