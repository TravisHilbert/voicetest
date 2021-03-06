using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.DeviceBridge.Web.Hubs;
using Microsoft.CognitiveServices.DeviceBridge.Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.CognitiveServices.DeviceBridge.Web
{
    public class Startup
    { 
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.Configuration = configuration;           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR(o => o.EnableDetailedErrors = true).AddAzureSignalR();

            services.AddSingleton<IInventoryManager, InventoryManager>();
            services.AddSingleton<IInventory, BasicInventory>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseFileServer();
            app.UseMvc();
            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<InventoryLogHub>("/inventoryLogHub");
            });
        }
    }
}
