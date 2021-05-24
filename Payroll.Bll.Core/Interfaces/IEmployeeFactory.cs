using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Interfaces
{
    public interface IEmployeeFactory
    {
        IEmployee GetEmployee(string jobGroup);
    }
}
