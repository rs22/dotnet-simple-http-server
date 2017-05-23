using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace SimpleHttpServer
{
    internal class Startup
    {
        internal static string Dir;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            
            var options = new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Dir),
                RequestPath = new PathString(""),
                EnableDirectoryBrowsing = true
            };
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            app.UseFileServer(options);
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.Dir = args[0];

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
