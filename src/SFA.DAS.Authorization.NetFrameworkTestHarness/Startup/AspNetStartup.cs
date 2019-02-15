using Microsoft.Owin;
using Owin;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Startup;

[assembly: OwinStartup(typeof(AspNetStartup))]
namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Startup
{
    public class AspNetStartup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}