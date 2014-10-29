using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LjusOchMiljoAB.Startup))]
namespace LjusOchMiljoAB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
