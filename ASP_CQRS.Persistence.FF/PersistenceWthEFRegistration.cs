using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Persistence.FF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
namespace ASP_CQRS.Persistence.FF
{
    public static class PersistenceWthEFRegistration
    {
        public static IServiceCollection Add_ASP_CQRS_EFServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ASP_CQRSContext>(options =>
                options.UseSqlServer(configuration.
                GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IWebinaryRepository, WebinarRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            return services;
        }
    }
}

