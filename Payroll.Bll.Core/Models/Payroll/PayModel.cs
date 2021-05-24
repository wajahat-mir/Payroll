using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Models.Payroll
{
    public class PayModel
    {
        public int employeeId { get; set; }
        public string JobGroup { get; set; }
        public DateTime date { get; set; }
        public int hoursWorked { get; set; }
    }
}
