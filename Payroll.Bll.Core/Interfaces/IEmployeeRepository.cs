using Payroll.Bll.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Bll.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeesAsync(IEnumerable<EmployeeModel> employees);
    }
}
