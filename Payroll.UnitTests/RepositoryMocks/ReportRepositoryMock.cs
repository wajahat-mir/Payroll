using Moq;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Core.Models.Report;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.UnitTests.RepositoryMocks
{
    public static class ReportRepositoryMock
    {
        public static Mock<IReportRepository> GetBaseRepositoryAsync()
        {
            var mock = new Mock<IReportRepository>();
            return mock;
        }

        public static Mock<IReportRepository> ReportAlreadyExists()
        {
            var mock = new Mock<IReportRepository>();
            mock.Setup(m => m.GetReportByIdAsync(It.IsAny<int>())).ReturnsAsync(new ReportModel());
            return mock;
        }
    }
}
