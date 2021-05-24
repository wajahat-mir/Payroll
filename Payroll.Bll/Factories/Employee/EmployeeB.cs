using Payroll.Bll.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Factories.Employee
{
    public class EmployeeB: IEmployee
    {
        public int GetHourlyRate()
        {
            return 30;
        }
    }
}
