using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCCustomAuthorizeAttribute.Startup))]
namespace MVCCustomAuthorizeAttribute
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
