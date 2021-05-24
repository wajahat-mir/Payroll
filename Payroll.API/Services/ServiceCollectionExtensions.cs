using Microsoft.Extensions.DependencyInjection;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Factories.Employee;
using Payroll.Bll.Services;
using Payroll.Dal.Adaptors;
using Payroll.Dal.Interfaces;
using Payroll.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.API.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IEmployeeFactory, EmployeeFactory>();

            return services;
        }
    }
}
