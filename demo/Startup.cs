using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;

namespace DemoApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddDashboard();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            app.UseOrleansDashboardOffline();

        public static void ConfigureOrleans(ISiloBuilder siloBuilder) =>
            siloBuilder.UseLocalhostClustering();
    }
}
