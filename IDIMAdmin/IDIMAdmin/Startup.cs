using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IDIMAdmin.Startup))]
namespace IDIMAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
