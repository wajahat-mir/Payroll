using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Models.Payroll
{
    public class EmployeeReportModel
    {
        public int employeeId { get; set; }
        public PayPeriodModel payPeriod { get; set; }
        public string amountPaid { get; set; }
    }
}
