using Microsoft.Extensions.DependencyInjection;
using MVCProject.BLL.Intarfaces;
using MVCProject.BLL.Repositories;
using MVCProject.PL.MappingProfiles;
using MVCProject.PL.Services.EmailSender;
using System.Runtime.CompilerServices;

namespace MVCProject.PL.Extensions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services )
        {
            //services.AddScoped <IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IEmailSender,EmailSender>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            return services;
        }
    }
}
