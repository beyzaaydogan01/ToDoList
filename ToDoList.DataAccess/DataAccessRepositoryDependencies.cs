using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DataAccess.Abstracts;
using ToDoList.DataAccess.Concretes;
using ToDoList.DataAccess.Contexts;

namespace ToDoList.DataAccess;

public static class DataAccessRepositoryDependencies
{
    public static IServiceCollection AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IToDoRepository, EfToDoRepository>();

        services.AddDbContext<BaseDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        return services;
    }
}