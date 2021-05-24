using Payroll.Bll.Core.Models.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Bll.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<ReportModel> GetReportByIdAsync(int reportId);
        Task CreateReportAsync(int reportId);
    }
}
