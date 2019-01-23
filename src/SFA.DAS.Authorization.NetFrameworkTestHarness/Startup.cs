using Microsoft.Owin;
using Owin;
using SFA.DAS.Authorization.NetFrameworkTestHarness;

[assembly: OwinStartup(typeof(Startup))]
namespace SFA.DAS.Authorization.NetFrameworkTestHarness
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}