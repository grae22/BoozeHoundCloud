using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BoozeHoundCloud
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AutoMapperConfig.Initialise();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteTable.Routes.MapMvcAttributeRoutes();
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
  }
}
