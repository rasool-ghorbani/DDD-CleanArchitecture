using Application.Behaviors;
using Application.Features.Customers.Commands.CreateCustomer;
using Application.Mappings;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });


            services.AddAutoMapper(cfg =>
            {
                //cfg.AddProfile<CustomerProfile>();
                cfg.AddMaps(typeof(CustomerProfile).Assembly);
               // cfg.AddProfile(typeof(CustomerProfile));
            });


            return services;
        }
    }
}
