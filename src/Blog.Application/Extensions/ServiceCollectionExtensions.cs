using Blog.Application.UserPost.Command.AddUserPost;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationService(this IServiceCollection services)
    {
       services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
        services.AddFluentValidation(x=>x.RegisterValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}