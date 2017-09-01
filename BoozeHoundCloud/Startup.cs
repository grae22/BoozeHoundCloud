using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoozeHoundCloud.Startup))]
namespace BoozeHoundCloud
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
