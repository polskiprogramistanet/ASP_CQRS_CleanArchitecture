using ASP_CQRS.Application;
using ASP_CQRS.Persistence.FF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_SQRS.API
{
    public class Startup
    {
      
        IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddASPCQRSApplication();
            services.Add_ASP_CQRS_EFServices(Configuration);
            services.AddControllers();
            services.AddCors(options =>
                { options.AddPolicy("Open",
                     builder => builder.AllowAnyOrigin()
                     .AllowAnyHeader().AllowAnyMethod());
                });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("Open");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
