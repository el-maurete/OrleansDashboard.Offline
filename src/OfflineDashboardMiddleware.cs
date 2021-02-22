using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OrleansDashboard;

// ReSharper disable once CheckNamespace
namespace Orleans
{
    public sealed class OfflineDashboardMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IOptions<DashboardOptions> options;

        public OfflineDashboardMiddleware(
            RequestDelegate next,
            IOptions<DashboardOptions> options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/" || string.IsNullOrEmpty(context.Request.Path))
            {
                await WriteIndexFile(context);
                return;
            }
            await next(context);
        }
        
        private async Task WriteIndexFile(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            await using var stream = typeof(OfflineDashboardMiddleware).GetTypeInfo().Assembly
                .GetManifestResourceStream($"OrleansDashboard.Offline.Index.html");
            var content = await new StreamReader(stream!).ReadToEndAsync();
            var basePath = string.IsNullOrWhiteSpace(options.Value.ScriptPath)
                ? context.Request.PathBase.ToString()
                : options.Value.ScriptPath;

            if (basePath != "/")
            {
                basePath += "/";
            }

            content = content.Replace("{{BASE}}", basePath);
            content = content.Replace("{{HIDE_TRACE}}", options.Value.HideTrace.ToString().ToLowerInvariant());

            await context.Response.WriteAsync(content);
        }
    }
}
