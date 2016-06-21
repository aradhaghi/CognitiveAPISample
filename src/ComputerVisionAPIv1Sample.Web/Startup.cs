using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ComputerVisionAPIv1.Infrastructure;
using ComputerVisionAPIv1.Infrastructure.Contracts;

namespace ComputerVisionAPIv1Sample.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IImageRepository, ImageRepository>(s =>
            {
                string databaseId = Configuration["DocumentDB:Database"];
                string collectionId = Configuration["DocumentDB:Collection"];
                string endpoint = Configuration["DocumentDB:Endpoint"];
                string authKey = Configuration["DocumentDB:AuthKey"];

                return new ImageRepository(endpoint, authKey, databaseId, collectionId);
            });

            services.AddScoped<ICognitiveService, CognitiveService>(s =>
           {
               string url = Configuration["CognitiveService:ComputerVision:Url"];
               string subscriptionKey = Configuration["CognitiveService:ComputerVision:SubscriptionKey"];
               string contentType = Configuration["CognitiveService:ComputerVision:ContentType"];

               return new CognitiveService(url, subscriptionKey, contentType);
           });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Cognitive}/{action=ProcessImage}/{id?}");
            });
        }
    }
}
