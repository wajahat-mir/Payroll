using Moq;
using Payroll.Bll.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.UnitTests.FactoryMocks
{
    public static class EmployeeFactoryMock
    {
        public static Mock<IEmployeeFactory> GetBaseFactoryAsync()
        {
            var mock = new Mock<IEmployeeFactory>();
            return mock;
        }
    }
}
