using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationService(this IServiceCollection services)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });
        services.AddFluentValidationAutoValidation();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
    }
}