using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeatherWebApplication.Startup))]
namespace WeatherWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
