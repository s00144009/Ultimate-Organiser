using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ultimateorganiser.Startup))]
namespace ultimateorganiser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
