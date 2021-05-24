using Moq;
using Payroll.Bll.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.UnitTests.RepositoryMocks
{
    public static class EmployeeRepositoryMock
    {
        public static Mock<IEmployeeRepository> GetBaseRepositoryAsync()
        {
            var mock = new Mock<IEmployeeRepository>();
            return mock;
        }
    }
}
