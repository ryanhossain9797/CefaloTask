using Microsoft.Extensions.DependencyInjection;
using Cefalo.Csharp.Core.Services;
using Cefalo.Csharp.Core.Repositories;
using Cefalo.Csharp.Application.Services;
using Cefalo.Csharp.Infrastructure.Repositories;

namespace Cefalo.Csharp.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}