using Moq;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Core.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.UnitTests.RepositoryMocks
{
    public static class PayrollRepositoryMock
    {
        public static Mock<IPayrollRepository> GetBaseFactoryAsync()
        {
            var mock = new Mock<IPayrollRepository>();
            return mock;
        }

        public static Mock<IPayrollRepository> GetNoPaymentsMock()
        {
            List<PayModel> payments = new List<PayModel>();
            var mock = new Mock<IPayrollRepository>();
            mock.Setup(m => m.GetAllPaymentsAsync()).ReturnsAsync(payments);
            return mock;
        }
    }
}
