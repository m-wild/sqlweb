using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlWeb.Audit;
using SqlWeb.Database;
using SqlWeb.Persistence;

namespace SqlWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddSpaStaticFiles(staticFiles => staticFiles.RootPath = "wwwroot");

            services.AddOptions();

            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<ISessionStore, InMemorySessionStore>();
            services.AddScoped<ISessionFactory, SessionFactory>();
            services.AddScoped<IResourceStoreFactory, ResourceStoreFactory>();
            services.AddScoped<IAuditLogFactory, AuditLogFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpaStaticFiles();
            app.UseSpa(spa => 
            {
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:1234/");
                }
            });
            
        }
    }
}
