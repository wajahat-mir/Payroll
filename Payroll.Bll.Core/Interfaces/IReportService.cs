using Microsoft.AspNetCore.Http;
using Payroll.Bll.Core.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Bll.Core.Interfaces
{
    public interface IReportService
    {
        Task CreateReportAsync(int reportId);
        Task<PayrollReportModel> GetPayrollReportAsync();
        Task<IEnumerable<PayrollModel>> ParseAndValidatePayrollReportAsync(IFormFile payrollFile, int reportId);
        Task InsertPayrollsAsync(IEnumerable<PayrollModel> payrolls, int reportId);
    }
}
