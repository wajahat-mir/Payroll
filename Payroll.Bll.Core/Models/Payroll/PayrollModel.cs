using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Models.Payroll
{
    public class PayrollModel
    {
        public DateTime date { get; set; }
        public double hoursWorked { get; set; }
        public int employeeId { get; set; }
        public string jobGroup { get; set; }
    }
}
