using Domain.Aggregates.Customer.Repositories;
using Domain.Aggregates.Customer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Options;
using Persistence.Repositories;
using Persistence.Services;
using Shared.Options;

namespace Persistence
{
    public static class Extensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetOptions<DataBaseOptions>("DataBaseConnectionString");

            services.AddDbContext<ApplicationDbContext>(ctx => ctx.UseSqlServer(options.ConnectionString));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerUniquenessCheckerService, CustomerUniquenessCheckerService>();

            return services;
        }
    }
}
