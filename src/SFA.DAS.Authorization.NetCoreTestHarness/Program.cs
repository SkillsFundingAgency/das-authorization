using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SFA.DAS.Authorization.NetCoreTestHarness.Startup;
using StructureMap.AspNetCore;

namespace SFA.DAS.Authorization.NetCoreTestHarness
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseNLog()
                .UseStructureMap()
                .UseStartup<AspNetStartup>();
        }
    }
}