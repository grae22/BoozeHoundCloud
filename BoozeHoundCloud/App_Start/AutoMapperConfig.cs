using AutoMapper;

namespace BoozeHoundCloud
{
  public static class AutoMapperConfig
  {
    public static void Initialise()
    {
      Mapper.Initialize(cfg =>
      {
      });

      Mapper.AssertConfigurationIsValid();
    }
  }
}