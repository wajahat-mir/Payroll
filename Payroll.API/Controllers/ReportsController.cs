using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.Bll.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IReportService _reportService;

        public ReportsController(ILogger<ReportsController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var report = await _reportService.GetPayrollReportAsync();

            if (report.payrollReport.employeeReports.Count() == 0)
                return NoContent();
            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(IFormFile payrollFile)
        {
            var reportId = Convert.ToInt32((payrollFile.FileName.Split("-")[2]).Split(".")[0]);

            var payrolls = await _reportService.ParseAndValidatePayrollReportAsync(payrollFile, reportId);

            if (payrolls == null || payrolls.Count() == 0)
                return NoContent();

            await _reportService.CreateReportAsync(reportId);
            await _reportService.InsertPayrollsAsync(payrolls, reportId);
            return Ok();
        }
    }
}
