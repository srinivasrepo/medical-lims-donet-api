using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MedicalLIMSApi.Web.Startup))]

namespace MedicalLIMSApi.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
