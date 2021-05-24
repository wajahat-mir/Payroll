using Payroll.Bll.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Factories.Employee
{
    public class EmployeeFactory: IEmployeeFactory
    {

        public IEmployee GetEmployee(string jobGroup)
        {
            switch (jobGroup)
            {
                case "A":
                    return new EmployeeA();
                case "B":
                    return new EmployeeB();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
