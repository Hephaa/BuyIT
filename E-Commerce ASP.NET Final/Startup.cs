using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Commerce_ASP.NET_Final.Startup))]
namespace E_Commerce_ASP.NET_Final
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
