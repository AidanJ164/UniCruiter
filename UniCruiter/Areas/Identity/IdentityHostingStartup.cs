using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(UniCruiter.Areas.Identity.IdentityHostingStartup))]
namespace UniCruiter.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}