using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Service.Abstracts;
using ToDoList.Service.Concretes;
using ToDoList.Service.Profiles;
using ToDoList.Service.Rules;

namespace ToDoList.Service;
public static class ServiceDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddScoped<ToDoBusinessRules>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IToDoService, ToDoService>();
        services.AddScoped<UserBusinessRules>();
        services.AddScoped<ToDoBusinessRules>();
        services.AddScoped<CategoryBusinessRules>();
        return services;
    }
}