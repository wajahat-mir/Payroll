using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Core.Models.Employee;
using Payroll.Bll.Core.Models.Payroll;
using Payroll.Bll.Core.Models.Report;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Bll.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeFactory _employeeFactory;

        public ReportService(IReportRepository reportRepository, IEmployeeRepository employeeRepository, 
            IPayrollRepository payrollRepository, IEmployeeFactory employeeFactory)
        {
            _reportRepository = reportRepository;
            _employeeRepository = employeeRepository;
            _payrollRepository = payrollRepository;
            _employeeFactory = employeeFactory;
        }

        public async Task<PayrollReportModel> GetPayrollReportAsync()
        {
            var payrollReport = new PayrollReportModel();
            var pays = await _payrollRepository.GetAllPaymentsAsync();

            Dictionary<EmployeeReportKeyModel, double> amountByEmployeePerPeriod = new Dictionary<EmployeeReportKeyModel, double>();
            foreach (var pay in pays)
            {
                var employee = _employeeFactory.GetEmployee(pay.JobGroup);

                var key = new EmployeeReportKeyModel()
                {
                    EmployeeId = pay.employeeId
                };
                if(pay.date.Day <= 15)
                {
                    key.startDate = pay.date.AddDays(-pay.date.Day + 1).ToString("yyyy-MM-dd");
                    key.endDate = pay.date.AddDays(15 - pay.date.Day).ToString("yyyy-MM-dd");
                }
                else
                {
                    key.startDate = pay.date.AddDays(15 - pay.date.Day + 1).ToString("yyyy-MM-dd");
                    key.endDate = pay.date.AddDays(DateTime.DaysInMonth(pay.date.Year, pay.date.Month) - pay.date.Day).ToString("yyyy-MM-dd");
                }

                var amountEarned = pay.hoursWorked * employee.GetHourlyRate();
                if (amountByEmployeePerPeriod.ContainsKey(key))
                {
                    amountByEmployeePerPeriod[key] += amountEarned;
                }
                else
                {
                    amountByEmployeePerPeriod.Add(key, amountEarned);
                }
            }

            foreach(var employeePayPeriod in amountByEmployeePerPeriod)
            {
                var employeeReport = new EmployeeReportModel()
                {
                    employeeId = employeePayPeriod.Key.EmployeeId,
                    payPeriod = new PayPeriodModel()
                    {
                        startDate = employeePayPeriod.Key.startDate,
                        endDate = employeePayPeriod.Key.endDate
                    },
                    amountPaid = employeePayPeriod.Value.ToString("C", CultureInfo.CurrentCulture)
                };
                payrollReport.payrollReport.employeeReports.Add(employeeReport);
            }
            
            return payrollReport;
        }

        public async Task<IEnumerable<PayrollModel>> ParseAndValidatePayrollReportAsync(IFormFile payrollFile, int reportId)
        {
            List<PayrollModel> payrolls = new List<PayrollModel>();
            var report = await _reportRepository.GetReportByIdAsync(reportId);

            if (report != null)
                throw new Exception("Report already exists");

            if (payrollFile.Length > 0)
            {
                using (var reader = new TextFieldParser(new StreamReader(payrollFile.OpenReadStream())))
                {
                    reader.HasFieldsEnclosedInQuotes = true;
                    reader.SetDelimiters(",");
                    var header = reader.ReadFields();

                    while (!reader.EndOfData)
                    {
                        var line = reader.ReadFields();
                        payrolls.Add(new PayrollModel()
                        {
                            date = DateTime.ParseExact(line[0].ToString(), "d/M/yyyy", CultureInfo.InvariantCulture),
                            hoursWorked = Convert.ToDouble(line[1].ToString()),
                            employeeId = Convert.ToInt32(line[2].ToString()),
                            jobGroup = line[3].ToString()
                        });
                    }
                }
            }

            return payrolls;
        }

        public async Task CreateReportAsync(int reportId)
        {
            await _reportRepository.CreateReportAsync(reportId);
        }

        public async Task InsertPayrollsAsync(IEnumerable<PayrollModel> payrolls, int reportId)
        {
            foreach(var payroll in payrolls)
            {
                await _employeeRepository.AddEmployeeAsync(payroll.employeeId, payroll.jobGroup);
            }
            await _payrollRepository.InsertPayrollsAsync(payrolls, reportId);
        }
    }
}
