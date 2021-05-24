using Payroll.Bll.Core.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Bll.Core.Interfaces
{
    public interface IPayrollRepository
    {
        Task InsertPayrollAsync(PayrollModel payroll, int reportId);
        Task<IEnumerable<PayModel>> GetAllPaymentsAsync();
        Task InsertPayrollsAsync(IEnumerable<PayrollModel> payrolls, int reportId);
    }
}
