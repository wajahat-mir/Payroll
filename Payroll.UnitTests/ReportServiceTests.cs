using Microsoft.AspNetCore.Http;
using Moq;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Services;
using Payroll.UnitTests.FactoryMocks;
using Payroll.UnitTests.RepositoryMocks;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Payroll.UnitTests
{
    public class ReportServiceTests
    {
        private IEmployeeRepository baseEmployeeRepository = EmployeeRepositoryMock.GetBaseRepositoryAsync().Object;
        private IReportRepository baseReportRepository = ReportRepositoryMock.GetBaseRepositoryAsync().Object;
        private IEmployeeFactory employeeFactory = EmployeeFactoryMock.GetBaseFactoryAsync().Object;
        private IPayrollRepository basePayrollRepository = PayrollRepositoryMock.GetBaseFactoryAsync().Object;

        [Fact]
        public async Task GetPayrollReportAsync_NoPayments_ReturnNull()
        {
            var payrollRepository = PayrollRepositoryMock.GetNoPaymentsMock().Object;

            var reportService = new ReportService(baseReportRepository, baseEmployeeRepository, payrollRepository, employeeFactory);

            var result = await reportService.GetPayrollReportAsync();

            Assert.Empty(result.payrollReport.employeeReports);
        }

        [Fact]
        public async Task ParseAndValidatePayrollReportAsync_ReportExists_ReturnsException()
        {
            var reportRepository = ReportRepositoryMock.ReportAlreadyExists().Object;

            var reportService = new ReportService(reportRepository, baseEmployeeRepository, basePayrollRepository, employeeFactory);

            await Assert.ThrowsAsync<Exception>(() => reportService.ParseAndValidatePayrollReportAsync(It.IsAny<IFormFile>(), It.IsAny<int>()));
        }
    }
}
