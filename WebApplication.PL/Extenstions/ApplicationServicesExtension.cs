using Microsoft.Extensions.DependencyInjection;
using WebApplication.BLL;
using WebApplication.BLL.Interfaces;
using WebApplication.BLL.Repositories;
using WebApplication.PL.Helpers;

namespace WebApplication.PL.Extenstions
{
    public static class ApplicationServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepo>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepo>();
            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
        }
    }
}
