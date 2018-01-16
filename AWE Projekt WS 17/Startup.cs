using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AWE_Projekt_WS_17.Startup))]
namespace AWE_Projekt_WS_17
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
