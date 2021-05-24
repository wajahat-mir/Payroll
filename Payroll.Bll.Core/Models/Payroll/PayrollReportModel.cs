using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Models.Payroll
{
    public class PayrollReportModel
    {
        public PayrollReport payrollReport { get; set; }

        public PayrollReportModel()
        {
            payrollReport = new PayrollReport();
        }
    }

    public class PayrollReport
    {
        public List<EmployeeReportModel> employeeReports { get; set; }

        public PayrollReport()
        {
            employeeReports = new List<EmployeeReportModel>();
        }
    }
}
