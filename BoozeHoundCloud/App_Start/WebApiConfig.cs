using System.Web.Http;

namespace BoozeHoundCloud
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/Core/{controller}/{id}",
        defaults: new {id = RouteParameter.Optional}
      );
    }
  }
}
