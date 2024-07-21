using Microsoft.Extensions.DependencyInjection;
using MVCProject.BLL.Intarfaces;
using MVCProject.BLL.Repositories;
using System.Runtime.CompilerServices;

namespace MVCProject.PL.Extensions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services )
        {
            //services.AddScoped <IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
