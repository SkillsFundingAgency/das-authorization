using Microsoft.Owin;
using Owin;
using SFA.DAS.Authorization.TestHarness;

[assembly: OwinStartup(typeof(Startup))]
namespace SFA.DAS.Authorization.TestHarness
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
