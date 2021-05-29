using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Factories.Employee;
using Payroll.Bll.Services;
using Payroll.Dal.Adaptors;
using Payroll.Dal.Interfaces;
using Payroll.Dal.Repositories;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Payroll.API.Services
{
    public static class ServiceCollectionExtensions
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(300);
        private static readonly IAsyncPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                }
            );

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

        public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ILocationApiClient, LocationApiClient>(c =>
            {
                c.BaseAddress = new Uri(configuration["Location:ApiUrl"]);
                c.DefaultRequestHeaders.Add("api-key", configuration["Location:ApiKey"]);
                c.Timeout = _timeout;
            }).AddPolicyHandler(RetryPolicy);

            return services;
        }
    }
}
