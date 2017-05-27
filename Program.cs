using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleHttpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Default options
            var contentRoot = Directory.GetCurrentDirectory();
            var port = 8000;

            // Parse arguments
            var remainingArgs = args.ToList();
            while (remainingArgs.Count > 0)
            {
                if (Directory.Exists(remainingArgs.First()))
                {
                    contentRoot = remainingArgs.First();
                } 
                else if (int.TryParse(remainingArgs.First(), out int customPort))
                {
                    port = customPort;
                }
                else
                {
                    break;
                }

                remainingArgs = remainingArgs.Skip(1).ToList();
            }

            var host = new WebHostBuilder()
                .UseUrls("http://*:" + port)
                .UseContentRoot(contentRoot)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }

    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment)
        {
            loggerFactory.AddConsole();
            
            var options = new FileServerOptions()
            {
                FileProvider = hostingEnvironment.ContentRootFileProvider,
                RequestPath = new PathString(string.Empty),
                EnableDirectoryBrowsing = true
            };

            options.StaticFileOptions.ServeUnknownFileTypes = true;

            app.UseFileServer(options);
        }
    }
}
