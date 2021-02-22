using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using OrleansDashboard;

// ReSharper disable CheckNamespace

namespace Orleans
{
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder UseOrleansDashboardOffline(this IApplicationBuilder app,
            DashboardOptions options = null)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new EmbeddedFileProvider(
                    typeof(OfflineDashboardMiddleware).Assembly,
                    "OrleansDashboard.Offline"
                )
            });
            app.UseMiddleware<OfflineDashboardMiddleware>();
            return app.UseOrleansDashboard(options);
        }
    }
}
